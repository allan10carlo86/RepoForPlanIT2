using System;
using AventStack.ExtentReports;
using System.Runtime.CompilerServices;

namespace PlantITTestingNewZealand.Utilities
{
    public class ExtentTestManager
    {
        [ThreadStatic]
        private static ExtentTest _parentTest;

        [ThreadStatic]
        private static ExtentTest _childTest;

        /// <summary>
        /// This creates the parent Extent Report Class
        /// </summary>
        /// <param name="testName"> Test Case Name </param>
        /// <param name="description">Test Case Description</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateParentTest(string testName, string description = null)
        {

            _parentTest = ExtentManager.Instance.CreateTest(testName, description);

            return _parentTest;
        }

        /// <summary>
        /// This creates the child Test.
        /// </summary>
        /// <param name="testName">Test Case Name </param>
        /// <param name="description">Test Case Description</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateTest(string testName, string description = null)
        {
            _childTest = _parentTest.CreateNode(testName, description);
            return _childTest;
        }

        /// <summary>
        /// Get Child Test
        /// </summary>
        /// <returns>ExtentTest - Returns Extent Test</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetTest()
        {
            return _childTest;
        }
    }
}

