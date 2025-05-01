using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V133.Page;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Shouldly;

namespace Task_6_NUnit
{
    public class MainPage
    {
        private readonly IWebDriver _driver;

        private readonly By _about = By.LinkText("About");

        private readonly By _search = By.CssSelector("input.form-control[plerdy-tracking-id='30561103501']");
        private readonly By _searchIcon = By.CssSelector("div.header-search[plerdy-tracking-id='52227623201']");


        private readonly By _languageSwitcher = By.ClassName("language-switcher");
        private readonly By _lit = By.CssSelector("a[plerdy-tracking-id='39047282601']");


        private readonly By _email = By.CssSelector("li[plerdy-tracking-id='35448735101']");
        private readonly By _phoneLT = By.CssSelector("li[plerdy-tracking-id='50296369501']");
        private readonly By _phoneBY = By.CssSelector("li[plerdy-tracking-id='39744896801']");
        private readonly By _socialNetworks = By.CssSelector("li[plerdy-tracking-id='64965466401']");

        public IWebElement Email { get; private set; }
        public IWebElement PhoneLT { get; private set; }
        public IWebElement PhoneBY { get; private set; }
        public IWebElement SocialNetworks { get; private set; }
        public MainPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public MainPage NavigateToAbout()
        {
            _driver.FindElement(_about).Click();
            return this;
        }


        //[TestCase("study programs")]
        public MainPage Search(string search)
        {
            Actions actions = new Actions(_driver);
            actions.MoveToElement(_driver.FindElement(_searchIcon)).Perform();

            IWebElement searchWebElement = _driver.FindElement(_search);
            searchWebElement.Click();
            searchWebElement.SendKeys(search);
            searchWebElement.SendKeys(Keys.Enter);
            return this;
        }

        public MainPage LanguageChange()
        {
            IWebElement languageWebElement = _driver.FindElement(_languageSwitcher);
            Actions actions = new Actions(_driver);
            actions.MoveToElement(languageWebElement).Perform();

            IWebElement lit = _driver.FindElement(_lit);
            lit.Click();

            return this;
        }

        public MainPage ContactInfo()
        {
            _driver.Navigate().GoToUrl("https://en.ehu.lt/contact/");
            Email = _driver.FindElement(_email);
            PhoneLT = _driver.FindElement(_phoneLT);
            PhoneBY = _driver.FindElement(_phoneBY);
            SocialNetworks = _driver.FindElement(_socialNetworks);

            return this;
        }
    }
}
