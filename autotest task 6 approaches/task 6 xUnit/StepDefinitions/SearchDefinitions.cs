﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll.xUnit;
using task_6_xUnit;
using Reqnroll;
using Serilog;
using Shouldly;
using Xunit;
using OpenQA.Selenium;

namespace task_6_xUnit.StepDefinitions
{
    [Binding]
    public class SearchDefinitions
    {
        private IWebDriver _driver;
        private MainPage _mainPage;

        [Given("the user is on the main page")]
        public void GivenTheUserIsOnTheMainPage()
        {
            _driver = WebDriverSingleton.GetDriver();

            _mainPage = new MainPageBuilder(_driver).Build();

            Log.Debug("starting SearchTest");
        }

        [When("the user enters search programs and clicks the search button")]
        public void WhenTheUserEntersSearchProgramsAndClicksTheSearchButton()
        {
            Log.Information("entering 'study programs'");
            _mainPage.Search("study programs");
        }


        [Then("the user should be redirected to search results page")]
        public void ThenTheUserShouldBeRedirectedToSearchResultsPage()
        {
            try
            {
                _driver.Url.ShouldBe("https://en.ehu.lt/?s=study+programs");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Assertion failed in SearchTest");
                Assert.Fail("SearchTest failed: " + ex.Message);
            }
        }

    }
}
