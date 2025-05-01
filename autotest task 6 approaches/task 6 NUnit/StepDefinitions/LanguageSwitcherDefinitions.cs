using OpenQA.Selenium;
using Reqnroll;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Task_6_NUnit;
using Serilog;

namespace task_6_NUnit.StepDefinitions
{
    [Binding]
    public class LanguageSwitcherDefinitions
    {
        private IWebDriver _driver;
        private MainPage _mainPage;


        [Given("the user is on the english language version page")]
        public void GivenTheUserIsOnTheEnglishLanguageVersionPage()
        {
            _driver = WebDriverSingleton.GetDriver();

            _mainPage = new MainPageBuilder(_driver).Build();

            Log.Debug("starting AboutTest");
        }

        [When("the user switches the language")]
        public void WhenTheUserSwitchesTheLanguage()
        {
            Log.Debug("starting LanguageSwitcherTest");
            _mainPage.LanguageChange();
        }


        [Then("the user should be redirected to selected language page")]
        public void ThenTheUserShouldBeRedirectedToSelectedLanguagePage()
        {
            Log.Information("asserting current url");
            _driver.Url.ShouldBe("https://lt.ehu.lt/");
        }
    }
}
