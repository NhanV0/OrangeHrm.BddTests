@login @ui
Feature: Login
As a user of the OrangeHRM application
I want to be able to log in to the application
So that only authorized users can access the system

Background:
  Given I am on the OrangeHRM login page

  @smoke @positive @TC-LOGIN-01
  Scenario: Successful login with valid admin credentials
    When I log in with valid admin credentials
    Then I should be redirected to the dashboard page
    And The dashboard page header should be "Dashboard"
    And The user menu should be visible

  @smoke @negative @TC-LOGIN-02
  Scenario: Login is rejected when the password is wrong
    When I log in with username "Admin" and password "wrong_password_123"
    Then I should see an error message "Invalid credentials"
    And I should remain on the login page
