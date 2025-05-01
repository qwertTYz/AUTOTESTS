Feature: xUnitFeatures


Scenario: About
     Given the user goes to the website
     When the user clicks the about button
     Then the user should be redirected to about page

Scenario: Search
       Given the user is on the main page
       When the user enters search programs and clicks the search button
       Then the user should be redirected to search results page

Scenario: LanguageSwitcher
       Given the user is on the english language version page
       When the user switches the language
       Then the user should be redirected to selected language page

Scenario: ContactInfo
        Given the user is on the page
        When the user clicks the contactInfo button
        Then the user should be redirected to contactInfo page