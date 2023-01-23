using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using System.Globalization;
using System.Net.NetworkInformation;

namespace TestAutomation.Utilities.DateUtility
{
    public class DateUtility
    {
        static DateTime now = DateTime.Now;

        /// <summary>
        /// Gets the Date Today in Format MM/DD/YYYY
        /// </summary>
        /// <returns>Returns Today's Date in Format MM/DD/YYYY</returns>
        public static string getDateToday()
        {
            string date = now.ToString("MM/dd/yyyy");
            TestContext.WriteLine(date);
            return date;
        }

        /// <summary>
        /// Get the Date Time Today in Default Format
        /// </summary>
        /// <returns>Returns Date Time Today in Default Format</returns>
        public static string getDateTimeToday()
        {
            string date = now.ToString();
            TestContext.WriteLine(date);
            return date;
        }

        /// <summary>
        /// Returns Date Time Today in a Specific Format
        /// </summary>
        /// <param name="format">the format you want to do or this format "MMM-dd-yyyy-HH-mm-ss-tt"</param>
        /// <returns>Date in a specific format or this format - "MMM-dd-yyyy-HH-mm-ss-tt" </returns>
        public static string getDateTimeToday_WithFormat(string format= "MMM-dd-yyyy-HH-mm-ss-tt")
        {
            string date = now.ToString(format);
            TestContext.WriteLine(date);
            return date;
        }


    }
}
