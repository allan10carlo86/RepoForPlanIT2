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

namespace PlantITTestingNewZealand.PageObjects
{
    public class CheckOutPage : BaseAbstractPage
    {

        private Dictionary<String, List<String>> dictionaryItem;
        private IList<IWebElement> itemPrice_text;

        public CheckOutPage(IWebDriver driver) : base(driver)
        {
            dictionaryItem = new Dictionary<String, List<String>>();
        }

        [FindsBy(How = How.XPath, Using = "//tbody/tr")]
        private IList<IWebElement> items;

        [FindsBy(How = How.XPath, Using = "//td//strong")]
        private IWebElement total_from_webpage_element_text;


        public void validateNumbersAndSums()
        {
            List<String> listOfProducts = new List<String>();

            int countOfInput = 1;
            int countOfProducts = items.Count;
            int counterForInputProducts = 1;
                     
            foreach (IWebElement element in items) {


                IList<IWebElement> childNodes = element.FindElements(By.XPath("./child::td"));
                int childNodesOfTrs = childNodes.Count;

                int countTD = 1;

                List<String> storageOfDataForAnItem = new List<String>();
                foreach (IWebElement childNode in childNodes)
                {

                    if (countTD > 4)
                    {
                        break;
                    }

                    if (countTD != 3 & countTD <= 4)
                    {
                        if (countTD == 1)
                        {
                            listOfProducts.Add(childNode.Text.Trim());
                        }
                        storageOfDataForAnItem.Add(childNode.Text.Trim());
                    }
                    else if (countTD == 3) /* There is something wrong with this implementation */
                    {
                        IWebElement input = driver.FindElement(By.XPath("(//tbody//tr//input)["+ (dictionaryItem.Count + 1).ToString() + "]"));
                        int quantity;
                        quantity = Int32.Parse(input.GetAttribute("value").Trim());
                        ExtentTestManager.GetTest().Log(Status.Info, quantity.ToString());
                        storageOfDataForAnItem.Add(quantity.ToString());
                    }

                    countTD = countTD + 1;
                }
                dictionaryItem.Add(storageOfDataForAnItem[0], storageOfDataForAnItem);
                counterForInputProducts += counterForInputProducts;

                if (dictionaryItem.Count >= countOfProducts)
                {
                    break;
                }
            }

            double sum_from_computation = 0;
            foreach (KeyValuePair<String, List<String>> entry in dictionaryItem) {
                double priceFromAddToCart = Double.Parse(excelReader.readColumnRowCell("Items", entry.Key.ToString(), "Price").Trim().Substring(1));
                String[] valuesOfItem = entry.Value.ToArray();
                int quantity_from_checkout = Int32.Parse(valuesOfItem[2].Trim()); ;
                double price_from_checkout = Double.Parse(valuesOfItem[1].Trim().Substring(1));
                double subtotal_from_checkout = Double.Parse(valuesOfItem[3].Trim().Substring(1));


                ExtentTestManager.GetTest().Log(Status.Info, entry.Key.ToString());

                if (priceFromAddToCart != price_from_checkout)
                {
                    ExtentTestManager.GetTest().Log(Status.Fail, "priceFromAddToCart = " + priceFromAddToCart
                        + " is not equal to price_from_checkout = " + price_from_checkout);
                    Assert.Fail();
                }

                double subtotal_computed = quantity_from_checkout * price_from_checkout;
                if (subtotal_from_checkout != subtotal_computed)
                {
                    ExtentTestManager.GetTest().Log(Status.Fail, "subtotal_from_checkout = " + subtotal_from_checkout
                        + " is not equal to sub_total_computed = " + subtotal_computed);
                    Assert.Fail();
                }


                sum_from_computation += subtotal_from_checkout;
            }

            String[] split_data_total = total_from_webpage_element_text.Text.Split(": ");


            double double_total_from_webpage_element_text = Double.Parse(split_data_total[1].Trim());

            if (sum_from_computation != double_total_from_webpage_element_text) {
                ExtentTestManager.GetTest().Log(Status.Fail, "sum_from_computation = " + sum_from_computation
                         + " is not equal to double_total_from_webpage_element_text = " + double_total_from_webpage_element_text);
                Assert.Fail();
            }
         }
    }
}
