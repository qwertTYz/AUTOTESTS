using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Shouldly;

namespace task_6_NUnit
{
    public class WebDriverSingleton
    {
        private static IWebDriver? _driver;

        private WebDriverSingleton() { }

        public static IWebDriver? Driver { get => _driver; set => _driver = value; }

        public static IWebDriver GetDriver()
        {
            Log.Warning("warning. driver can be null");
            if (Driver == null)
            {
                Driver = new ChromeDriver();
            }

            return Driver;
        }

        public static void QuitDriver()
        {
            Log.Warning("warning. driver can be null");
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
                Driver = null;
            }
        }
    }
}
