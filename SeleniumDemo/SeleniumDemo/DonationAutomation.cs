using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using NUnit.Framework;
using System;
using Assert = NUnit.Framework.Assert;

namespace DonationTests {
    [TestFixture]
    public class DonationAutomation {
        IWebDriver driver;

        // Set up ChromeDriver automatically
        [SetUp]
        public void StartBrowser() {
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            // we should Only use this when we're trying to maximuze window
            //driver.Manage().Window.Maximize();

            // Navigate to our local XAMPP link that we'd set up
            // We'd installed xammp and started apache
            driver.Navigate().GoToUrl("http://localhost/Donation/Index.html");
        }

        [Test]
        public void Test_Donation_20_Returns_0Percent() {
            FillForm("John", "Doe", "john1@test.com", "20");

            // The HTML tag is found in <p id="deduction">
            var resultElement = driver.FindElement(By.Id("deduction"));
            Assert.That(resultElement.Text, Is.EqualTo("Your donation will get a 0% tax deduction."));
        }

        // small rage to test after the 0% deduction: $25 to $1000 -> 5%
        [Test]
        public void Test_Donation_25_Returns_5Percent() {
            FillForm("Jane", "Doe", "jane2@test.com", "25");

            // the asserting messages will have to be the same as the messages in the script
            var resultElement = driver.FindElement(By.Id("deduction"));
            Assert.That(resultElement.Text, Is.EqualTo("Your donation will get a 5% tax deduction."));
        }

        // Mid range value to test
        [Test]
        public void Test_Donation_500_Returns_5Percent() {
            FillForm("Bob", "Doe", "Bob3@test.com", "500");

            var resultElement = driver.FindElement(By.Id("deduction"));
            Assert.That(resultElement.Text, Is.EqualTo("Your donation will get a 5% tax deduction."));
        }
        
        // High range value to test
        [Test]
        public void Test_Donation_1000_Returns_5Percent() {
            FillForm("Jake", "Doe", "Jake4@test.com", "1000");

            var resultElement = driver.FindElement(By.Id("deduction"));
            Assert.That(resultElement.Text, Is.EqualTo("Your donation will get a 5% tax deduction."));
        }
        // 15% deduction test
        [Test]
        public void Test_Donation_1001_Returns_15Percent() {
            FillForm("Banner", "Doe", "Banner5@test.com", "1001");

            var resultElement = driver.FindElement(By.Id("deduction"));
            Assert.That(resultElement.Text, Is.EqualTo("Your donation will get a 15% tax deduction."));
        }

        // Helper method for us to use the exact IDs from Donation/Index.html
        private void FillForm(string first, string last, string email, string amount) {
            driver.FindElement(By.Id("firstName")).SendKeys(first);
            driver.FindElement(By.Id("lastName")).SendKeys(last);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("donationAmount")).SendKeys(amount);

            // The button doesn't have an ID, so we find it by type
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
        }

        [TearDown]
        public void CloseBrowser() {
            if (driver != null) {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}