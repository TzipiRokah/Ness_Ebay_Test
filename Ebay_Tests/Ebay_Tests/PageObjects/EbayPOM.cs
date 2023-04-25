using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using SeleniumExtras.PageObjects;
using System.Linq;
using System.Collections.Generic;

namespace Ebay_Tests.PageObjects
{
    internal class EbayPOM
    {
        private IWebDriver driver;
        public EbayPOM(IWebDriver driver)
        {
            this.driver = driver;
            //Init all elemnt
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "#gh-ac")]
        private IWebElement searchBook;
        [FindsBy(How = How.CssSelector, Using = "#gh-btn")]
        private IWebElement seaerchBookButton;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-controls__control srp-controls__count\"] h1 span:last-child")]
        private IWebElement resultSearch;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-controls__control srp-controls__count\"] h1 span:last-child")]
        private IList<IWebElement> resultsSearch;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-results srp-grid clearfix\"] [class=\"s-item__title\"]")]
        private IWebElement resultSearchTwo;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-results srp-grid clearfix\"] [class=\"s-item__title\"]")]
        private IList<IWebElement> resultsSearchTwo;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-refine__category__list\"] [class=\"srp-refine__category__item\"] span")]
        private IWebElement chooseFilter;
        [FindsBy(How = How.CssSelector, Using = "ul [class=\"s-item s-item__pl-on-bottom\"]")]
        private IWebElement chooseItem;
        [FindsBy(How = How.CssSelector, Using = "[class=\"x-buybox-cta\"] li:nth-child(2)")]
        private IWebElement addToCartButton;
        [FindsBy(How = How.CssSelector, Using = "[class=\"x-buybox-cta\"] li:nth-child(2)")]
        private IWebElement addToCartButtonTwo;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-controls__control srp-controls__count\"] h1")]
        private IWebElement checkSearch;
        [FindsBy(How = How.CssSelector, Using = "[class=\"srp-refine__category__item\"] span")]
        private IWebElement checkFilter;
        [FindsBy(How = How.CssSelector, Using = "[class=\"listsummary-content-itemdetails\"] h3")]
        private IWebElement ItemInCart;
        [FindsBy(How = How.CssSelector, Using = "#x-msku__select-box-1000")]
        private IWebElement selectButton;
        [FindsBy(How = How.CssSelector, Using = "#x-msku__option-box-0")]
        private IWebElement chooseSize;
        [FindsBy(How = How.CssSelector, Using = "#gh-cart-n")]
        private IWebElement addToCartIcon;

        //A function that receives an element and waits for it to load
        public void WaitForElement(IWebElement element, int timeout = 1)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMinutes(timeout));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            wait.Until<bool>(driver =>
            {
                try
                {
                    return element.Displayed;
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }

        //Login to the website
        public void goToPage()
        {
            driver.Navigate().GoToUrl("http://www.ebay.com");
        }

        //Checking search option
        public String search(String wordSearch)
        {
            searchBook.SendKeys(wordSearch);
            seaerchBookButton.Click();
            //Search by the first possible cssSelector
            try
            {
                WaitForElement(resultSearch);
            }catch(NoSuchElementException e) {
                //Changing CssSelector after first not found
                resultSearch = resultSearchTwo;
            }

            //Over all the options found and checking that they correspond to the requested search
            foreach (var item in resultsSearch)
            {
                if (!item.Text.Contains(wordSearch))
                    return "non";
            }
            return resultSearch.Text;
        }
        //Checking filter option
        public Boolean filter()
        {
            WaitForElement(chooseFilter);
            //The name of the selected filter
            String filterName = chooseFilter.Text;
            chooseFilter.Click();
            WaitForElement(checkFilter);
            //The category that was activated
            String resultFilter = checkFilter.Text;
            //Checking that the category that was activated is the same as the selected filter
            if (resultFilter.Contains(filterName))
                return true;
            return false;
        }

        //checking add to cart option
        public int addToCart()
        {
            chooseItem.Click();
            //The name of the selected item
            String _chooseItemName = chooseItem.Text.Substring(0, 30);
            //Update driver to new tub
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            try
            {
                //Search by the first possible cssSelector
                WaitForElement(addToCartButton);
            }
            catch (NoSuchElementException e)
            {
                //Changing CssSelector after first not found
                addToCartButton = addToCartButtonTwo;
            }
              try
            {
                addToCartButton.Click();
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine(e.Message);
            }
            WaitForElement(addToCartIcon);
            //The name of the item that went into the cart
            String _IteminCartName = ItemInCart.Text.Substring(0, 30);
            //Checking that the cart has been updated to one item and that the selected item is the same as the item in the cart
            if (addToCartIcon.Text.Equals("1") && _IteminCartName.Equals(_chooseItemName))
                return int.Parse(addToCartIcon.Text);
            return 0;
        }
    }
}
