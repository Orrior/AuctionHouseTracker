﻿using System.Text.Json;
using AutoMapper;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.Auctions;
using WebApplication1.Utils;

namespace WebApplication1.Services.Auction;

public class AuctionRequests : IAuctionRequests
{
    private readonly string _region;
    private readonly string _locale;
    private readonly string _baseUrl;
    private readonly string _token;
    private readonly ILogger<AuctionRequests> _logger;
    private readonly IMapper _mapper;

    public AuctionRequests(ILogger<AuctionRequests> logger, IConfiguration configuration, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;

        _region = configuration["RequestParameters:Region"];
        _locale = configuration["RequestParameters:Locale"];

        _token = WoWAuthenticator.GetToken(
            configuration["OAuth2Credentials:ClientId"],
            configuration["OAuth2Credentials:ClientSecret"]).Result;

        _baseUrl = $"https://{_region}.api.blizzard.com";
    }

    public List<WowAuthenticatorRecords.RealmData> GetRealmNames()
    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };
        var client = new RestClient(options);
        var request = new RestRequest($"/data/wow/realm/index");
        request.AddParameter("namespace", $"dynamic-{_region}");
        request.AddParameter("locale", _locale);

        return client.Get<WowAuthenticatorRecords.Realms>(request).realms;
    }

    public List<WowAuthenticatorRecords.RealmData> GetConnectedRealm(string connectedRealmId)
    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };
        var client = new RestClient(options);
        var request = new RestRequest($"/data/wow/connected-realm/{connectedRealmId}");
        request.AddParameter("namespace", $"dynamic-{_region}");
        request.AddParameter("locale", _locale);
        
        var response = client.Get<WowAuthenticatorRecords.ConnectedRealmsIndex>(request);

        return response?.Realms ?? new List<WowAuthenticatorRecords.RealmData>();
    }
    
    public Dictionary<string, List<WowAuthenticatorRecords.RealmData>> GetConnectedRealms()
    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };
        var client = new RestClient(options);
        var request = new RestRequest($"data/wow/connected-realm/index");
        request.AddParameter("namespace", $"dynamic-{_region}");
        request.AddParameter("locale", _locale);

        var realmDatas = client.Get<WowAuthenticatorRecords.ConnectedRealms>(request);
        var realmdIds = realmDatas?.connectedRealms.Select(
            x => x.Values.First().Substring(53,4).Replace("?","")).ToList();

        Dictionary<string, List<WowAuthenticatorRecords.RealmData>> result =
            new Dictionary<string, List<WowAuthenticatorRecords.RealmData>>();

        foreach (var connectedRealmId in realmdIds)
        {
            request.Resource = $"/data/wow/connected-realm/{connectedRealmId}";

            var response = client.Get<WowAuthenticatorRecords.ConnectedRealmsIndex>(request);
            
            result.Add(connectedRealmId, response.Realms);
        }

        return result;
    }

    public async Task<List<WowAuthenticatorRecords.AuctionSlotNonCommodity>> GetNonCommodities(string realmId)
    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };
        var client = new RestClient(options);
        var request = new RestRequest($"/data/wow/connected-realm/{realmId}/auctions");
        request.AddParameter("namespace", $"dynamic-{_region}");
        request.AddParameter("locale", _locale);

        var result = (await client.GetAsync<WowAuthenticatorRecords.AuctionRequestNonCommodity>(request))?.AuctionSlots
                     ?? new List<WowAuthenticatorRecords.AuctionSlotNonCommodity>();
        
        return result;
    }

    public async Task<List<WowAuthenticatorRecords.AuctionSlotCommodity>> GetCommodities()
    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };
        var client = new RestClient(options);

        var request = new RestRequest($"/data/wow/auctions/commodities");
        request.AddParameter("namespace", $"dynamic-{_region}");
        request.AddParameter("locale", _locale);

        return (await client.GetAsync<WowAuthenticatorRecords.AuctionRequestCommodity>(request))?.AuctionSlots
               ?? new List<WowAuthenticatorRecords.AuctionSlotCommodity>();
    }

    public async Task<List<WowAuthenticatorRecords.AuctionSlotNonCommodity>> GetCheapestNonCommodities(string realmId)
    {
        var nonCommodities = await GetNonCommodities(realmId);

        var dict = new Dictionary<long, WowAuthenticatorRecords.AuctionSlotNonCommodity>();

        //TODO! Premature optimisation is the root of all evil.
        foreach (var nonCommodity in nonCommodities)
        {
            var itemId = nonCommodity.Item.Id;
            var item = nonCommodity;
            item.realmId = Int64.Parse(realmId);
            
            if (!dict.ContainsKey(itemId))
            {
                dict.Add(itemId, nonCommodity);
            }
            else if (dict[itemId].BuyOut > nonCommodity.BuyOut)
            {
                dict[itemId] = nonCommodity;
            }
        }

        return dict.Values.ToList();
    }

    public async Task<List<WowAuthenticatorRecords.AuctionSlotCommodity>> GetCheapestCommodities()
    {
        var allCommodities = await GetCommodities();

        var dict = new Dictionary<long, WowAuthenticatorRecords.AuctionSlotCommodity>();

        foreach (var commodity in allCommodities)
        {
            var itemId = commodity.Item.Id;

            if (!dict.ContainsKey(itemId))
            {
                dict.Add(itemId, commodity);
            }
            else if (dict[itemId].UnitPrice > commodity.UnitPrice)
            {
                dict[itemId] = commodity;
            }
        }

        return dict.Values.ToList();
    }

    public async Task<List<CommodityInfo>> GetCommodityInfos() {
        
        //TODO Remove range copy!
        var comItems = await GetCheapestCommodities();

        var itemInfos = await GetItemInfos(comItems.Select(x => x.Item.Id).ToList());

        var comInfosMapped = _mapper.Map<List<CommodityInfo>>(itemInfos);

        return comInfosMapped;
    }
    
    public async Task<List<NonCommodityInfo>> GetNonCommodityInfos(string realmId) {
        var nonComItems = await GetCheapestNonCommodities(realmId);

        var itemInfos = await GetItemInfos(nonComItems.Select(x => x.Item.Id).ToList());

        var comInfosMapped = _mapper.Map<List<NonCommodityInfo>>(itemInfos);

        return comInfosMapped;
    }
    
    public WowAuthenticatorRecords.ItemInfo? GetItemInfo(long itemId)
        // Returns null if something went wrong
    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };

        var client = new RestClient(options);

        var request = new RestRequest($"/data/wow/item/{itemId}");
        request.AddParameter("namespace", $"static-{_region}");
        request.AddParameter("locale", _locale);

        var response = new WowAuthenticatorRecords.ItemInfo();
        try
        {
            response = client
                .Get<WowAuthenticatorRecords.ItemInfo>(
                    request); //TODO!!! Make record for this thing. We don't need so much data!!!.
        }
        catch (Exception e)
        {
            _logger.LogError($"AN ERROR HAS OCCURED WITH AN ITEM ID \"{itemId}\". \r\n {e.Message}");

            response = null;
        }
        
        return response;
    }

    public Task<List<WowAuthenticatorRecords.ItemInfo>> GetItemInfos(List<long> itemIds)

    {
        var options = new RestClientOptions(_baseUrl)
            { Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_token, "Bearer") };

        var client = new RestClient(options);
        var itemInfos = new List<WowAuthenticatorRecords.ItemInfo>();

        var request = new RestRequest();
        request.AddParameter("namespace", $"static-{_region}");
        request.AddParameter("locale", _locale);
        
        for (int i = 0; i < itemIds.Count; i++)
        {
            var itemId = itemIds[i];
            WowAuthenticatorRecords.ItemInfo? res;
            request.Resource = $"/data/wow/item/{itemId}";

            // If something fails, try to get item manually, if fails anyways, returns with null.
            try
            {
                res = client.Get<WowAuthenticatorRecords.ItemInfo>(request);

                if (res?.Name == null)
                {
                    _logger.LogError($"Item with id'{itemId}' wasn't found.");
                    continue;
                }
                
                itemInfos.Add(res);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong with item \"{itemId}\". \r\n {e.Message}");
                res = GetItemInfo(itemId); //If error was caused due to API bottleneck, try to fetch again, but slowly.
                if (res != null)
                {
                    itemInfos.Add(res);
                }

            }
            _logger.LogDebug($"PROGRESS: {i + 1}/{itemIds.Count}");
        }
        _logger.LogInformation($"Items scanning has finished! Scanned {itemInfos.Count}/{itemIds.Count} items!");
        return Task.FromResult(itemInfos);
    }
}