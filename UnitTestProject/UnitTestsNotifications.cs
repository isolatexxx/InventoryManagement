using InventoryManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsNotifications
    {
        private InventoryForm form;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists("inventory.txt"))
            {
                File.Delete("inventory.txt");
            }
            form = new InventoryForm();
        }

        [TestMethod]
        public void LowStockNotification_WhenQuantityBelowThreshold_ReturnsTrue()
        {
            int threshold = 5;
            int quantity = 3;
            form.AddItem("ТЕСТ1", quantity.ToString(), "100", "Спорт");

            bool shouldNotify = form.ShouldShowLowStockNotification(threshold);

            Assert.IsTrue(shouldNotify, $"Товар с количеством {quantity} должен вызвать уведомление при пороге {threshold}");
        }

        [TestMethod]
        public void LowStockNotification_WhenQuantityAboveThreshold_ReturnsFalse()
        {
            int threshold = 5;
            int quantity = 10;
            form.AddItem("ТЕСТ2", quantity.ToString(), "100", "Спорт");

            bool shouldNotify = form.ShouldShowLowStockNotification(threshold);

            Assert.IsFalse(shouldNotify, $"Товар с количеством {quantity} НЕ должен вызывать уведомление при пороге {threshold}");
        }

        [TestMethod]
        public void LowStockNotification_WhenQuantityEqualsThreshold_ReturnsTrue()
        {
            int threshold = 5;
            int quantity = 5;
            form.AddItem("ТЕСТ3", quantity.ToString(), "100", "Спорт");

            bool shouldNotify = form.ShouldShowLowStockNotification(threshold);

            Assert.IsTrue(shouldNotify, $"Товар с количеством {quantity} должен вызвать уведомление при пороге {threshold}");
        }

        [TestMethod]
        public void LowStockNotification_WhenMultipleItems_OnlyLowStockItemsTrigger()
        {
            int threshold = 5;
            int quantity1 = 3;
            int quantity2 = 10;
            int quantity3 = 2;
            form.AddItem("ТЕСТ4", quantity1.ToString(), "100", "Спорт");
            form.AddItem("ТЕСТ5", quantity2.ToString(), "200", "Спорт");
            form.AddItem("ТЕСТ6", quantity3.ToString(), "500", "Спорт");

            var lowStockItems = form.GetLowStockItems(threshold);

            Assert.AreEqual(2, lowStockItems.Count, $"Должно быть 2 товара с низким запасом при пороге {threshold}");
            Assert.AreEqual("ТЕСТ4", lowStockItems[0].Name);
            Assert.AreEqual("ТЕСТ6", lowStockItems[1].Name);
        }

    
    }
}