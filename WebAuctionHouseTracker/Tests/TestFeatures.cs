using System.Text.Json;
using Microsoft.AspNetCore.Routing.Matching;
using WebApplication1.Services;
using WebApplication1.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Tests;

public class TestFeatures
{
    public static void Main()
    {
        
        Console.WriteLine("====================");
        Console.WriteLine("DEBUG START");
        string token = WoWAuthenticator.GetToken("ef3e3d3c1bf843528e66c9db9b640ebb","Gn4BDUrgAB0s6FzOwOfYLKai5AipYrxG").Result;
        
        Console.WriteLine($"token: {token}");
        
        // var test = new AuctionRequests("eu", "en_US", token);
        
        
        // //Scan all commodity items and add them to DB.
        // var comItems = test.GetCheapestCommodities().Result;
        //
        // Console.WriteLine(comItems);
        //
        // var comInfos = test.GetItemInfos(new List<WowAuthenticatorRecords.AuctionSlotBase>(comItems));

        // var testList = test.GetCheapestCommodities().Result.GetRange(0, 100);
        //
        // var timerStart = DateTime.Now;
        // for (int i = 0; i < testList.Count ; i++)
        // {
        //     var item = test.GetItemInfo(testList[i].Item.Id);
        //     Console.WriteLine(JsonSerializer.Serialize(item));
        //     Console.WriteLine($"PROGRESS: {i+1}/{testList.Count}");
        // }
        //
        // var timerEnd = DateTime.Now - timerStart;
        // Console.WriteLine($"TIME TO REQUEST1: {timerEnd}");
        // timerStart = DateTime.Now;
        //
        // var data = test.GetItemInfos(testList.Select(x => x.Item.Id).ToList()).Result;
        // timerEnd = DateTime.Now - timerStart;
        // Console.WriteLine($"TIME TO REQUEST2: {timerEnd}");
        // Console.WriteLine(data.Count);
        
        





        //
        // Console.WriteLine("GetCommodities() : " + test.GetCommodities().Result.Count);
        // Console.WriteLine("GetCheapestCommodities() : " + test.GetCheapestCommodities().Result.Count);
        //
        // // Console.WriteLine(JsonSerializer.Serialize(test.GetCheapestCommodities()));
        //
        // const string realmId = "3391";
        //
        // Console.WriteLine("GetNonCommodities() : " + test.GetNonCommodities(realmId).Result.Count);
        // Console.WriteLine("GetCheapestCommodities() : " + test.GetCheapestNonCommodities(realmId).Result.Count);

        
        
        
        // var timer = DateTime.Now;
        //
        // var list = WoWAuthenticator.GetCommodities(token).Result;
        //
        // Console.WriteLine($"GET COMMODITIES: {DateTime.Now - timer}");
        //
        // Console.WriteLine($"TOTAL OBJECTS: {list.Count}");
        // Console.WriteLine("EXAMPLE LIST:");
        // Console.WriteLine("----------------");
        // for (int i = 0; i < 5; i++)
        // {
        //     var item = list[i];
        //     Console.WriteLine($"AH SLOT: #{i+1}: {JsonSerializer.Serialize(item)}");
        //     // Console.WriteLine($"ITEM INFO: {WoWAuthenticator.GetItemInfo(item.Item.Id.ToString(), token).Result}");
        //     
        //     Thread.Sleep(250);
        // }
        
        // timer = DateTime.Now;
        //
        // var list2 = WoWAuthenticator.GetNonCommodities(token).Result;
        //
        // Console.WriteLine($"GET NON COMMODITIES: {DateTime.Now - timer}");
        //
        // Console.WriteLine($"TOTAL OBJECTS: {list2.Count}");
        // Console.WriteLine("EXAMPLE LIST:");
        // Console.WriteLine("----------------");
        // for (int i = 0; i < 5; i++)
        // {
        //     var item = list2[i];
        //     Console.WriteLine($"AH SLOT: #{i+1}: {JsonSerializer.Serialize(item)}");
        //     // Console.WriteLine($"ITEM INFO: {WoWAuthenticator.GetItemInfo(item.Item.Id.ToString(), token).Result}");
        //     
        //     Thread.Sleep(250);
        // }

        Console.WriteLine("====================");
    }
}