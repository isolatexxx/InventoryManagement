using InventoryManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsInventoryManager
    {
        private InventoryManager manager;
        [TestInitialize]
        public void Setup()
        {
            if (File.Exists("inventory.txt")) { File.Delete("inventory.txt"); }
            manager = new InventoryManager();
        }
        [TestMethod]
        public void AddItem_ValidItem_ItemAdded()
        {
            var item = new InventoryItem("Теннисная ракетка", 15, 12000, "Теннис");
            manager.AddItem(item);
            Assert.AreEqual(1, manager.Items.Count);
            Assert.AreEqual("Теннисная ракетка", manager.Items[0].Name);
        }
        [TestMethod]
        public void RemoveItem_ExistingItem_ItemRemoved()
        {
            var item = new InventoryItem("Гимнастические кольца", 20, 99000, "Гимнастика");
            manager.AddItem(item);
            manager.RemoveItem(item);
            Assert.AreEqual(0, manager.Items.Count);
        }
        [TestMethod]
        public void UpdateItemQuantity_ExistingItem_QuantityUpdated()
        {
            var item = new InventoryItem("Баскетбольный мяч", 30, 3300, "Баскетбол");
            manager.AddItem(item);
            manager.UpdateItemQuantity(item, 25);
            Assert.AreEqual(25, manager.Items[0].Quantity);
        }
        [TestMethod]
        public void UpdateItemPrice_ExistingItem_PriceUpdated()
        {
            var item = new InventoryItem("Ракетка", 10, 6300, "Теннис");
            manager.AddItem(item);
            manager.UpdateItemPrice(item, 7800);
            Assert.AreEqual(7800, manager.Items[0].Price);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItem_NegativeQuantity_Error()
        {
            var item = new InventoryItem("Шапочки", -20, 900, "Плавание");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItem_NegativePrice_Error()
        {
            var item = new InventoryItem("Ласты", 20, -1900, "Плавание");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddItem_Null_ThrowsException()
        {
            InventoryItem item = null; // чтобы пройти проверку на нулл
            manager.AddItem(item);
        }
    }
}
