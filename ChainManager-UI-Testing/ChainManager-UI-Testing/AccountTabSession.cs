
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;
using AccountTabTest.Models;

namespace ChainManager_UI_Testing
{
    [TestClass]
    public class AccountTabSession
    {

        private const string buildNumber = "22.8.16.1284";
        private const string user = "Amir1";

        private const string mainPath = @"C:\Program Files (x86)\PCM\";
        private const string executable = @"\debug\Chain_Manager.exe";
        private const string cm_path = mainPath + buildNumber + executable;

        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsDriver<WindowsElement> sessionDesktop;

        [TestMethod]
        public static void Setup(TestContext context)
        {
            // Launch Chain Manager application if it is not yet launched
            if (session == null)
            {
                TearDown();

                // Create a new session to bring up Chain Manager application
                AppiumOptions appCapabilities = new AppiumOptions();
                AppiumOptions appCapabilitiesContext = new AppiumOptions();

                appCapabilities.AddAdditionalCapability("app", cm_path);
                appCapabilitiesContext.AddAdditionalCapability("app", "Root");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                sessionDesktop = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilitiesContext);

                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            }

        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }

            if (sessionDesktop != null)
            {
                sessionDesktop.Quit();
                sessionDesktop = null;
            }
        }


        [TestInitialize]
        public virtual void TestInit()
        {
            try
            {
                if (Locators.LocateChainManagerElement(Properties.LoginWindow).Displayed)
                {
                    //if (sessionDesktop != null)
                    //{
                    //    sessionDesktop.Quit();
                    //    sessionDesktop= null;
                    //}

                    //Clear username text box first
                    Locators.LocateChainManagerElement(Properties.UserName).Clear();

                    //Enter username
                    Locators.LocateChainManagerElement(Properties.UserName).SendKeys(user);

                    //Enter password
                    Locators.LocateChainManagerElement(Properties.Password).SendKeys("amir12345");

                    //Login to chain manager
                    Locators.LocateChainManagerElement(Properties.Login_Button).Click();
                    Thread.Sleep(TimeSpan.FromSeconds(3));

                    //Login window closes, it needs to switch to the chain manager main window, as the currently open form
                    Locators.SwitchToCurrentForm();

                    //Verify the main window with the currently logged in user opens
                    WindowsElement chain_managerEntry = session.FindElementByXPath($"//Window[starts-with(@Name,\"RX Music Chain Manager\")]");
                    Assert.IsNotNull(chain_managerEntry);
                    Assert.IsTrue(chain_managerEntry.Text.Contains("DEV"));
                    Assert.IsTrue(chain_managerEntry.Text.Contains(user));

                    //Main window with two buttons are displayed
                    Assert.IsNotNull(Locators.LocateChainManagerElement("Music Search & Playlists"));
                    Assert.IsNotNull(Locators.LocateChainManagerElement("Stations & Schedules"));
                }

            }
            catch (Exception)
            {
                //Console.WriteLine("Login page not launched");
                return;
            }
        }
    }
}
