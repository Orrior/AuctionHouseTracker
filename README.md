# AuctionHouseTracker
Tool to track prices and history in wow auction house!

This project is part of a bachelor's degree thesis.
The purpose of the application is to track the history of prices in selected realms and regions of World of Warcraft.

TODO:
##### REQUEST PHASE
1. Add ability to make request to World of Warcraft API: https://develop.battle.net/documentation/world-of-warcraft/game-data-apis. <br>
	1.1 Get Oauth2.0 Token (Done!) <br>
	1.2 Add check whether token is still valid. (Done!) <br>
	1.3 Make request price list for non-commodity items<br>(Done!)
	1.3 Make request price list for commodity items<br>(Done!)
	1.4 Parse Data. <br>(Done!)
	1.5 Make requests for commodity items data (realm-wise items)<br>
	1.5 Make requests for non-commodity items data (realm-wise items)<br>
	1.6 Parse Data. <br>
2. Take All credentials that needed to make request working from **appsettings.json**
3. Make so you can chose which realms and region you're going to request via **appsettings.json**
4. Make SLOW item data requesting (8 requests/sec?) due to max request frequency limitation is 9 request/sec 
<br> NB!**^This will be ON-DEMAND AND NOT AUTOMATIC**
##### DATABASE PHASE
1. Choose database (POSTRES or TIMESCALEDB?)
2. Implement it, check possibilities for time-seires database, and whether i need it or not.
3. Make models/repos for Database.
4. Make so they store price data separate for each region/realm separately.
5. Make so item data stored locally. 
##### MIDDLE phase
1. Make data polling every hour using background tasks feature: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio
2. Make so admin can request item (also make price request manually?)
3. **IF EXTRA TIME!** Implement search for specific items or by categories?
### LATE PHASE
1. make visual table for items.
2. **IF VERY EXTRA TIME!** try to implement wow-ish tooltips for items: https://www.wowhead.com/tooltips (I don't know Javascript so it will be really time-consuming.) 
