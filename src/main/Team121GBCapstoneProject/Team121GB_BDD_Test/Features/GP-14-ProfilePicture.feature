@Cole
Feature: ProfilePicture

A user would like to change their profile picture. Either they just made their account or would like to update their old one. Either way they would 
navigate to their profile page and click on a button somewhere around their profile and have it open up the file explorer and let them choose an image.

Background:
	Given the following users exist
	  | UserName   | Email                          | FirstName | LastName | Password   |
	  | TaliaK     | knott@example.com          | Talia     | Knott    | Password1! |
	  | ZaydenC    | clark@example.com          | Zayden    | Clark    | Password1! |
	  | DavilaH    | hareem@example.com     | Hareem    | Davila   | Password1! |
	And the following users do not exist
	  | UserName | Email               | FirstName | LastName | Password  |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a |
	  | JoannaV  | valdezJ@example.com | Joanna    | Valdez   | d9u(*dsF4 |

@LoggedIn
Scenario Outline: Uploding Profile Picutre
	Given I am a user with first name '<FirstName>'
	When I login
	And I navigate to the '<Page>' page
	When I upload an image
	Then I should be able to select an image for my profile picture
	Examples:
	| FirstName | Page    |
	| Zayden    | Profile |
	| Hareem    | Profile |
	| Talia     | Profile |
