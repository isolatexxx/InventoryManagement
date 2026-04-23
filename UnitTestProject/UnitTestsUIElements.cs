using InventoryManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestsUIElements
    {
        private InventoryForm form;

        [TestInitialize]
        public void Setup()
        {
            form = new InventoryForm();
        }

        [TestMethod]
        public void NameTextBox_IsEnabled()
        {
            Assert.IsTrue(form.TestNameTextBox.Enabled);
        }

        [TestMethod]
        public void QuantityTextBox_IsEnabled()
        {
            Assert.IsTrue(form.TestQuantityTextBox.Enabled);
        }

        [TestMethod]
        public void PriceTextBox_IsEnabled()
        {
            Assert.IsTrue(form.TestPriceTextBox.Enabled);
        }

        [TestMethod]
        public void CategoryTextBox_IsEnabled()
        {
            Assert.IsTrue(form.TestCategoryTextBox.Enabled);
        }

        [TestMethod]
        public void AddButton_IsEnabled()
        {
            Assert.IsTrue(form.TestAddButton.Enabled);
        }

        [TestMethod]
        public void RemoveButton_IsEnabled()
        {
            Assert.IsTrue(form.TestRemoveButton.Enabled);
        }

        [TestMethod]
        public void UpdateButton_IsEnabled()
        {
            Assert.IsTrue(form.TestUpdateButton.Enabled);
        }

        [TestMethod]
        public void ItemsListBox_IsEnabled()
        {
            Assert.IsTrue(form.TestItemsListBox.Enabled);
        }
    }
}
