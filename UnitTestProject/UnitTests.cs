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

        [TestMethod]
        public void AddItem_FullFields()
        {
            var form = new InventoryForm();
            
            form.AddItem("Футбольный мяч", "20", "3999", "Футбол");
            Assert.AreEqual(1, form.ReturnItemsCount(), "Строчка с заполненными полями НЕ добавлена в список!");
        }

        [TestMethod]
        public void AddItem_EmptyFields()
        {
            var form = new InventoryForm();

            form.AddItem("Хоккейная клюшка", "", "", "");
            Assert.AreEqual(0, form.ReturnItemsCount(), "Строчка с пустыми полями добавлена в список!");
        }








    }
}
