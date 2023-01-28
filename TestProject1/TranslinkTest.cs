using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V107.Network;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Microsoft.VisualBasic.FileIO;

namespace TestProject1
{
    public class Tests
    {
        IWebDriver driver;

        string ActualResult;
        string ExpectedResult = "Translink";
        private IWebElement schedulestimefilter;
        private IWebElement starttime;

        [SetUp]
        public void Setup()
        {
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            driver = new ChromeDriver(path + @"\drivers\");
        }

        [Test]
        public void Test1()
        {
            driver.Navigate().GoToUrl("https://www.translink.ca/");
            System.Threading.Thread.Sleep(500);
            driver.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(500);

//click "Schedules and Maps" menu 
            var Menu = driver.FindElement(By.XPath("/html/body/header/div[2]/nav[2]/ul/li[4]/a"));
            Menu.Click();
            System.Threading.Thread.Sleep(500);

//Input 99 in the Bus field

            var Search = driver.FindElement(By.Id("find-schedule-searchbox"));
            driver.FindElement(By.Id("find-schedule-searchbox")).SendKeys("99");

            var Find = driver.FindElement(By.XPath("//*[@id=\"find-schedule\"]/section[2]/div[1]/button"));
            Find.Click();
            System.Threading.Thread.Sleep(1000);

//select UBC B-line

            var UBC = driver.FindElement(By.PartialLinkText("UBC"));

            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", UBC);
            System.Threading.Thread.Sleep(300);

            Console.WriteLine("Click on " + UBC.Text);
            UBC.Click();
            System.Threading.Thread.Sleep(500);
            //select the date

            DateTime tomorrow = DateTime.Now.AddDays(1);
            Console.WriteLine("Today is {0} day {1} {2}", tomorrow.DayOfYear,tomorrow.Month ,tomorrow.Year);

            //Fill start time as 7.00 AM

            var Starttime = driver.FindElement(By.Id("schedulestimefilter-starttime"));
            Starttime.SendKeys("0700AM");
            Starttime.SendKeys(Keys.Tab);
            Console.WriteLine("Wrote Start Time 0700AM");
            System.Threading.Thread.Sleep(200);

            //Fill End time 8.30AM

            var Endtime = driver.FindElement(By.Id("schedulestimefilter-endtime"));
            Endtime.SendKeys("0830AM");
            Endtime.SendKeys(Keys.Tab);
            Console.WriteLine("Wrote End Time 0830AM");
            System.Threading.Thread.Sleep(200);

            //Click Search button

            driver.FindElement(By.XPath("//*[@id=\"schedules_tab\"]/section/div/div[3]/button")).Click();
            
            Console.WriteLine("Clicked Search Button");
            System.Threading.Thread.Sleep(2000);

            //check stops 50913, 50916, 58613
            var stop = driver.FindElement(By.XPath("//*[@id=\"DesktopSchedulesTable\"]/tbody/tr[6]/th[1]/label/input"));
            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", stop);
            System.Threading.Thread.Sleep(200);
            stop.Click();
            Console.WriteLine("Clicked First Stop " + stop.Text);
            stop = driver.FindElement(By.XPath("//*[@id=\"DesktopSchedulesTable\"]/tbody/tr[7]/th[1]/label/input"));
            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", stop);
            System.Threading.Thread.Sleep(200);
            stop.Click();
            Console.WriteLine("Clicked Second Stop " + stop.Text);

            stop = driver.FindElement(By.XPath("//*[@id=\"DesktopSchedulesTable\"]/tbody/tr[8]/th[1]/label/input"));
            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", stop);
            System.Threading.Thread.Sleep(200);
            stop.Click();
            Console.WriteLine("Clicked Third Stop " + stop.Text);
            //click Selected stops only button

            var Busbutton = driver.FindElement(By.XPath("//*[@id=\"content\"]/div[11]/section[3]/div[1]/button[1]"));
            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", Busbutton);
            System.Threading.Thread.Sleep(200);
            Busbutton.Click();
            Console.WriteLine("Clicked Bus Button " + Busbutton.Text);
            System.Threading.Thread.Sleep(200);

//Add to favourites
            var Favour = driver.FindElement(By.XPath("//*[@id=\"information\"]/section[2]/div/button"));
            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", Favour);
            System.Threading.Thread.Sleep(200);
            Favour.Click();
            Console.WriteLine("Clicked Favourite Button");
            System.Threading.Thread.Sleep(2000);

//rename favourites
            var renamefav = driver.FindElement(By.XPath("//*[@id=\"newfavourite\"]"));
            renamefav.Click();
            renamefav.Clear();
            renamefav.SendKeys("99 UBC B-Line - Morning Schedule");

//confirm

            var Confirm = driver.FindElement(By.XPath("//*[@id=\"add-to-favourites_dialog\"]/form/section/div/button"));
            Confirm.Click();
            Console.WriteLine("Added to Favourites"); ;
            System.Threading.Thread.Sleep(200);

//Manage my favourites

            var Manage = driver.FindElement(By.XPath("//*[@id=\"information\"]/section[2]/div/a[3]"));
            ((IJavaScriptExecutor)driver)
            .ExecuteScript("arguments[0].scrollIntoView(true);", Manage);
            System.Threading.Thread.Sleep(500);
            Manage.Click();

            //validation

            String actual_title = driver.FindElement(By.PartialLinkText("99 UBC B-Line - Morning Schedule")).Text;
            String expected_title = "99 UBC B-Line - Morning Schedule";
            Assert.AreEqual(actual_title, expected_title);
            Console.WriteLine("My favourites validated");
            Console.WriteLine("Test passed");

            driver.Close();
            driver.Quit();

            Assert.Pass();
        }


        [OneTimeTearDown] public void TearDown() { }
    }
}