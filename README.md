# AuctionHouseTracker
Tool to track prices and history in wow auction house!

This project is part of a bachelor's degree thesis.
The purpose of the application is to track the history of prices in selected realms and regions of World of Warcraft.

TODO:
===REQUEST PHASE===
1. Add ability to make request to World of Warcraft API: https://develop.battle.net/documentation/world-of-warcraft/game-data-apis.
2. Take All credentials that needed to make request working from **appsettings.json**
3. Make so you can chose which realms and region you're going to request via **appsettings.json**
4. Make SLOW item data requesting (8 requests/sec?) due to max request frequency limitation is 9 request/sec - **This will be ON-DEMAND AND NOT AUTOMATIC**
===DATABASE PHASE===
1. Choose database (POSTRES or TIMESCALEDB?)
2. Implement it, check possibilities for time-seires database, and whether i need it or not.
3. Make models/repos for Database.
4. Make so they store price data separate for each region/realm separately.
5. Make so item data stored locally. 
===IMPLEMENTING AUTOMATIC REGULAR DATA REQUEST phase===
1. Make data polling every hour using background tasks feature: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio
