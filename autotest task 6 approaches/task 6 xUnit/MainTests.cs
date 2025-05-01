using System;
using System.IO;
using OpenQA.Selenium;
using Serilog;
using Shouldly;
using Xunit;

namespace task_6_xUnit
{
    public class MainTests
    {
        private IWebDriver _driver;
        private MainPage _mainPage;

        static MainTests()
        {
            var runId = Guid.NewGuid();
            var ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var logPath = Path.Combine("TestLogs", $"TestLog_{ts}_{runId}.txt");

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
                    retainedFileCountLimit: 7)
                .CreateLogger();

            Log.Information("Logging initialized for xUnit tests");
        }
        private void Init()
        {
            _driver = WebDriverSingleton.GetDriver();
            _mainPage = new MainPageBuilder(_driver).Build();
        }

        [Fact]
        public void AboutTest()
        {
            Log.Information("starting AboutTest");
            Init();

            try
            {
                Log.Debug("Clicking About link");
                _mainPage.NavigateToAbout();

                Log.Information("Asserting URL is About page");
                _driver.Url.ShouldBe("https://en.ehu.lt/about/");

                Log.Information("AboutTest PASSED");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "AboutTest FAILED");
                throw;
            }
            finally
            {
                WebDriverSingleton.QuitDriver();
            }
        }

        [Theory]
        [InlineData("study programs")]
        public void SearchTest(string query)
        {
            Log.Information("starting SearchTest");
            Init();

            try
            {
                Log.Debug("Performing search for: {Query}", query);
                _mainPage.Search(query);

                Log.Information("Asserting URL is search results");
                _driver.Url.ShouldBe("https://en.ehu.lt/?s=study+programs");

                Log.Information("SearchTest PASSED");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SearchTest FAILED");
                throw;
            }
            finally
            {
                WebDriverSingleton.QuitDriver();
            }
        }

        [Fact]
        public void LanguageChangeTest()
        {
            Log.Information("starting LanguageChangeTest");
            Init();

            try
            {
                Log.Debug("changing language");
                _mainPage.LanguageChange();

                Log.Information("asserting url is lithuanian page");
                _driver.Url.ShouldBe("https://lt.ehu.lt/");

                Log.Information("LanguageChangeTest PASSED");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LanguageChangeTest FAILED");
                throw;
            }
            finally
            {
                WebDriverSingleton.QuitDriver();
            }
        }

        [Fact]
        public void ContactInfoTest()
        {
            Log.Information("starting ContactInfoTest");
            Init();

            try
            {
                Log.Debug("Navigating to Contact page and grabbing elements");
                _mainPage.ContactInfo();

                Log.Information("asserting email is displayed");
                _mainPage.Email.Displayed.ShouldBeTrue();

                Log.Information("asserting phoneLT is displayed");
                _mainPage.PhoneLT.Displayed.ShouldBeTrue();

                Log.Information("asserting phoneBY is displayed");
                _mainPage.PhoneBY.Displayed.ShouldBeTrue();

                Log.Information("asserting SocialNetworks is displayed");
                _mainPage.SocialNetworks.Displayed.ShouldBeTrue();

                Log.Information("ContactInfoTest PASSED");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ContactInfoTest FAILED");
                throw;
            }
            finally
            {
                WebDriverSingleton.QuitDriver();
            }
        }
    }
}
