using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using AventStack.ExtentReports;
using System.Configuration;
using PlantITTestingNewZealand.PageObjects;
using NLog;
using System.Text;
using PlantITTestingNewZealand.Utilities;
using NUnit.Framework.Interfaces;
using Logger = NLog.Logger;

namespace PlantITTestingNewZealand.BaseClass
{
    public class BaseClass
    {

        //public IWebDriver driver;
        public ThreadLocal<IWebDriver> driver = new System.Threading.ThreadLocal<IWebDriver>();

        //Pages
        //public LoginPage loginPage;
        public HomePage homePage;
        public ContactPage contactPage;
        public BuyPage buyPage;
        public CheckOutPage checkoutPage;
        //Logger Notepad Text
        public static Logger logger;    

        //Create Report File
        [OneTimeSetUp]
        public void oneTimeSetup()
        {
            logger = LogManager.GetCurrentClassLogger();
            ExtentTestManager.CreateParentTest(GetType().Name);
        }

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setup Method Execution");
            ExtentTestManager.CreateTest(TestContext.CurrentContext.Test.Name);
            string browserName = ConfigurationManager.AppSettings["browser"];
            TestContext.WriteLine("Browser is : " + browserName);
            browserName = "Chrome";
            InitBrowser(browserName);
            if (browserName != "Chrome")
            {
                driver.Value.Manage().Window.Maximize();
            }
            this.driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            this.driver.Value.Url = "http://jupiter.cloud.planittesting.com";

            string stringTitle = driver.Value.Title;
            TestContext.WriteLine("Title : " + stringTitle);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            logger.Info("Instantiate All Web Pages");
            homePage = new HomePage(this.driver.Value);
            contactPage = new ContactPage(this.driver.Value);
            buyPage = new BuyPage(this.driver.Value);
            checkoutPage = new CheckOutPage(this.driver.Value);
        }


        [TearDown]
        public void tearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                    ? ""
                    : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    break;
                default:
                    logstatus = Status.Pass;
                    break;
            }

            ExtentTestManager.GetTest().Log(logstatus, "Test ended with " + logstatus + stacktrace, ScreenshotUtility.CaptureScreenshot(this.driver.Value, "Failure"));
            TestContext.WriteLine("Tear Down");
            NLog.LogManager.Flush();
            driver.Value.Quit();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            ExtentManager.Instance.Flush();

        }

        public void InitBrowser(string browserName)
        {
            switch (browserName)
            {
                case "Firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    FirefoxProfile profile = new FirefoxProfileManager().GetProfile("TestAutomation");
                    firefoxOptions.Profile = profile;
                    firefoxOptions.AddArguments("--start-maximized");
                    firefoxOptions.AcceptInsecureCertificates = true;
                    this.driver.Value = new FirefoxDriver(firefoxOptions);
                    break;
                case "Chrome":
                    ChromeOptions chromeoptions = new ChromeOptions();
                    chromeoptions.AddArguments("--start-maximized");
                    this.driver.Value = new ChromeDriver(chromeoptions);
                    break;
            }
        }
    }
}
