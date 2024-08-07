@Login
Feature: Login
	User tries to log in
	
Scenario: User tries to log in with valid credential
	Given User that not log in
	And Navigate to login page
	When User input correct credential
	|Email	|Password|
 	|bob	|Pass123$|
	Then User should be able to login