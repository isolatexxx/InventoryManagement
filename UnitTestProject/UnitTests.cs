using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using InventoryManagement;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsInventoryForm
    {
        [TestInitialize]
        public void Setup()
        {
            if (File.Exists("inventory.txt"))
            {
                File.Delete("inventory.txt");
            }
        }

        /*
        4. Проверка граничных значений: 
           - Пустые поля при добавлении товара. 
           - Некорректный формат количества или цены. 
           - Некорректный выбор товара для удаления или обновления. 
        */

        [TestMethod]
        public void AddItem_FullFields_StrokeAdd()
        {
            var form = new InventoryForm();
            form.AddItem("Футбольный мяч", "20", "3999", "Футбол");
            Assert.AreEqual(1, form.ReturnItemsCount(), "Строчка с заполненными полями НЕ добавлена в список!");
        }

        [TestMethod]
        public void AddItem_EmptyFields_Error()
        {
            var form = new InventoryForm();
            form.AddItem("Хоккейная клюшка", "", "", "");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с пустыми полями добавлена в список!");
        }

        [TestMethod]
        public void AddItem_WrongQuantity_Error ()
        {
            var form = new InventoryForm();
            form.AddItem("Баскетбольное кольцо", "quantity", "4200", "Баскетбол");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с неверным кол-вом попала в список!");
        }

        [TestMethod]
        public void AddItem_WrongPrice_Error()
        {
            var form = new InventoryForm();
            form.AddItem("Боксерский шлем", "10", "price", "Бокс");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с неверной ценой попала в список!");
        }
    }

    [TestClass]
    public class UnitTestsInventoryItem
    {
        [TestInitialize]
        public void Setup ()
        {
            if (File.Exists("inventory.txt"))
            {
                File.Delete("inventory.txt");
            }
        }

        [TestMethod]
        public void AddItem_IsAvailable_CorrectData_ReturnTrue ()
        {
            InventoryItem item = new InventoryItem("Велосипед", 10, 55000, "Веелоспорт");
            var result = item.IsAvailable();
            Console.WriteLine($"Корректность записи стороки: {result}");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AddItem_IsAvailable_IncrorrectData_ReturnFalse ()
        {
            var item = new InventoryItem("Test", 0, 0, "");
            var result = item.IsAvailable();
            Console.WriteLine($"Корректность записи стороки: {result}");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddItem_ToString_CorrectFormat ()
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
        public void AddItem_IsAvailable_WhenPriceIsNegative_ReturnFalse ()
        {
            var item = new InventoryItem("Футбольные щитки", 11, -55000, "Футбол");
            var result = item.IsAvailable();
            var resultString = item.ToString();
            Console.WriteLine(resultString);
            Console.WriteLine("Корректность записи строки: " + result);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AddItem_IsAvailable_WhenQuantityIsNegative_ReturnFalse()
        {
            var item = new InventoryItem("Гимнастические кольца", -5, 99000, "Гимнастика");
            var result = item.IsAvailable();
            var resultString = item.ToString();
            Console.WriteLine(resultString);
            Console.WriteLine("Корректность записи строки: " + result);
            Assert.IsFalse(result);
        }





    }



}
