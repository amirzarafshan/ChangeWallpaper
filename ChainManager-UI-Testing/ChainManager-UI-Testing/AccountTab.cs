using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AccountTabTest.Models;
using ChainManager_UI_Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace AccountTabTest
{
    [TestClass]
    public class AccountTest : AccountTabSession
    {

        [TestMethod]
        public void TC01_SearchOptionsFilterBoxTest()
        {
            // Open search Options combo/filter box and move to the first item in the box
            WindowsElement SearchOptionsCombobox = session.FindElementByAccessibilityId("SearchOptions");
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Up + Keys.Up);

            // Verify first item is 'Account' in the filter box
            WindowsElement accountItem = session.FindElementByName("Account");
            Assert.IsNotNull(accountItem);
            Assert.AreEqual(accountItem.Text, "Account");

            // Verify first item 'Account' in the filter box is selectable
            accountItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var accountItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(accountItemSelected.Contains("Account"));
            Assert.AreEqual(accountItemSelected, "{\"name\":\"Account\",\"id\":1}");


            // Verify second item is 'Location' in the filter box
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Down);
            WindowsElement locationItem = session.FindElementByName("Location");
            Assert.IsNotNull(locationItem);
            Assert.AreEqual(locationItem.Text, "Location");

            // Verify second item 'Location' in the filter box is selectable
            locationItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var locationItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(locationItemSelected.Contains("Location"));
            Assert.AreEqual(locationItemSelected, "{\"name\":\"Location\",\"id\":2}");


            // Verify third item is 'Station' in the filter box
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Down);
            WindowsElement stationItem = session.FindElementByName("Station");
            Assert.IsNotNull(stationItem);
            Assert.AreEqual(stationItem.Text, "Station");

            // Verify third item 'Location' in the filter box is selectable
            stationItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var stationItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(stationItemSelected.Contains("Station"));
            Assert.AreEqual(stationItemSelected, "{\"name\":\"Station\",\"id\":3}");

            // Verify fourth item is 'CRM' in the filter box
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Down);
            WindowsElement crmItem = session.FindElementByName("CRM");
            Assert.IsNotNull(crmItem);
            Assert.AreEqual(crmItem.Text, "CRM");

            // Verify fourth item 'CRM' in the filter box is selectable
            crmItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var crmItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(crmItemSelected.Contains("CRM"));
            Assert.AreEqual(crmItemSelected, "{\"name\":\"CRM\",\"id\":4}");

            // Re-select item 'station' in filterbox as the defult item. //
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Up);
            stationItem.Click();
            Assert.IsNotNull(stationItem);
            Assert.IsTrue(stationItemSelected.Contains("Station"));

            SearchOptionsCombobox = null;
        }

        [TestMethod]
        public void TC00_FileMenuTest()
        {

            WindowsElement file = session.FindElementByName("File");
            file.Click();

            WindowsElement menu_new = session.FindElementByName("New...");
            menu_new.Click();

            WindowsElement accountMenuItem = session.FindElementByAccessibilityId("AccountMenuItem");
            Assert.IsTrue(accountMenuItem.Enabled);

            WindowsElement stationMenuItem = session.FindElementByAccessibilityId("StationMenuItem");
            Assert.IsFalse(stationMenuItem.Enabled);

            WindowsElement chainMenuItem = session.FindElementByAccessibilityId("ChainMenuItem");
            Assert.IsFalse(chainMenuItem.Enabled);

            WindowsElement subchainMenuItem = session.FindElementByAccessibilityId("SubChainMenuItem");
            Assert.IsFalse(subchainMenuItem.Enabled);

            WindowsElement userMenuItem = session.FindElementByAccessibilityId("UserMenuItem");
            Assert.IsTrue(userMenuItem.Enabled);

            file.Click();

        }

        [TestMethod]
        public void TC02_CreateAccountCancelButtonTest()
        {

            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_Item).Click();

            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_New_Item).Click();

            // Create new account
            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_New_Account_Item).Click();

            Locators.SwitchToCurrentForm();
            WindowsElement newAccountForm = Locators.LocateChainManagerElement(Properties.New_Account_Form);

            //Test 'Cancel' button
            Locators.LocateChainManagerElement(Properties.Cancel_Button).Click() ;

            var allWindowHandles = session.WindowHandles;
            Assert.IsTrue(allWindowHandles.Count == 2);
            Assert.IsFalse(newAccountForm.Displayed);
            Locators.SwitchToCurrentForm();

        }

        [TestMethod]
        public void TC04_CreateAccountSaveButtonTest()
        {
            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_Item).Click();

            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_New_Item).Click();

            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_New_Account_Item).Click();

            // create new account
            Locators.SwitchToCurrentForm();
            WindowsElement newAccountForm = Locators.LocateChainManagerElement(Properties.New_Account_Form);

            // Test 'Save' button
            Locators.LocateChainManagerElement(Properties.Save_Button).Click();

            // Verify 'Name cannot be blank' warning message appears on acreen
            var allWindowHandles = session.WindowHandles;
            session.SwitchTo().Window(allWindowHandles[0]);
            WindowsElement warningMessage = session.FindElementByXPath($"//Text[starts-with(@Name,\"Name cannot be blank\")]");
            Assert.IsNotNull(warningMessage);
            Assert.IsTrue(warningMessage.Displayed);

            // Press OK button on warning message
            WindowsElement confirmMessage = session.FindElementByName("OK");
            confirmMessage.Click();

            // Switch back to the 'new account' window after confirming warning message
            //session.SwitchTo().Window(allWindowHandles[1]);

            // Press Cancel on new account window
            WindowsElement cancelButton = session.FindElementByName("Cancel");
            cancelButton.Click();

            Locators.SwitchToCurrentForm();
        }


        [TestMethod]
        public void TC05_CreateAccountTest()
        {
            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_Item).Click();

            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_New_Item).Click();

            Locators.LocateChainManagerElement(Properties.StationsAndSchedules_File_New_Account_Item).Click();

            // Create new account
            Locators.SwitchToCurrentForm();
            WindowsElement newAccountForm = Locators.LocateChainManagerElement(Properties.New_Account_Form);

            // Enter account name
            WindowsElement accountName = Locators.LocateChainManagerElement(Properties.New_Account_Form);
            accountName.SendKeys(Properties.Account_Name_Value);

            // Enter email 
            accountName.SendKeys(Keys.Tab + Properties.Account_Email_Value);

            // Press save 
            Locators.LocateChainManagerElement(Properties.Save_Button).Click();

            var allWindowHandles = session.WindowHandles;

            // Verify account created:
            // 1.no internal error generated
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.AreEqual(allWindowHandles.Count, 2);


            // 1.1 First close the station and schedule window and relaunch it 
            Locators.SwitchToCurrentForm();
            Locators.LocateButton("Close").Click();
            Locators.SwitchToCurrentForm();
            TestInit();

            // 2.check whether the account is created in search result
            // 2.1 Open search Options combo/filter box and move to the 'Account' item in the box
            Locators.SwitchToCurrentForm();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            Locators.SelectAccountFromSearchBox();

            // 2.2 Check auto filter checkbox
            //session.FindElementByClassName(Properties.Filter_CheckBox).Click();

            // 2.3 Enter account name in searchbox
            WindowsElement SearchBox = Locators.LocateChainManagerElement(Properties.Search_Box);
            SearchBox.Click();
            SearchBox.SendKeys(Properties.Account_Name_Value);

            // 2.4 Press view button
            Locators.LocateButton(Properties.View_Button).Click();

            //Thread.Sleep(TimeSpan.FromSeconds(3));
            Locators.WaitElementToShowUp(Properties.Account_Name_Value);
            Assert.AreEqual(Locators.LocateChainManagerElement(Properties.Account_Name_Value).Text, SearchBox.Text);

            Locators.SwitchToCurrentForm();
        }

        [TestMethod]
        public void TC06_DeleteAccountTest()
        {
            //if (!IsAccountExist())
            if (!IsCMElementExist(Properties.Account_Item_in_Search_Filter_box, Properties.Account_Name_Value))
            {
                TC05_CreateAccountTest();
            }

            Locators.GotoElemnetContextMenu(Locators.LocateChainManagerElement(Properties.Account_Name_Value));
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Locators.WaitElementToShowUpContextMenu(Properties.Delete_Context_Menu);
            Locators.LocateChainManagerElementContextMenu(Properties.Delete_Context_Menu).Click();

            Locators.SwitchToCurrentForm();
            Locators.LocateChainManagerElement(Properties.Yes_Button).Click();

            Locators.LocateButton(Properties.View_Button).Click();
            Assert.IsFalse(IsCMElementExist(Properties.Account_Item_in_Search_Filter_box, Properties.Account_Name_Value));
        }

        [TestMethod]
        public void TC07_CreateLocationTest()
        {
            if (!IsCMElementExist(Properties.Account_Item_in_Search_Filter_box, Properties.Account_Name_Value))
            {
                TC05_CreateAccountTest();
            }

            Locators.GotoElemnetContextMenu(Locators.LocateChainManagerElement(Properties.Account_Name_Value));
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Locators.WaitElementToShowUpContextMenu(Properties.New_Location_Form);
            Locators.LocateChainManagerElementContextMenu(Properties.New_Location_Form).Click();

            Locators.SwitchToCurrentForm();
            Assert.IsTrue(Locators.LocateChainManagerElement("Monitor").Enabled);

            WindowsElement location_name = Locators.LocateChainManagerElement("Name");
            location_name.SendKeys(Properties.Location_Name_Value);
            location_name.SendKeys(Keys.Tab + Properties.Location_Address_Value);
            location_name.SendKeys(Keys.Tab + Keys.Tab  + Properties.Location_Contact_Name_Value);
            location_name.SendKeys(Keys.Tab + Keys.Tab  + Keys.Tab + Properties.Location_Phone_Value);
            location_name.SendKeys(Keys.Tab + Keys.Tab  + Keys.Tab + Keys.Tab + Properties.Location_Email_Value);
            Locators.LocateButton(Properties.Save_Button).Click();

            Locators.SwitchToCurrentForm();
            
            Locators.SelectLocationFromSearchBox();

            WindowsElement SearchBox = Locators.LocateChainManagerElement(Properties.Search_Box);
            SearchBox.Clear();
            SearchBox.Click();
            SearchBox.SendKeys(Properties.Location_Name_Value);

            // Press view button
            Locators.LocateButton(Properties.View_Button).Click();
            Locators.WaitElementToShowUp(Properties.Location_Name_Value);

            Assert.IsTrue(IsCMElementExist(Properties.Location_Item_in_Search_Filter_box, Properties.Location_Name_Value));

        }

        [TestMethod]
        public void TC08_DeleteLocationTest()
        {
            if (!IsCMElementExist(Properties.Location_Item_in_Search_Filter_box, Properties.Location_Name_Value))
            {
                TC07_CreateLocationTest();
            }

            Locators.GotoElemnetContextMenu(Locators.LocateChainManagerElement(Properties.Location_Name_Value));
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Locators.WaitElementToShowUpContextMenu(Properties.Delete_Context_Menu);
            Locators.LocateChainManagerElementContextMenu(Properties.Delete_Context_Menu).Click();

            Locators.SwitchToCurrentForm();
            Locators.LocateChainManagerElement(Properties.Yes_Button).Click();

            Locators.SelectLocationFromSearchBox();
            Locators.LocateButton(Properties.View_Button).Click();
            Assert.IsFalse(IsCMElementExist(Properties.Location_Item_in_Search_Filter_box, Properties.Location_Name_Value));

        }

        [TestMethod]
        public void TC09_CreateStationTest()
        {

            if (!IsCMElementExist(Properties.Station_Item_in_Search_Filter_box, Properties.Station_Name_Value))
            {
                // uncheck auto filter checkbox
                if (session.FindElementByClassName(Properties.Filter_CheckBox).Enabled)
                    session.FindElementByClassName(Properties.Filter_CheckBox).Click();

                // Check whether locaiton exist first, if not create it 
                if (!IsCMElementExist(Properties.Location_Item_in_Search_Filter_box, Properties.Location_Name_Value))
                {
                    TC07_CreateLocationTest();
                }

                Locators.GotoElemnetContextMenu(Locators.LocateChainManagerElement(Properties.Location_Name_Value));
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Locators.WaitElementToShowUpContextMenu(Properties.New_Station_Form);
                Locators.LocateChainManagerElementContextMenu(Properties.New_Station_Form).Click();

                Locators.SwitchToCurrentForm();
                //Locators.LocateChainManagerElement(Properties.Cancel_Button).Click();
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_UI_Main).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_UI_Jukebox).Selected);
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Active_Item).Selected);
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Allow_Download_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Hi_Res_Album_Art_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Hi_Res_Video_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Live_Content_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Explicit_Content_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Coin_Operated_Item).Enabled);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Preserve_Main_Content_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Update_CD_Item).Selected);
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Billable_Item).Selected);
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Allow_Mobile_Access_Item).Selected);
                Assert.IsFalse(Locators.LocateChainManagerElement(Properties.Station_Allow_Video_Content_Item).Selected);
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Allow_Audio_Content_Item).Selected);
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Allow_Remote_Notifications_Item).Selected);

                WindowsElement StationName = Locators.LocateChainManagerElement("NameTextBox");
                StationName.Click();
                StationName.SendKeys(Properties.Station_Name_Value);

                WindowsElement GeneratePassword = Locators.LocateButton("Generate");
                GeneratePassword.Click();
                GeneratePassword.SendKeys(Keys.Tab + Keys.Enter);
                //session.FindElementByClassName("Button").Click();

                Locators.SwitchToCurrentForm();
                WindowsElement Country = Locators.LocateChainManagerElement(Properties.Station_Country_Search_box);
                Country.Click();
                Country.SendKeys("Canada" + Keys.Enter);
                Locators.WaitElementToShowUp("Canada");
                Locators.LocateChainManagerElement("Canada").Click();

                Locators.LocateButton(Properties.Save_Button).Click();
                
                // return to station settings form
                Locators.SwitchToCurrentForm();

                Locators.LocateButton(Properties.Save_Button).Click();

                // return to stations & schedules form
                Locators.SwitchToCurrentForm();

                Locators.SwitchToCurrentForm();
                WindowsElement SearchOptionsCombobox = Locators.LocateChainManagerElement(Properties.Search_Options_Filter_box);
                SearchOptionsCombobox.Click();
                SearchOptionsCombobox.SendKeys(Keys.Down); 
                Locators.LocateChainManagerElement(Properties.Station_Item_in_Search_Filter_box).Click();


                WindowsElement SearchBox = Locators.LocateChainManagerElement(Properties.Search_Box);
                SearchBox.Click();
                SearchBox.Clear();
                SearchBox.SendKeys(Properties.Station_Name_Value);

                // Press view button
                Locators.LocateButton(Properties.View_Button).Click();
                Locators.WaitElementToShowUp(Properties.Station_Name_Value);
                Assert.AreEqual(Locators.LocateChainManagerElement(Properties.Station_Name_Value).Text, SearchBox.Text);

            }
        }


        private static bool IsAccountExist()
        {
            try
            {
                Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Account_Name_Value).Displayed);
                return true;
            }
            catch
            {
                try
                {
                    Locators.SelectAccountFromSearchBox();

                    // Enter account name in searchbox
                    WindowsElement SearchBox = Locators.LocateChainManagerElement(Properties.Search_Box);
                    SearchBox.Click();
                    SearchBox.Clear();
                    SearchBox.SendKeys(Properties.Account_Name_Value);

                    // Press view button
                    Locators.LocateButton(Properties.View_Button).Click();

                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Account_Name_Value).Displayed);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        private static bool IsCMElementExist(string elementInFilterbox, string elementValue)
        {
            try
            {
                Assert.IsTrue(Locators.LocateChainManagerElement(elementValue).Displayed);
                return true;
            }
            catch
            {
                try
                {
                    WindowsElement SearchBox = Locators.LocateChainManagerElement(Properties.Search_Box);
                    SearchBox.Clear();

                    if (elementInFilterbox.Equals(Properties.Account_Item_in_Search_Filter_box))
                    {
                        // Enter account name in searchbox
                        Locators.SelectAccountFromSearchBox();
                        SearchBox.Click();
                        SearchBox.SendKeys(Properties.Account_Name_Value);

                        // Press view button
                        Locators.LocateButton(Properties.View_Button).Click();
                        Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Account_Name_Value).Displayed);
                    }

                    else if (elementInFilterbox.Equals(Properties.Location_Item_in_Search_Filter_box))
                    {

                        // check auto filter checkbox
                        if (session.FindElementByClassName(Properties.Filter_CheckBox).Enabled) 
                        {
                            session.FindElementByClassName(Properties.Filter_CheckBox).Click();
                        }
        
                        // Enter location name in searchbox
                        Locators.SelectLocationFromSearchBox();
                        SearchBox.Click();
                        SearchBox.SendKeys(Properties.Location_Name_Value);

                        // Press view button
                        Locators.LocateButton(Properties.View_Button).Click();
                        Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Location_Name_Value).Displayed);
                    }

                    else if (elementInFilterbox.Equals(Properties.Station_Item_in_Search_Filter_box))
                    {
                        // Enter station name in searchbox
                        Locators.SelectStationFromSearchBox();
                        SearchBox.Click();
                        session.FindElementByClassName(Properties.Filter_CheckBox).Click();
                        //Locators.LocateChainManagerElement(Properties.Filter_CheckBox).Click();
                        SearchBox.SendKeys(Properties.Station_Name_Value);

                        // Press view button
                        Locators.LocateButton(Properties.View_Button).Click();
                        Assert.IsTrue(Locators.LocateChainManagerElement(Properties.Station_Name_Value).Displayed);
                    }

                    // Ignore to check auto filter checkbox

                    Thread.Sleep(TimeSpan.FromSeconds(3));
               
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Switch to the stations & schedules window and close the session
            Locators.SwitchToCurrentForm();
            WindowsElement closeAccountWindow = session.FindElementByXPath($"//Button[@Name=\"Close\"]");
            closeAccountWindow.Click();

            // Switch to the main window and close the session
            Locators.SwitchToCurrentForm();

            TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            // Invoke base class test initialization to ensure that the app is in the main page
            base.TestInit();

            try
            {
                if (session.FindElementByXPath($"//Window[starts-with(@Name,\"RX Music Chain Manager\")]").Displayed)
                {

                    //Go to Stations & Schedules window
                    Locators.LocateChainManagerElement(Properties.StationsAndScheduleWindow).Click();

                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    Locators.SwitchToCurrentForm();

                    //Verify CM correctly sitting on 'Stations & Schedules window':
                    //1. CM display "Stations & Schedules window".
                    Assert.IsNotNull(Locators.LocateChainManagerElement(Properties.StationsAndScheduleWindow));

                    //2. Tab 'Accounts' is focused/selected by default
                    WindowsElement accountTab = session.FindElementByName("Accounts");
                    Assert.IsNotNull(accountTab);

                    //3. From filter combobox, item 'Station' is selected by default.
                    WindowsElement accountSearchCombobox = session.FindElementByAccessibilityId("SearchOptions");
                    Assert.IsNotNull(accountSearchCombobox);

                    var comboboxItems = accountSearchCombobox.Text;
                    Assert.IsTrue(comboboxItems.Contains("Station"));
                    Assert.AreEqual(comboboxItems, "{\"name\":\"Station\",\"id\":3}");
                    accountSearchCombobox = null;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
