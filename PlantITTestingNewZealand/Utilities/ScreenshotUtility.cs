using System;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace PlantITTestingNewZealand.Utilities
{
    public class ScreenshotUtility
    {
        /// <summary>
        /// This Method captures screenshot
        /// </summary>
        /// <param name="driver"> Webdriver Instance of Selenium</param>
        /// <param name="screenshotName"> Screenshot name of the screenshot </param>
        /// <returns> MediaEntityModelProvider </returns>
        public static MediaEntityModelProvider CaptureScreenshot(IWebDriver driver, String screenshotName)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            var screenshot = ts.GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, screenshotName).Build();

        }

        /// <summary>
        /// Gives a screenshot in Base64 Format
        /// </summary>
        /// <param name="driver"> Instantiation of the webpage </param>
        /// <returns></returns>
        public static string ScreenCaptureAsBase64String(IWebDriver driver)
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();
            return screenshot.AsBase64EncodedString;
        }

        /// <summary>
        /// This is used to Log Failures in the test
        /// </summary>
        /// <param name="driver">WebDriver instance of the page </param>
        /// <param name="testExtent"> ExtentTest instance of the page</param>
        public static void logFailed(IWebDriver driver, ExtentTest testExtent)
        {
            var testResult = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            DateTime time = DateTime.Now;
            String fileName = "Screenshot_" + time.ToString("h_mm_ss") + ".png";
            if (testResult == TestStatus.Failed)
            {
                testExtent.Fail("Test Failed", ScreenshotUtility.CaptureScreenshot(driver, fileName));
                testExtent.Log(Status.Fail, "test failed with log trace " + stackTrace);
            }

        }

        /// <summary>
        /// This will Log a status with Screenshot 
        /// </summary>
        /// <param name="log"> Description of the log</param>
        /// <param name="driver"> Webdriver instance of the page</param>
        /// <param name="testExtent"> ExtentTest instance of the page</param>
        /// <param name="screenshotName"> String Filename of the Screenshot</param>
        public static void log(string log, IWebDriver driver, ExtentTest testExtent, string screenshotName)
        {
            testExtent.Log(Status.Info, log, CaptureScreenshot(driver, screenshotName));
        }


        /// <summary>
        /// This will log a status in the reports without screenshot
        /// </summary>
        /// <param name="log">Description of the log</param>
        /// <param name="testExtent">ExtentTest instance of the page</param>
        public static void log(string log, ExtentTest testExtent)
        {
            testExtent.Log(Status.Info, log);
        }
    }
}

