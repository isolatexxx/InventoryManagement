using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsInventoryItem
    {
        [TestInitialize]
        public void Setup()
        {
            if (File.Exists("inventory.txt")) { File.Delete("inventory.txt"); }
        }

        [TestMethod]
        public void AddItem_IsAvailable_CorrectData_ReturnTrue()
        {
            InventoryItem item = new InventoryItem("Велосипед", 10, 55000, "Веелоспорт");
            var result = item.IsAvailable();
            Console.WriteLine($"Корректность записи стороки: {result}");
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItem_IsAvailable_IncrorrectData_ReturnFalse()
        {
            var item = new InventoryItem("Test", 0, 0, "");
        }

        [TestMethod]
        public void AddItem_ToString_CorrectFormat()
        {
            var item = new InventoryItem("Волейбольный мяч", 10, 3300, "Волейбол");
            var result = item.ToString();
            Console.WriteLine(result);
            StringAssert.Contains(result, "Название: Волейбольный мяч");
            StringAssert.Contains(result, "Количество: 10");
            StringAssert.Contains(result, "Цена: 3300");
            StringAssert.Contains(result, "Категория: Волейбол");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItem_IsAvailable_WhenPriceIsNegative_ReturnFalse()
        {
            var item = new InventoryItem("Футбольные щитки", 11, -55000, "Футбол");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddItem_IsAvailable_WhenQuantityIsNegative_ReturnFalse()
        {
            var item = new InventoryItem("Гимнастические кольца", -5, 99000, "Гимнастика");
        }
    }
}
