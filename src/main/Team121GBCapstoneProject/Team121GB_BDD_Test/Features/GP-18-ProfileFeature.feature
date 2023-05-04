@Cole
Feature: GP-14

A user would like to change their profile picture. Either they just made their account or would like to update their old one. 
Either way they would navigate to their profile page and click on a button somewhere around their profile 
and have it open up the file explorer and let them choose an image.


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
Scenario Outline: Finding Profile Name Boxes
	Given I am a user with first name '<FirstName>'
	When I login
	And I navigate to the '<Page>' page
	Then I should see a form with a text box to edit my profile name and a submit button.
Examples:
	| FirstName | Page    |
	| Talia     | Profile |
	| Zayden    | Profile |
	| Hareem    | Profile |

Scenario Outline: Changing Profile Name
	Given I am a user with first name '<FirstName>'
	When I login
	And I navigate to the '<Page>' page
	And the I type in valid input
	And I submit the form for editing my profile name.
	Then I should see the change reflected on page reload.
Examples:
	| FirstName | Page    |
	| Talia     | Profile |
	| Zayden    | Profile |
	| Hareem    | Profile |

Scenario Outline: Invalid Profile Name
	Given I am a user with first name '<FirstName>'
	When I login
	And I navigate to the '<Page>' page
	And then I type in invalid input
	And I submit the form for editing my profile name.
	Then I should see a notification telling my the input is invalid.
	Examples:
	| FirstName | Page    |
	| Talia     | Profile |
	| Zayden    | Profile |
	| Hareem    | Profile |