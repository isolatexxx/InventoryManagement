using InventoryManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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
            form.TestQuantityTextBox.Text = "50";
            form.TestPriceTextBox.Text = "2000";
            form.UpdateItem();
            var items = form.GetAllItems();
            Assert.AreEqual(1, items.Count, "Количество товаров не должно измениться");
            Assert.AreEqual(50, items[0].Quantity, "Количество должно обновиться на 50");
            Assert.AreEqual(2000, items[0].Price, "Цена должна обновиться на 2000");
        }

        // Проверка: Некорректный выбор товара для удаления или обновления. 

        [TestMethod]
        public void RemoveButton_Click_WhenNoItemSelected_ShowsErrorAndDoesNotRemove()
        {
            form.RemoveItem(); // MessageBox вылетит
            Assert.AreEqual(0, form.ReturnItemsCount(), "Список должен остаться пустым");
        }

        [TestMethod]
        public void UpdateButton_Click_WhenNoItemSelected_ShowsErrorAndDoesNotUpdate()
        {
            form.UpdateItem(); // MessageBox вылетит
            Assert.AreEqual(0, form.ReturnItemsCount(), "Список должен остаться пустым");
        }

        [TestMethod]
        public void RemoveButton_Click_WhenItemSelectedButAlreadyDeleted_ShowsError()
        {
            form.AddItem("Тестовый товар", "10", "100", "Тест");
            form.SelectItem(0);
            form.RemoveItem();
            Assert.AreEqual(0, form.ReturnItemsCount());
            form.RemoveItem(); // MessageBox вылетит
            Assert.AreEqual(0, form.ReturnItemsCount());
        }

        [TestMethod]
        public void UpdateButton_Click_WhenItemSelectedButFieldsEmpty_ShowsError()
        {
            form.AddItem("Тестовый товар", "10", "100", "Тест");
            form.SelectItem(0);
            form.TestQuantityTextBox.Text = "";
            form.TestPriceTextBox.Text = "";
            form.UpdateItem(); // MessageBox вылетит об успешном обновлении товара но цена и кол-во останется прежней
            var items = form.GetAllItems();
            Assert.AreEqual(10, items[0].Quantity, "Количество не должно измениться");
            Assert.AreEqual(100, items[0].Price, "Цена не должна измениться");
        }
    }
}
