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
    public class ProductsDBTest
    {
        ProductDB db;

        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "A4CS";
            p.Description = "101 Main Street";
            p.UnitPrice = "50";
            p.OnHandsQuantity = 4;

            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductId);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, p.ProductId);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(0, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve("Murach");
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve("Murach"));
        }

        [Test]
        public void TestDeleteForeignKeyConstraint()
        {
            ProductProps p = (ProductProps)db.Retrieve("ProductId");
            Assert.Throws<MySqlException>(() => db.Delete(p));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve("ProductId");
            p.ProductCode = "A4CS";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve("ProductId");
            Assert.AreEqual("A4CS", p.ProductCode);
        }

        [Test]
        public void TestUpdateFieldTooLong()
        {
            ProductProps p = (ProductProps)db.Retrieve("ProductCode");
            p.ProductCode = "AS1234567890";
            Assert.Throws<MySqlException>(() => db.Update(p));
        }

        [Test]
        public void TestCreatePrimaryKeyViolation()
        {
            ProductProps p = new ProductProps();
            p.ProductId = 0;
            p.Description = "Book";
            Assert.Throws<MySqlException>(() => db.Create(p));
        }
    }
}
