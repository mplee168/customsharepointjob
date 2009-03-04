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

    }
}
