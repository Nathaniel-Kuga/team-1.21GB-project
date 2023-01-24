# Steam Web API

There are four total web APIs provided by steam.   
1) ISteamNews (This API provides news information about each steam Game)
2) ISteamUserStats (This API provides global stats about each Game)
3) ISteamUser (This API provides information about a user)
4) ITFItems_440 (This API provides information about player item data)

----------------------------------------------------------------
We are the most interested in using the News, UserStats, and User APIs. They can provide helpful information to our users on steam.
They can also potentially help us to provide suggestions based on a users backlog.
Valve makse the Steam Web APIs free to use as long as you read the terms use and create an account. 
I have thusfar been unable to fully test the functionality of these APIs because the API requires a domain name for the website that you will be using the API for.

Here is are two stock examples they provide in documentation.

1) [ISteamNews](http://api.steampowered.com/ISteamNews/GetNewsForApp/v0002/?appid=440&count=3&maxlength=300&format=json)
2) [ISteamUserStats Example](http://api.steampowered.com/ISteamUserStats/GetGlobalAchievementPercentagesForApp/v0002/?gameid=440&format=json)

The only hold up as of now is that we cannot fully test these APIs to see how effectively we can use them because we don't have a website domain yet.


[Steam Web API Documentation](https://steamcommunity.com/dev)

[Steam Web API Terms of Use](https://steamcommunity.com/dev/apiterms)