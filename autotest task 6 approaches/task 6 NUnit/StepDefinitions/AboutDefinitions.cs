using OpenQA.Selenium;
using Reqnroll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using task_6_NUnit;

namespace task_6_NUnit.StepDefinitions
{
    [Binding]
    public class AboutDefinitions
    {
        private IWebDriver _driver;
        private MainPage _mainPage;


        [Given("the user goes to the website")]
        public void GivenTheUserGoesToTheWebsite()
        {
            _driver = WebDriverSingleton.GetDriver();

            _mainPage = new MainPageBuilder(_driver).Build();

            Log.Debug("starting AboutTest");
        }

        [When("the user clicks the about button")]
        public void WhenTheUserClicksTheAboutButton()
        {
            Log.Debug("starting AboutTest");
            _mainPage.NavigateToAbout();
        }


        [Then("the user should be redirected to about page")]
        public void ThenTheUserShouldBeRedirectedToAboutPage()
        {
            Log.Information("asserting current url");
            _driver.Url.ShouldBe("https://en.ehu.lt/about/");
        }
    }
}
