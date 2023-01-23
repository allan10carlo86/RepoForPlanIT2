using System.Net.NetworkInformation;
using AventStack.ExtentReports;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using PlantITTestingNewZealand.PageObjects.BasePage;
using PlantITTestingNewZealand.Utilities;
using NLog;

namespace PlantITTestingNewZealand.PageObjects
{
    public class ContactPage : BaseAbstractPage
    {
        String[] listOfErrorMessages = {
            "Forename is required",
            "Email is required",
            "Message is required"
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driver"></param>
        public ContactPage(IWebDriver driver) : base(driver)
        {
        }

        [FindsBy(How = How.CssSelector, Using = "a.btn-contact.btn.btn-primary")]
        private IWebElement submit_button;
        private By submit_button_by = By.CssSelector("a.btn-contact.btn.btn-primary");

        [FindsBy(How = How.CssSelector, Using = "span.help-inline.ng-scope")]
        private IList<IWebElement> errorMessages;

        [FindsBy(How = How.Id, Using = "forename")]
        private IWebElement foreName_TextField;

        [FindsBy(How = How.Id, Using = "email")]
        private IWebElement email_TextField;

        [FindsBy(How = How.Id, Using = "message")]
        private IWebElement message_TextField;



        [FindsBy(How = How.CssSelector, Using = "div.alert.alert-success")]
        private IWebElement successMessage_Text;
        private By successMessage_Thanks_Text_By = By.XPath("//div[contains(text(),\"\")]/child::strong[contains(text(),'Thanks')]");

        private By sendingFeedbackText_By = By.XPath("//*[contains(text(), 'Sending Feedback')");
        /// <summary>
        /// 
        /// </summary>
        public void clickSubmitButton()
        {
            TestContext.WriteLine("Run clickSubmitButton");
            base.wait.Until(ExpectedConditions.ElementIsVisible(submit_button_by));
            submit_button.Click();
            ExtentTestManager.GetTest().Log(Status.Pass, "Click On Submit Button");
        }
        /// <summary>
        /// 
        /// </summary>
        public void validateErrors() {
            TestContext.WriteLine("Run validateErrors");
            Status status = Status.Pass;
            foreach (IWebElement errorElement in errorMessages) {
                if(checkErrorMessagesFromAList(errorElement.Text))
                {
                    ExtentTestManager.GetTest().Log(status, errorElement.Text + " is existing as an Error Message");
                    continue;
                } else {
                    status = Status.Fail;
                    ExtentTestManager.GetTest().Log(status, errorElement.Text + " is not Part of the Error Message");
                    Assert.Fail();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <returns></returns>
        public Boolean checkErrorMessagesFromAList(String stringToCheck) {
            TestContext.WriteLine("Run checkErrorMessagesFromAList");

            for (int i = 0; i < listOfErrorMessages.Length; i++) {
                if (listOfErrorMessages[i].Equals(stringToCheck)) {
                    return true;
                }
            }
            return false;
        }

        public void inputToFields() {
            TestContext.WriteLine("Run inputToFields");
            foreName_TextField.SendKeys("John");
            email_TextField.SendKeys("email@email.com");
            message_TextField.SendKeys("Hello to you all");
            ExtentTestManager.GetTest().Log(Status.Pass, "Sent Keys Passed");

        }

        public void validateIfErrorsAreGone()
        {
            TestContext.WriteLine("Run validateIfErrorsAreGone");
            Status status = Status.Pass;
            foreach (IWebElement errorMessage in errorMessages) {
                Thread.Sleep(5000);
                try {
                    String sampleText = errorMessage.Text;
                    if (sampleText != null || !sampleText.Equals("")) {
                        ExtentTestManager.GetTest().Log(Status.Fail, sampleText + " error message is existing");
                        status = Status.Fail;
                        Assert.Fail();
                    }
                } catch (NullReferenceException e) { 
                    TestContext.WriteLine("Pass No Error Here");
                    ExtentTestManager.GetTest().Log(Status.Pass, "No Such Error Message");
                    logger.Info("Pass No Error Here");
                    continue;
                }
            }
        }

        public void validateSuccessContactMessage() {
            base.wait.Until(ExpectedConditions.InvisibilityOfElementLocated(sendingFeedbackText_By));
            base.wait.Until(ExpectedConditions.ElementIsVisible(successMessage_Thanks_Text_By));
            String text = successMessage_Text.Text;
            TestContext.WriteLine(text);
            logger.Info(text);
            ExtentTestManager.GetTest().Log(Status.Pass, "Success Message is thrown", ScreenshotUtility.CaptureScreenshot(this.driver, "Screenshot Successs"));
        }
    }
}
