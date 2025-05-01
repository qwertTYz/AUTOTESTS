using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Reqnroll;
using Serilog;
using Task_6_NUnit;
using Shouldly;

namespace task_6_NUnit.StepDefinitions
{
    [Binding]
    public class ContactInfoDefinitions
    {
        private IWebDriver _driver;
        private MainPage _mainPage;


        [Given("the user is on the page")]
        public void GivenTheUserIsOnThePage()
        {
            _driver = WebDriverSingleton.GetDriver();

            _mainPage = new MainPageBuilder(_driver).Build();

            Log.Debug("starting AboutTest");
        }

        [When("the user clicks the contactInfo button")]
        public void WhenTheUserClicksTheContactInfoButton()
        {
            Log.Debug("starting ContactInfoTest");
            _mainPage.ContactInfo();
        }


        [Then("the user should be redirected to contactInfo page")]
        public void ThenTheUserShouldBeRedirectedToContactInfoPage()
        {
            Log.Information("asserting email is displayed");
            _mainPage.Email.Displayed.ShouldBeTrue();

            Log.Information("asserting phoneLT is displayed");
            _mainPage.PhoneLT.Displayed.ShouldBeTrue();

            Log.Information("asserting phoneBY is displayed");
            _mainPage.PhoneBY.Displayed.ShouldBeTrue();

            Log.Information("asserting SocialNetworks is displayed");
            _mainPage.SocialNetworks.Displayed.ShouldBeTrue();
        }
    }
}
