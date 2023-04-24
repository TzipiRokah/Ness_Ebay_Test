using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Threading;
using Ebay_Tests.PageObjects;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;

namespace Ebay_Tests
{
    public class EbayTests
    {

        IWebDriver driver = new ChromeDriver();
        EbayPOM ebayPom;
        Boolean plag=false;

        [SetUp]
        //Entering the Ebay website
        public void Initialize()
        {
            //Enter the website for the first time only
            if (!plag)
            {
                ebayPom = new EbayPOM(driver);
                ebayPom.goToPage();
                plag= true;
            }
        }

        [Test]
        public void test001_search()
        {
            String search = "Watch";
            Assert.AreEqual(ebayPom.search(search), search);
        }

        [Test]
        public void test002_filter()
        {
            Assert.IsTrue(ebayPom.filter());
        }

        [Test]
        public void test003_addToCart()
        {
            Assert.AreEqual(ebayPom.addToCart(),1);
        }
    }
}
