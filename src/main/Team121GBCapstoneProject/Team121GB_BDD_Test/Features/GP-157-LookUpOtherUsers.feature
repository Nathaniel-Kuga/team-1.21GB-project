@Cole
Feature: GP-157-LookUpOtherUsers

A user will be able to go to a page (either their profile page or a new page) and search for other users. 
When they click on one of the names that were brought up they will be taken to the person's profile page and 
should be able to see their first and last name, their bio, and a list of their games if they have any.

Background:
	Given the following users exist
		| UserName | Email              | FirstName | LastName | Password   |
		| TaliaK   | knott@example.com  | Talia     | Knott    | Password1! |
		| ZaydenC  | clark@example.com  | Zayden    | Clark    | Password1! |
		| DavilaH  | hareem@example.com | Hareem    | Davila   | Password1! |
	And the following users do not exist
		| UserName | Email             | FirstName | LastName | Password  |
		| AndreC   | colea@example.com | Andre     | Cole     | 0a9dfi3.a |

@LoggedIn
Scenario Outline: User can find button on page
	Given I am a user with first name '<FirstName>'
	And I login
	And I navigate to the '<Page>' page
	Then I should see a button to find users
	And I click on it it will take me to the "/Home/FindFriends" page
Examples:
	| FirstName | Page |
	| Talia     | Profile |
	| Zayden    | Profile |
	| Hareem    | Profile |

Scenario Outline: User can look up other users
	Given I am a user with first name '<FirstName>'
	And I login
	And I navigate to the '<Page>' page
	When I look up a valid user
	Then I will see their information
Examples:
	| FirstName | Page |
	| Talia     | Friend |
	| Zayden    | Friend |
	| Hareem    | Friend |

Scenario Outline: User can look up invlaid users and have nothing be returned
	Given I am a user with first name '<FirstName>'
	And I login
	And I navigate to the '<Page>' page
	When I look up an invalid user
	Then I will see an "Person Not Found" error
Examples:
	| FirstName | Page |
	| Talia     | Friend |
	| Zayden    | Friend |
	| Hareem    | Friend |
