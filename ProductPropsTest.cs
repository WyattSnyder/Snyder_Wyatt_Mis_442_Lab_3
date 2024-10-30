using NUnit.Framework;

using MMABooksProps;
using System;

namespace MMABooksTests
{
    [TestFixture]
     public class ProductPropsTests
        {
            ProductProps props;
            [SetUp]
            public void Setup()
            {
                props = new ProductProps();
                props.ProductId = 1;
                props.ProductCode = "An48";
                props.Description = "book";
                props.UnitPrice = "57";
                props.OnHandsQuantity = 1;
            }

        [Test]
        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
            Assert.IsTrue(jsonString.Contains(props.Description));
        }

        [Test]
        public void TestSetState()
        {
            string jsonString = props.GetState();
            ProductProps newProps = new ProductProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductId, newProps.ProductId);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
        
        [Test]
        public void TestClone()
        {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductId, newProps.ProductId);
            Assert.AreEqual(props.ProductCode, newProps.ProductCode);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}
