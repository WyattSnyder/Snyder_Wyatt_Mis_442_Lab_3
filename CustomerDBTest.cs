using NUnit.Framework;

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
    public class CustomerDBTest
    {
        CustomerDB db;

        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Minnie Mouse";
            p.Address = "101 Main Street";
            p.City = "Orlando";
            p.State = "FL";
            p.ZipCode = "10001";

            db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerId);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, p.CustomerId);
            Assert.AreEqual("Molunguri, A", p.Name);
        }
        
        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("Molunguri, A");
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve("Molunguri, A"));
        }

        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("CustomerId");
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("CustomerId");
            p.Name = "Molunguri, A";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve("CustomerId");
            Assert.AreEqual("Molunguri, A", p.Name);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            CustomerProps p = (CustomerProps)db.Retrieve("Oregon");
            p.State = "ore";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            CustomerProps p = new CustomerProps();
            p.CustomerId = 0;
            p.Name = "Wyatt Snyder";
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
    }
}
