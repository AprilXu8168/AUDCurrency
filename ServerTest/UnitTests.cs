using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
namespace ServerTest
{
    public class unitTests
    {
        private IWebDriver driver;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);        
        }

        [Test]
        public void TestSwagger()
        {
            driver.Navigate().GoToUrl("http://localhost:5151");
            string actualTitle = driver.Title;
            Console.WriteLine("received title is: {0}", actualTitle);
            Assert.That(actualTitle, Is.EqualTo("Swagger UI"), "The page title does not match the expected title.");
        }

        [Test]
        public void TestGraphql()
        {
            driver.Navigate().GoToUrl("http://localhost:5151/graphql");
            string actualTitle = driver.Title;
            Console.WriteLine("received title is: {0}", actualTitle);
            Assert.That(actualTitle, Is.EqualTo("Banana Cake Pop"), "The page title does not match the expected title.");
        }
        
        [Test]
        public void TestValueReview()
        {
            driver.Navigate().GoToUrl("http://localhost:5151/api/ExchangeRatesService/2");
            var preElement = driver.FindElement(By.TagName("pre"));
            var jsonResponse = preElement.Text;
            var jsonObject = JObject.Parse(jsonResponse);
            Console.WriteLine("received object is: {0}", jsonObject);

            var actualName =  jsonObject["name"];
            Assert.That(actualName.Value<string>(), Is.EqualTo("JPY"));

            var acualValue = jsonObject["value"];
            Assert.That(acualValue.Value<string>(), Is.EqualTo("430"));

            var actualID = jsonObject["id"];
            Assert.That(actualID.Value<string>(), Is.EqualTo("2"));
        
        }

        [OneTimeTearDown]
        public void TestClose()
        {
            driver.Close();
        }
    }
}