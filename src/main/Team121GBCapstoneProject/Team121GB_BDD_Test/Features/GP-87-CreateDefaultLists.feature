@Nathaniel
Feature: GP-87-CreateDefaultLists

** As a user I would like the ability to create list so that I can add games to them and view those lists. **

This feature gives users three lists by default when they create an account with our app.
These three lists are Currently Playing, Completed, Want To Play
Background:
	Given the following users exist
		| UserName | Email             | FirstName | LastName | Password   |
		| TaliaK   | knott@example.com | Talia     | Knott    | Password1! |

@LoggedIn
Scenario Outline: A Logged in user can navigate to their Library page
	Given I am a logged in user with Firstname '<FirstName>'
	When I click on the dropdown menu in the nav bar
	And I click on the Game Lists button in the navbar dropdown
	Then I should be redirected to the '<Page>'page
Examples:
	| FirstName | Page        |
	| Talia     | Games Lists |


@LoggedIn
Scenario Outline: A Logged in user can see their default lists on their library page
	Given I am a logged in user with Firstname '<FirstName>'
	And I am on the '<Games Lists>' page
	Then I should see my default lists display
Examples:
	| FirstName | Page        |
	| Talia     | Games Lists |
