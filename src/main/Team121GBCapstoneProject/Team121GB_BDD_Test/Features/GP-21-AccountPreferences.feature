@Nathaniel
Feature: GP-21-AccountPreferences
** As a user, I want to be able to see my account preferences so that I can customize my account ** 

This feature ensures that user who is logged in can see and customize their account preferences on a 
dedicated web page.

Background:
	Given the following users exist
	  | UserName   | Email                          | FirstName | LastName | Password   |
	  | TaliaK     | BDDTesting1@gmail.com          | Talia     | Knott    | Password1! |
	  | ZaydenC    | BDDTesting2@gmail.com          | Zayden    | Clark    | Password1! |
	  | DavilaH    | team121gigabytes@gmail.com     | Hareem    | Davila   | Password1! |
	And the following users do not exist
	  | UserName | Email               | FirstName | LastName | Password  |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a |
	  | JoannaV  | valdezJ@example.com | Joanna    | Valdez   | d9u(*dsF4 |

@LoggedIn
Scenario Outline: Logged in user can click on their name in the navbar
	Given I am a logged in user with Firstname '<FirstName>'
	And I am on the '<Page>' page 
	When I click my name in the navbar
	Then I should be redirected to the '<ProfilePage>' page
	Examples: 
	| FirstName | Page | ProfilePage |
	| Talia     | Home | Profile     |
	| Zayden    | Home | Profile     |
	| Hareem    | Home | Profile     |



