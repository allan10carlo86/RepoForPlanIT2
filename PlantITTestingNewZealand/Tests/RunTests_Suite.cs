namespace PlantITTestingNewZealand.Tests
{
    [TestFixture, Parallelizable(ParallelScope.Fixtures)]

    public class RunTest_Suite : PlantITTestingNewZealand.BaseClass.BaseClass
    {


        [Test]
        public void Test1()
        {
            TestContext.WriteLine("Running Test 1");
            base.homePage.clickOnContact();
            base.contactPage.clickSubmitButton();
            base.contactPage.validateErrors();
            base.contactPage.inputToFields(); // need input;
            base.contactPage.validateIfErrorsAreGone();
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            TestContext.WriteLine("Running Test 1");
            base.homePage.clickOnContact();
            base.contactPage.inputToFields(); // need input;
            base.contactPage.clickSubmitButton();
            base.contactPage.validateSuccessContactMessage();
            Assert.Pass();
        }


        [Test]
        public void Test3()
        {
            TestContext.WriteLine("Running Test 3");
            base.homePage.clickStartShopping();
            base.buyPage.addItemsToCart();
            base.buyPage.clickCart();
            base.checkoutPage.validateNumbersAndSums();
        }
    }


}
