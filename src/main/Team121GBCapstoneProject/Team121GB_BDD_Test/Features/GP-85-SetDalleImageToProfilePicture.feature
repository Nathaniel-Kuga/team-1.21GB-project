@Nathaniel
Feature: GP-85-SetDalleImageToProfilePicture

**As a user, I would like the ability to set my AI generated image as my profile picture in the app so that  I don't have to manually upload my image.**

This user story is about stream lining the experience users have when they user our apps Dalle image generator service. The version this feature improves on
makes a user have to download the image and then manualy upload the image. This user story combines those steps into a single button click.

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
Scenario: A visitor cannot access the Dalle Image generation page
	Given I am not logged in
	And I am on the home page
	And I attempt to access the image generator page,
	Then I should be ask to login before accessing the page.

# make sure Talia has at least one credit for this test to pass
@LoggedIn 
Scenario: Talia can access the Dalle Image generation page and enter a prompt
	Given I am a logged in user with first name '<FirstName>'
	And I am on the home page
	And I navigate to the image generator page
	And I should see a counter telling me how many image credits I have that is 'Credits remaining: '
	And I input a prompt
	When I click the Generate Image Button
	Then My credits will decrease by 1
	Examples:
	| FirstName | 
	| Talia |

@LoggedIn
Scenario: Zayden can access the Dalle Image generation page but Zayden has no credits so they cannot generate an image
	Given I am a logged in user with first name '<FirstName>'
	And I am on the image generator page
	And I should see a counter telling me how many image credits I have that is 'Credits remaining: 0 You have used all of your free credits.'
	Then I will not be able click the Generate Image Button
	Examples:
	| FirstName | 
	| Zayden |

@LoggedIn
Scenario: A logged in user can access the Dalle Image generation page and set their profile image to the Dalle generated image
	Given I am a logged in user on the image generator page
	And I've generated an image
	When I click the button for setting it as my profile picture
	Then my profile picture will be updated to display the new profile image

#This prompt is violent on purpose so we can test the behavior the should be expected
@LoggedIn
Scenario: A logged in user can access the Dalle Image generation page but there was an error and they are notified
	Given I am a logged in user on the image generator page
	And I have  entered a prompt that is totally inappropriate 'InappropriatePrompt' 
	When I click the Generate Image Button
	Then I should be notified that something went wrong.
