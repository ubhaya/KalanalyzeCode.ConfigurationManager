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
	
Scenario: Log in user can log out
	Given User that logged in
	When User tried to logged out
	Then User should be able to log out
	
Scenario: User tries to log in with invalid credential
	Given User that not log in
	And Navigate to login page
	When User input incorrect credential
	| Email | Password  |
	| bob   | Pass123$% |
 	Then User should see a error