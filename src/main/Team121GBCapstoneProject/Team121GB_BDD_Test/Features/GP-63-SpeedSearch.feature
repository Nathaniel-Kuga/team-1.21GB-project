Feature: GP-63-SpeedSearch

Currently on the site a user can add games one at a time to their collection of games, but they will have to search 
them manually one by one. This can take a lot of time and effort when either first setting up your account or updating 
your collection after a long period of time. The way this can be prevented is to allow the user to type in their games 
in a text box and we search the games for them. This would be done with a large textbox and the user will have to enter 
in a list of games with each game being on a new line. Once they click the submit button they will then see a list of 
games we got back as a result. The user can then add them in by clicking the button to add to a list similar to the search 
feature.

Background:
	Given the following users exist
	  | UserName   | Email                          | FirstName | LastName | Password   |
	  | TaliaK     | knott@example.com				| Talia     | Knott    | Password1! |
	  | ZaydenC    | clark@example.com				| Zayden    | Clark    | Password1! |
	  | DavilaH    | hareem@example.com				| Hareem    | Davila   | Password1! |
	And the following users do not exist
	  | UserName | Email               | FirstName | LastName | Password  |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a |
	  | JoannaV  | valdezJ@example.com | Joanna    | Valdez   | d9u(*dsF4 |

@Quinton
@LoggedIn
Scenario Outline: Logged in user can navigate to speedsearch page 
	Given I am a user with first name '<FirstName>'
	And I login
	And I am redirected to the '<Page>' page
	And I click the profile dropdown
	When I click SpeedSearch
	Then I will go to the SpeedSearch Page
	Examples:
	| FirstName | Page |
	| Talia     | Home |

#Scenario Outline: User input invalid data for speedsearch 
#	Given I am a user with first name '<FirstName>'
#	And I login
#	And I am on the SpeedSearch Page
#	When enter in a blank list
#	And click the submit button
#	Then I will be shown a error
#	Examples:
#	| FirstName | Page |
#	| Talia     | Home |

#Scenario Outline: User input valid data for speedsearch
#	Given I am a user with first name '<FirstName>'
#	And I login
#	And I am on the SpeedSearch Page
#	When I enter in valid data
#	And click the submit button
#	Then I will go the the Results page
#	Examples:
#	| FirstName | Page |
#	| Talia     | Home |