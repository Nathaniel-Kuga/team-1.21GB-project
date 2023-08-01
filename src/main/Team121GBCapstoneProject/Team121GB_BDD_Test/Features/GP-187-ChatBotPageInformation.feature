@Nathaniel
Feature: GP-187-ChatBotPageInformation

**As a user I would like to have more information about the features and functionality of the Chat bot page on so that I can better use the page.**

This user story gives users information what the chatbot page is for, and how to use it.

Background:
	Given the following users exist
		| UserName | Email              | FirstName | LastName | Password   |
		| TaliaK   | knott@example.com  | Talia     | Knott    | Password1! |
		| ZaydenC  | clark@example.com  | Zayden    | Clark    | Password1! |
		| DavilaH  | hareem@example.com | Hareem    | Davila   | Password1! |
	And the following users do not exist
		| UserName | Email             | FirstName | LastName | Password  |
		| AndreC   | colea@example.com | Andre     | Cole     | 0a9dfi3.a |

@loggedIn
Scenario: A Logged in user should be able to see a description of the chatbot page
	Given I am a logged in user
	And I am on the home page
	When I navigate to the chatbot page
	Then I should see a description of what the page is for

@loggedIn
Scenario: A logged in user should be able to see information about the limitations of the chatbot page
	Given I am a logged in user
	And I am on the home page
	When I navigate to the chatbot page
	Then I should see information about the limitations of the chat bot