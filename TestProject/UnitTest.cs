using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharePointJobs;

namespace TestProject
{
    /// <summary>
    /// Summary description for UnitTest
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        const string url = "http://apldevmoss:33768/sites/TopLevelSiteCollection";
        public UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            PendingReminder pr = new PendingReminder();
            Console.WriteLine("Count: " + pr.Count);
        }

        [TestMethod]
        public void TestOldSIR()
        {
            PendingReminder pr = new PendingReminder();
            pr.SendReminder();
        }

        [TestMethod]
        public void TestInsertSIR()
        {
            PendingReminder pr = new PendingReminder();
            pr.Insert();
        }

        [TestMethod]
        public void TestChangeSIR()
        {
            PendingReminder pr = new PendingReminder();
            pr.Change();
        }

        [TestMethod]
        public void TestDeleteSIR()
        {
            PendingReminder pr = new PendingReminder();
            pr.Delete();
        }

        [TestMethod]
        public void TestRelationSIR()
        {
            PendingReminder pr = new PendingReminder();
            pr.Relation();
        }

        [TestMethod]
        public void TestAddConnectionString()
        {
            WebConfigModifier.addConnectionString(url);
        }

        [TestMethod]
        public void TestRemoveConnectionString()
        {
            WebConfigModifier.removeConnectionString(url);
        }

        [TestMethod]
        public void TestSplit()
        {
            List<string> testString = new List<string> { @"701;#chris.schwimmer@apollogrp.edu",@"5358;#(602) 557-6941", "pong.lee@apollogrp.edu", @"5358;#pong.lee@apollogrp.edu"};
            foreach (var s in testString)
            {
                var splits = s.Split( new string[] {";#"}, StringSplitOptions.RemoveEmptyEntries );
                Console.WriteLine(splits[splits.Length - 1]);
            }

        }

        [TestMethod]
        public void TestWebconfigConnectionStringReading()
        {
            WebConfigModifier.DisplayConnectionString(url);
        }

        [TestMethod]
        public void TestWebconfigAppSettingsReading()
        {
            WebConfigModifier.DisplayAppSettings(url);
        }


    }
}
