Feature: GP-64-SteamGames

As a user I would like to have the ability to add games from my steam to the gaming platform.

Most video game players have a steam account if they play on a personal computer, and it is 
common for them to own games on it. However, it would be very tedious and time consuming for
a user to add games to their list on our site manually. To help elevate this we want the user
to be able to input their steam information and them automatically be able to to see all the
games in their steam account. To do this we will need to user to be able to input their steam 
account information to our site and then submit it, after that our site will display the games 
they have on steam.

Background:
	Given the following users exist
	  | UserName   | Email                          | FirstName | LastName | Password   |
	  | TaliaK     | knott@example.com          | Talia     | Knott    | Password1! |
	  | ZaydenC    | clark@example.com          | Zayden    | Clark    | Password1! |
	  | DavilaH    | hareem@example.com     | Hareem    | Davila   | Password1! |
	And the following users do not exist
	  | UserName | Email               | FirstName | LastName | Password  |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a |

@Quinton
@LoggedIn
Scenario Outline: User can navigate to new steam games page
	Given I am a user with first name '<FirstName>'
	And I login
	When I click on the dropdown menu in the nav bar
	And I click on the Steam Games button in the navbar dropdown
	Then I am redirected to the 'Steam Games' page
	Examples:
	| FirstName | Page |
	| Talia     | Home |
	| Zayden    | Home |
	| Hareem    | Home |
