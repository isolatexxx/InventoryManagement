using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using InventoryManagement;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsInventoryForm
    {
        private InventoryForm form; // добавлена ссылка на объект формы

        [TestInitialize]
        public void Setup()
        {
            // Удаляем файл хранилища перед созданием формы, чтобы InventoryManager загрузил пустое состояние
            if (File.Exists("inventory.txt")) { File.Delete("inventory.txt"); }
            form = new InventoryForm(); // каждый тест - создание объекта.

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
            form.AddItem("Футбольный мяч", "20", "3999", "Футбол");
            Assert.AreEqual(1, form.ReturnItemsCount(), "Строчка с заполненными полями НЕ добавлена в список!");
        }

        [TestMethod]
        public void AddItem_EmptyFields_Error()
        {
            form.AddItem("Хоккейная клюшка", "", "", "");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с пустыми полями добавлена в список!");
        }

        [TestMethod]
        public void AddItem_WrongQuantity_Error()
        {
            form.AddItem("Баскетбольное кольцо", "quantity", "4200", "Баскетбол");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с неверным кол-вом попала в список!");
        }

        [TestMethod]
        public void AddItem_WrongPrice_Error()
        {
            // Проверяем парсинг прямо в тесте
            bool canParse = decimal.TryParse("price", out decimal testPrice);
            Console.WriteLine($"decimal.TryParse('price') = {canParse}, result = {testPrice}");

            form.AddItem("Боксерский шлем", "10", "price", "Бокс");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с неверной ценой попала в список!");
        }

        // ЮНИТ-ТЕСТЫ 2ЛР
        // ЮНИТ-ТЕСТЫ 2ЛР
        // ЮНИТ-ТЕСТЫ 2ЛР

        [TestMethod]
        public void AddButton_Click_WhenNegativePrice_ShowMessageBoxAndError()
        {
            form.AddItem("Насос для мячей", "6", "-1500", "Обслуживание");
            Assert.AreEqual(0, form.ReturnItemsCount());
        }

        [TestMethod]
        public void AddButton_Click_WhenNegativeQuantity_ShowMessageBoxAndError()
        {
            form.AddItem("Лента", "-20", "1000", "Гимнастика");
            Assert.AreEqual(0, form.ReturnItemsCount());
        }

        [TestMethod]
        public void RemoveButton_Click_WhenItemSelected_RemovesProduct()
        {
            form.AddItem("Скакалка", "50", "1000", "Бокс");
            form.SelectItem(0);
            form.RemoveItem();
            Assert.AreEqual(0, form.ReturnItemsCount());
        }
        [TestMethod]
        public void UpdateButton_Click_WhenItemSelected_UpdatesProduct()
        {
            form.AddItem("Боксерские бинты", "30", "1500", "Бокс");
            Assert.AreEqual(1, form.ReturnItemsCount(), "Товар должен добавиться перед тестом");
            form.SelectItem(0);
            form.quantityTextBox.Text = "50";
            form.priceTextBox.Text = "2000";    
            form.UpdateItem();
            var items = form.GetAllItems();
            Assert.AreEqual(1, items.Count, "Количество товаров не должно измениться");
            Assert.AreEqual(50, items[0].Quantity, "Количество должно обновиться на 50");
            Assert.AreEqual(2000, items[0].Price, "Цена должна обновиться на 2000");
        }

    }

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
            InventoryItem item = null;
            manager.AddItem(item);
        }





    }




}
