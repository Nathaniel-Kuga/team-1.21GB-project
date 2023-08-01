@Nathaniel
Feature: GP-59-AiChatBot

**As a user, I would like to be able to access a chat bot so that I can get help when I am stuck on my games.**

This feature is about using the power of AI to help our users when they need help while gaming. It gives our users one more tool 
at their disposal.
Background:
	Given the following users exist
	  | UserName   | Email                          | FirstName | LastName | Password   |
	  | TaliaK     | knott@example.com          | Talia     | Knott    | Password1! |
	  | ZaydenC    | clark@example.com          | Zayden    | Clark    | Password1! |
	  | DavilaH    | hareem@example.com     | Hareem    | Davila   | Password1! |
	And the following users do not exist
	  | UserName | Email               | FirstName | LastName | Password  |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a |

@notLoggedIn
Scenario: A visitor cannot access the AI Chat bot page.
	Given I am a visitor on the Home page
	And I am on the home page
	When I try to navigate to the 'ChatGPT page'
	Then I should be ask to login before accessing the page.

@notLoggedIn
Scenario: A visitor cannot see a button for the AI Chat bot page in the nav bar
	Given I am a visitor
	And I am on the home page
	Then I should not see a button for the Chat bot page

@loggedIn
Scenario: A logged in user can access the Chat bot page
	Given I am a logged in user
	And I am on the home page
	When I click on the chatbot button
	Then I should be redirected to the chatbot page

@loggedIn
Scenario: A logged in user can submit a prompt on the Chat bot page
	Given I am a logged in user
	And I am on the chatbot page
	When I enter a prompt
	And I submit my prompt
	Then I should see a response to my prompt

@loggedIn
Scenario: A logged in user can enter an inapprorate prompt
	Given I am a logged in user
	And I am on the chatbot page
	When I enter an inappropriate prompt
	And I submit my prompt
	Then I should be told my prompt was inappropriate
	