using System;
using System.Xml.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using Serilog;
using Shouldly;
using Task_6_NUnit;

namespace Task_6_NUnit
{
    [TestFixture]
    [Parallelizable]
    public class MainTests
    {
        private IWebDriver _driver;
        private MainPage _mainPage;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            var runId = Guid.NewGuid();
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var logPath = Path.Combine("TestLogs", $"TestLog_{timestamp}_{runId}.txt");

            Directory.CreateDirectory("TestLogs");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithProperty("RunId", runId)
                .WriteTo.Console()
                .WriteTo.File(
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 10_000_000,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 7
                )
                .CreateLogger();

            Log.Information("Logging initialized for NUnit tests");
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            Log.CloseAndFlush();
        }

        [SetUp]
        public void SetUp()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var startTime = DateTime.Now;
            var os = Environment.OSVersion;
            var browser = "Chrome";

            Log.Information("=== Starting Test: {TestName} at {StartTime} | OS: {OS} | Browser: {Browser} ===",
                testName, startTime, os, browser);


            _driver = WebDriverSingleton.GetDriver();

            _mainPage = new MainPageBuilder(_driver).Build();
        }

        [Test]
        [Category("About")]
        public void AboutTest()
        {
            Log.Debug("starting AboutTest");
            _mainPage.NavigateToAbout();

            Log.Information("asserting current url");
            _driver.Url.ShouldBe("https://en.ehu.lt/about/");
        }


        [Test]
        [Category("Search")]
        public void SearchTest()
        {
            Log.Debug("starting SearchTest");
            Log.Information("entering 'study programs'");
            _mainPage.Search("study programs");

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

        [Test]
        public void LanguageSwitcherTest()
        {
            Log.Debug("starting LanguageSwitcherTest");
            _mainPage.LanguageChange();

            Log.Information("asserting current url");
            _driver.Url.ShouldBe("https://lt.ehu.lt/");
        }

        [Test]
        public void ContactInfoTest()
        {
            Log.Debug("starting ContactInfoTest");
            _mainPage.ContactInfo();

            Log.Information("asserting email is displayed");
            _mainPage.Email.Displayed.ShouldBeTrue();

            Log.Information("asserting phoneLT is displayed");
            _mainPage.PhoneLT.Displayed.ShouldBeTrue();

            Log.Information("asserting phoneBY is displayed");
            _mainPage.PhoneBY.Displayed.ShouldBeTrue();

            Log.Information("asserting SocialNetworks is displayed");
            _mainPage.SocialNetworks.Displayed.ShouldBeTrue();
        }

        [TearDown]
        public void TearDown()
        {
            var testName = TestContext.CurrentContext.Test.Name;
            var result = TestContext.CurrentContext.Result.Outcome.Status;
            Log.Information("=== Finished Test: {TestName} | Result: {Result} ===", testName, result);


            WebDriverSingleton.QuitDriver();
            _driver.Dispose();
        }
    }
}