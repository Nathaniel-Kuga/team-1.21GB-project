@Quinton
Feature: As a visitor, I want to be able to run a basic web application on localhost so that I know that the website exists

This feature is used to create a simple web application that can be ran even if it is small. In the section there 
will be a small home page that simply displays the title of the page and a small decription.

Scenario: Home page title contains Home Page
	Given I am a visitor
	When I am on the "Home" page
	Then The page title contains "Home Page"

#Scenario: Home page has a Create Account button
#	Given I am a visitor
#	When I am on the "Home" page
#	Then The page presents a Create Account button

#Scenario: Home page has a Log In button
#	Given I am a visitor
#	When I am on the "Home" page
#	Then The page presents a Login button
