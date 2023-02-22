using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;
using WebApplication1.Utils;

Console.WriteLine("====================");
Console.WriteLine("====================");
Console.WriteLine("====================");
Console.WriteLine("DEBUG START");
// string token = WoWAuthenticator.GetToken().Result;
string token = "EUEL1OgufxTQWkPCcRMZ3SsAWbhwUfNXgC";
Console.WriteLine($"token: {token}");
Console.WriteLine($"CHECK TOKEN VALIDITY: {WoWAuthenticator.CheckToken(token).Result}");

List < WowAuthenticatorHelper.AuctionSlot > test;
DateTime timer;

Console.WriteLine("START COMMODITIES REQUEST");

timer = DateTime.Now;
test = WoWAuthenticator.GetNonCommodities(token).Result;
Console.WriteLine($"GET NON-COMMODITY OBJECTS FROM AH: {test.Count}");
Console.WriteLine($"TIME TOOK:{(DateTime.Now - timer).Milliseconds} ms");
Console.WriteLine("START NON COMMODITIES REQUEST");

timer = DateTime.Now;
test = WoWAuthenticator.GetCommodities(token).Result;
Console.WriteLine($"GET COMMODITY OBJECTS FROM AH: {test.Count}");
Console.WriteLine($"TIME TOOK:{(DateTime.Now - timer).Milliseconds} ms");


Console.WriteLine("====================");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
