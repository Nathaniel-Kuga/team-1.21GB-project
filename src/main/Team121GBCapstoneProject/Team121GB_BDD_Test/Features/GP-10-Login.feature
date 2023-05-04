@Quinton
Feature: User Logins
**As a user, I want to be able to create an authorized account so that I can use more features of the site**

This feature ensures that users who have previously registered can successfully login and see a personalized message
that confirms they are recognized by the application and logged in.  It also *defines* a set of seeded users for 
future software test engineers to use when performing other kinds of tests.

The steps we define here can be re-used when testing the *register* feature.

To generate living documentation, create a Documentation folder and then run one of these from the project dir: 
    `livingdoc test-assembly -t bin\Debug\net7.0\TestExecution.json -o Documentation bin\Debug\net7.0\Standups_BDD_Tests.dll`
    `livingdoc feature-folder -t bin\Debug\net7.0\TestExecution.json -o Documentation .`

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
Scenario Outline: Existing user can login
	Given I am a user with first name '<FirstName>'
	When I login
	Then I am redirected to the '<Page>' page
	  And I can see a personalized message in the navbar that includes my email
	Examples:
	| FirstName | Page |
	| Talia     | Home |
	| Zayden    | Home |
	| Hareem    | Home |

Scenario Outline: Non-user cannot login
	Given I am a user with first name '<FirstName>'
	When I login
	Then I can see a login error message
	Examples:
	| FirstName |
	| Andre     |
	| Joanna    |

# Need to do this one after logging in to have any cookies
@support
Scenario: We can save cookies
	Given I am a user with first name 'Talia'
	When I login
	Then I can save cookies

@support
Scenario: We can log in with only a cookie
	Given I am a user with first name 'Talia'
		# we need to start on a page that has the same domain as the one we originally set the cookie on, so go there first
		And I am on the "Home" page
	When I load previously saved cookies
		And I am on the "Home" page
	Then I can see a personalized message in the navbar that includes my email