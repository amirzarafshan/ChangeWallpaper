using ChainManager_UI_Testing;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;


namespace AccountTabTest.Models
{
    public class Locators : AccountTabSession
    {

        public static WindowsElement LocateChainManagerElement(string cm_window)
        {
            try
            {
                return session.FindElementByName(cm_window);
            }
            catch (Exception)
            {
                try
                {
                    return session.FindElementByAccessibilityId(cm_window);
                }
                catch (Exception)
                {
                    try
                    {
                        return session.FindElementByXPath($"//Window[starts-with(@Name,\"" + cm_window + "" + "\")]");
                    }
                    catch (Exception)
                    {
                        return session.FindElementByXPath($"//Text[starts-with(@Name,\"" + cm_window + "" + "\")]");
                    }
                }
            }
        }

        public static WindowsElement LocateChainManagerElementContextMenu(string cm_window)
        {
            try
            {
                return sessionDesktop.FindElementByName(cm_window);
            }
            catch (Exception)
            {
                try
                {
                    return sessionDesktop.FindElementByAccessibilityId(cm_window);
                }
                catch (Exception)
                {
                    try
                    {
                        return sessionDesktop.FindElementByXPath($"//Window[starts-with(@Name,\"" + cm_window + "" + "\")]");
                    }
                    catch (Exception)
                    {
                        return sessionDesktop.FindElementByXPath($"//Text[starts-with(@Name,\"" + cm_window + "" + "\")]");
                    }
                }
            }
        }


        public static WindowsElement LocateButton(string name)
        {
            try
            {
                return session.FindElementByXPath($"//Button[@Name=\"" + name + "" + "\"]");
            }
            catch (Exception)
            {
                try
                {
                    return session.FindElementByName(name);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        public static void GotoElemnetContextMenu(WindowsElement element)
        {
            Actions builder = new Actions(session);
            builder.ContextClick(element).Build().Perform();
        }

        public static void WaitElementToShowUp(string element)
        {
            WebDriverWait wdw = new WebDriverWait(session, TimeSpan.FromSeconds(10));
            wdw.Until(pred => LocateChainManagerElement(element).Displayed);
        }

        public static void WaitElementToShowUpContextMenu(string element)
        {
            WebDriverWait wdw = new WebDriverWait(session, TimeSpan.FromSeconds(10));
            wdw.Until(pred => sessionDesktop.FindElementByName(element).Displayed);
        }

        public static void CloseCurrentFormUsingXbutton()
        {
            SwitchToCurrentForm();
            WindowsElement closeXButton = session.FindElementByXPath($"//Button[@Name=\"Close\"]");
            closeXButton.Click();
        }

        public static void SelectAccountFromSearchBox() 
        {
            WindowsElement SearchOptionsCombobox = LocateChainManagerElement(Properties.Search_Options_Filter_box);
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Up + Keys.Up);
            LocateChainManagerElement(Properties.Account_Item_in_Search_Filter_box).Click();
        }

        public static void SelectLocationFromSearchBox()
        {
            WindowsElement SearchOptionsCombobox = LocateChainManagerElement(Properties.Search_Options_Filter_box);
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Up);
            LocateChainManagerElement(Properties.Location_Item_in_Search_Filter_box).Click();
        }

        public static void SelectStationFromSearchBox()
        {
            WindowsElement SearchOptionsCombobox = LocateChainManagerElement(Properties.Search_Options_Filter_box);
            SearchOptionsCombobox.Click();
            //SearchOptionsCombobox.SendKeys(Keys.Up); 
            LocateChainManagerElement(Properties.Station_Item_in_Search_Filter_box).Click();
        }

        public static AppiumOptions CanDesktopDesiredCapabilities()
        {
            AppiumOptions canDesktop = new AppiumOptions();
            canDesktop.AddAdditionalCapability("app", "Root");
            //sessionDesktop = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), canDesktop);
            return canDesktop;
        }

        public static WindowsElement LocateAccountItemInSearchOptions()
        {
            return session.FindElementByName("Account");
        }

        public static WindowsElement LocateFileItemInTopMenu()
        {
            return session.FindElementByXPath($"//Text[@ClassName=\"TextBlock\"][@Name=\"File\"]");
        }

        public WindowsElement AccountSearchView()
        {
            return session.FindElementByAccessibilityId("SearchBox");
        }

        public static void SwitchToCurrentForm()
        {
            try
            {
                var allWindowHandles = session.WindowHandles;
                session.SwitchTo().Window(allWindowHandles[0]);
            }
            catch (Exception) { }
        }
    }
}
