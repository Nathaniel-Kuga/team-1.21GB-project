@Nathaniel
Feature: GP-34-ViewTop100Games

** As a user, I would like to view the top 100 games on the website so that I can know what games are good **

This feature is about giving users a way to see the top rated games on our application. 


Scenario: A visitor can navigate to the Top 100 Games page
	Given I am a visitor 
	When When I click the "Top 100 Games Page" button
	Then I should be redirected to the "Top 100 Games Page"
