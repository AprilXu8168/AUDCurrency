using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
namespace ServerTest
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        }

        [Test]
        public void TestTitle()
        {
            driver.Navigate().GoToUrl("http://localhost:4000");
            string actualTitle = driver.Title;
            Console.WriteLine("received title is: {0}", actualTitle);
            Assert.AreEqual("Next.JS App", actualTitle, "The page title does not match the expected title.");
        }

        [Test]
        public void Test1FirstItem()
        {
            driver.Navigate().GoToUrl("http://localhost:4000");
            System.Threading.Thread.Sleep(2000);
            IWebElement table = driver.FindElement(By.Id("CurrencyListTanble"));
            IWebElement firstRow = table.FindElement(By.CssSelector("tbody tr:first-child"));

            // Extract the data from the first cell (Name) and the second cell (Value)
            string actualName = firstRow.FindElement(By.CssSelector("td:nth-child(2)")).Text;
            string acutualValue = firstRow.FindElement(By.CssSelector("td:nth-child(4)")).Text;

            // Assert the values match the expected data
            Console.WriteLine("testing first row of table: {0} --> {1}", actualName, acutualValue);
            Assert.AreEqual("CNY", actualName, "The first item name does not match.");
            Assert.AreEqual("8/15/2024, 12:19:18 PM", acutualValue, "The first item value does not match.");
        }

        [OneTimeTearDown]
        public void TestClose()
        {
            driver.Close();
        }
    }
}