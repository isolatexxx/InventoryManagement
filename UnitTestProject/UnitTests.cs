using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using InventoryManagement;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTests
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
}
