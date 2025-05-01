using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V133.DOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace task_6_xUnit
{
    public class WebDriverSingleton
    {
        public static IWebDriver? _driver;

        public WebDriverSingleton() { }

        public static IWebDriver? Driver { get => _driver; set => _driver = value; }

        public static IWebDriver GetDriver()
        {
            if (Driver == null)
            {
                Driver = new ChromeDriver();
            }

            return Driver;
        }

        public static void QuitDriver()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
                Driver = null;
            }
        }
    }
}
