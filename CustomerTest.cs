using NUnit.Framework;

using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    internal class CustomerTest
    {
        [SetUp]
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetStateData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewStateConstructor()
        {
            // not in Data Store - no code
            Customer c = new Customer();

            Assert.AreEqual(string.Empty, c.Name);
            Assert.AreEqual(string.Empty, c.Address);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }

        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.Name = "Lindy, Stewart";
            c.Address = "101 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            c.Save();
            Customer c2 = new Customer(c.CustomerId);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer();
            c.Name = "Wyatt, Snyder";
            c.Address = "101 Main Street";
            c.City = "Orlando";
            c.State = "FL";
            c.ZipCode = "10001";
            c.Save();

            Customer c2 = new Customer(c.CustomerId);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }
        [Test]
        public void TestDelete()
        {
            Customer c = new Customer();
            c.Name = "Wyatt, Snyder";
            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(c.CustomerId));
        }

        [Test]
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            Assert.AreEqual("1414 S. Dairy Ashford", customers[0].Address);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - address and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - Address and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Address = "??";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "???");
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer();
            Customer c2 = new Customer();
            
            c1.Name = "Molunguri, A";
            c1.City = "Birmingham";
            c1.State = "AL";
            c1.Address = "1108 Johanna Bay Drive";
            c1.ZipCode = "35216-6909";
            c1.Save();

            c2.Name = "Molunguri, B";
            c2.City = "Birmingha";
            c2.State = "FL";
            c2.Address = "1108 Johann Bay Drive";
            c2.ZipCode = "35216-690";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}
