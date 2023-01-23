using System.Net.NetworkInformation;
using AventStack.ExtentReports;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using PlantITTestingNewZealand.PageObjects.BasePage;
using PlantITTestingNewZealand.Utilities;
using System.Collections;
using System.Xml.Linq;

namespace PlantITTestingNewZealand.PageObjects
{
    public class BuyPage : BaseAbstractPage
    {


        private Hashtable selectionOfItems;
        

        public BuyPage(IWebDriver driver) : base(driver)
        {
            selectionOfItems = new Hashtable();
            selectionOfItems.Add("Stuffed Frog", excelReader.readColumnRowCell("Items", "Stuffed Frog", "BuyCount"));
            selectionOfItems.Add("Fluffy Bunny", excelReader.readColumnRowCell("Items", "Fluffy Bunny", "BuyCount")); 
            selectionOfItems.Add("Valentine Bear", excelReader.readColumnRowCell("Items", "Valentine Bear", "BuyCount"));
        }



        

        [FindsBy(How = How.CssSelector, Using = "div.products.ng-scope ul li")]
        private IList<IWebElement> productsElements;

        [FindsBy(How=How.CssSelector, Using = "div.products.ng-scope ul li div h4")]
        private IList<IWebElement> nameOfProducts;

        [FindsBy(How = How.XPath, Using = "//a[@href='#/cart']")]
        private IWebElement cart_link;
        private By cart_link_by = By.XPath("//a[@href='#/cart']");
   

        public void addItemsToCart()
        {

            foreach (DictionaryEntry item in selectionOfItems) {
                int numberOfBuys = Int32.Parse(item.Value.ToString());
                String itemBought = item.Key.ToString();
                String price = "";
                for (int i = 0; i < numberOfBuys; i++) {

                    foreach (IWebElement element in nameOfProducts)
                    {
                        if (element.Text == item.Key.ToString())
                        {
                            By elementBuyButton = By.XPath("//h4[text()='" + element.Text + "']/parent::div/child::p/a");
                            price = driver.FindElement(By.XPath(
                                "//h4[text()='" + element.Text + "']/parent::div/child::p/span")).Text;
                            wait.Until(ExpectedConditions.ElementIsVisible(elementBuyButton));
                            driver.FindElement(elementBuyButton).Click();
                        }

                    }
                }

                //Write Down the Prices

                base.excelReader.writeOnExcel("Items", itemBought, "Price", price);
                ExtentTestManager.GetTest().Log(Status.Pass,
                                "Bought " + itemBought + " "+ numberOfBuys + " times",
                                ScreenshotUtility.CaptureScreenshot(base.driver,
                                "Screeenshot"));
            }
        }

        public void clickCart() {
            base.wait.Until(ExpectedConditions.ElementIsVisible(cart_link_by));
            cart_link.Click();
            ExtentTestManager.GetTest().Log(Status.Pass,
                                "Cart Link Clicked Successfully",
                                ScreenshotUtility.CaptureScreenshot(base.driver,
                                "ScreeenshotCartLink"));
        }
    }
}
