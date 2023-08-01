@Nathaniel 
Feature: GP-193-WhisperIntegrationWithChatGpt

** As user I would like to be able speak questions to the chat bot, so that I do not have to physically type questions by hand. **
This feature will allow the user to speak to the chat bot and have the chat bot respond to the user's questions. It gives the user 
another medium to be able to communicate with the chat bot.
Background:
	Given the following users exist
	  | UserName   | Email                          | FirstName | LastName | Password   |
	  | TaliaK     | knott@example.com          | Talia     | Knott    | Password1! |
	  | ZaydenC    | clark@example.com          | Zayden    | Clark    | Password1! |
	  | DavilaH    | hareem@example.com     | Hareem    | Davila   | Password1! |
	And the following users do not exist
	  | UserName | Email               | FirstName | LastName | Password  |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a |

@LoggedIn
Scenario: A logged can click the start and stop button to start and stop the microphone
	Given I am a logged in user
	And I am on the home page
	When I navigate to the chatbot page
	Then I should see buttons for using speech to text.

# Other acceptance criteria require the user to speak, and I am not sure how to test that.