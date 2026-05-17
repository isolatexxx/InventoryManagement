using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace UnitTestProject
{
    // UI-тесты автоматизации пользовательского интерфейса с помощью FlaUI
    // Тестируют реальное взаимодействие пользователя с exe приложением
    [TestClass]
    public class UnitTestsUIAutomation
    {
        private Application app;
        private UIA3Automation automation;
        private Window mainWindow;

        // Путь к exe файлу приложения
        private string appPath = @"C:\Users\Asus\OneDrive\Documentos\1_Колледж_ИСПО\3 курс\Тестирование ПМ Костин Ю.Н\1\InventoryManagement\InventoryManagement\bin\Debug\InventoryManagement.exe";

        [TestInitialize]
        public void TestInitialize()
        {
            if (File.Exists("inventory.txt")) 
            { 
                File.Delete("inventory.txt"); 
            }
            app = Application.Launch(appPath);
            automation = new UIA3Automation();
            mainWindow = app.GetMainWindow(automation);
            System.Threading.Thread.Sleep(2000);

            ListAllElements();
        }

        private void ListAllElements()
        {
            try
            {
                var allElements = mainWindow.FindAllDescendants();
                System.Diagnostics.Debug.WriteLine($"Всего найдено элементов: {allElements.Length}");

                foreach (var element in allElements)
                {
                    System.Diagnostics.Debug.WriteLine($"Элемент: Name={element.Name}, ControlType={element.ControlType}, AutomationId={element.AutomationId}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при перечислении элементов: {ex.Message}");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Закрытие приложения
            if (automation != null) { automation.Dispose(); }
            if (app != null) { app.Close(); }

            System.Threading.Thread.Sleep(500);
        }

        // добавление товара с корректными данными
        // проверка на появление товара в списке после добавления
        [TestMethod]
        public void TestAddItem_WithFullData_ItemAppearsInList()
        {
            string itemName = "Футбольный мяч";
            string quantity = "20";
            string price = "3999";
            string category = "Футбол";

            GetUIElements();

            nameTextBox.Text = itemName;
            quantityTextBox.Text = quantity;
            priceTextBox.Text = price;
            categoryTextBox.Text = category;
            addButton.Click();

            System.Threading.Thread.Sleep(500);

            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.IsTrue(itemsListBox.Items.Length > 0, "Товар не был добавлен в список");
            Assert.IsTrue(itemsListBox.Items[0].Text.Contains(itemName), "Название товара не соответствует");
            Assert.IsTrue(itemsListBox.Items[0].Text.Contains(quantity), "Количество товара не соответствует");
            Assert.IsTrue(itemsListBox.Items[0].Text.Contains(price), "Цена товара не соответствует");
        }

        // добавления товара с пустыми полями
        // проверяет что появляется сообщение об ошибке и товар не добавляется
        [TestMethod]
        public void TestAddItem_WithEmptyFields_ShowsErrorMessage()
        {
            GetUIElements();

            nameTextBox.Text = "";
            quantityTextBox.Text = "";
            priceTextBox.Text = "";
            categoryTextBox.Text = "";

            addButton.Click();
            System.Threading.Thread.Sleep(500);

            Window messageBox = mainWindow.ModalWindows.FirstOrDefault();
            Assert.IsNotNull(messageBox, "MessageBox не появился");

            Button okButton = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)).AsButton();
            okButton.Click();

            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.AreEqual(0, itemsListBox.Items.Length, "Товар не должен был добавиться с пустыми полями");
        }

        // Попытка добавления товара с неверным количеством
        // Проверяет что появляется сообщение об ошибке и товар не добавляется
        [TestMethod]
        public void TestAddItem_WithWrongQuantity_ShowsErrorMessage()
        {
            GetUIElements();

            nameTextBox.Text = "Баскетбольное кольцо";
            quantityTextBox.Text = "quantity";
            priceTextBox.Text = "4200";
            categoryTextBox.Text = "Баскетбол";

            addButton.Click();
            System.Threading.Thread.Sleep(500);

            Window messageBox = mainWindow.ModalWindows.FirstOrDefault();
            Assert.IsNotNull(messageBox, "MessageBox не появился при неверном количестве");

            Button okButton = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)).AsButton();
            okButton.Click();

            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.AreEqual(0, itemsListBox.Items.Length, "Товар не должен был добавиться с неверным количеством");
        }

        // Попытка добавления товара с отрицательным количеством
        // Проверяет валидацию отрицательных значений
        [TestMethod]
        public void TestAddItem_WithNegativeQuantity_ShowsErrorMessage()
        {
            GetUIElements();

            nameTextBox.Text = "Лента";
            quantityTextBox.Text = "-20";
            priceTextBox.Text = "1000";
            categoryTextBox.Text = "Гимнастика";

            addButton.Click();
            System.Threading.Thread.Sleep(500);

            Window messageBox = mainWindow.ModalWindows.FirstOrDefault();
            Assert.IsNotNull(messageBox, "MessageBox не появился при отрицательном количестве");

            Button okButton = messageBox.FindFirstDescendant(cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.Button)).AsButton();
            okButton.Click();

            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.AreEqual(0, itemsListBox.Items.Length, "Товар не должен был добавиться с отрицательным количеством");
        }

        // Удаление товара из списка
        // Проверяет что товар удаляется после выбора и нажатия кнопки удаления
        [TestMethod]
        public void TestRemoveItem_WhenItemSelected_RemovesProduct()
        {
            GetUIElements();

            nameTextBox.Text = "Скакалка";
            quantityTextBox.Text = "50";
            priceTextBox.Text = "1000";
            categoryTextBox.Text = "Бокс";

            addButton.Click();
            System.Threading.Thread.Sleep(500);

            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.IsTrue(itemsListBox.Items.Length > 0, "Товар не был добавлен");

            itemsListBox.Items[0].Click();
            System.Threading.Thread.Sleep(300);

            removeButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("removeItemButton")).AsButton();
            removeButton.Click();
            System.Threading.Thread.Sleep(500);

            itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.AreEqual(0, itemsListBox.Items.Length, "Товар не был удалён из списка");
        }

        // Обновление количества товара
        // Проверяет что количество товара обновляется после нажатия кнопки обновления
        [TestMethod]
        public void TestUpdateItem_WhenItemSelected_UpdatesQuantity()
        {
            GetUIElements();

            nameTextBox.Text = "Велосипед";
            quantityTextBox.Text = "10";
            priceTextBox.Text = "55000";
            categoryTextBox.Text = "Веелоспорт";

            addButton.Click();
            System.Threading.Thread.Sleep(500);

            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            itemsListBox.Items[0].Click();
            System.Threading.Thread.Sleep(300);

            quantityTextBox.Text = "";
            quantityTextBox.Text = "25";

            updateButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("updateItemButton")).AsButton();
            updateButton.Click();
            System.Threading.Thread.Sleep(500);

            itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            Assert.IsTrue(itemsListBox.Items[0].Text.Contains("Количество: 25"), "Количество товара не было обновлено");
        }

        [TestMethod]
        public void TestCheckProductAvailability()
        {
            // Добавить товар
            GetUIElements();
            nameTextBox.Text = "Товар для проверки";
            quantityTextBox.Text = "10";
            priceTextBox.Text = "5000";
            categoryTextBox.Text = "Тест";
            addButton.Click();
            System.Threading.Thread.Sleep(500);

            // Выбрать товар
            ListBox itemsListBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("itemsListBox")).AsListBox();
            itemsListBox.Items[0].Click();
            System.Threading.Thread.Sleep(300);

            // Проверить доступность (если в приложении есть такая кнопка)
            // Например, через сообщение или визуальное состояние
            Assert.IsTrue(itemsListBox.Items[0].Text.Contains("10"), "Товар должен быть доступен");
        }


        private TextBox nameTextBox;
        private TextBox quantityTextBox;
        private TextBox priceTextBox;
        private TextBox categoryTextBox;
        private Button addButton;
        private Button removeButton;
        private Button updateButton;

        // Получает элементы UI из главного окна
        // Поиск всех текстовых полей и кнопок в главном окне приложения
        private void GetUIElements()
        {
            try
            {
                // Поиск по AutomationId (имена контролов в Designer)
                nameTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("nameTextBox")).AsTextBox();
                quantityTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("quantityTextBox")).AsTextBox();
                priceTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("priceTextBox")).AsTextBox();
                categoryTextBox = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("categoryTextBox")).AsTextBox();

                if (nameTextBox == null || quantityTextBox == null || priceTextBox == null || categoryTextBox == null)
                {
                    throw new Exception("Не удалось найти один или несколько TextBox элементов по AutomationId");
                }
                System.Diagnostics.Debug.WriteLine("Успешно найдены все TextBox элементы");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при поиске TextBox: {ex.Message}");
                throw;
            }

            try
            {
                addButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("addItemButton")).AsButton();
                removeButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("removeItemButton")).AsButton();
                updateButton = mainWindow.FindFirstDescendant(cf => cf.ByAutomationId("updateItemButton")).AsButton();

                if (addButton == null || removeButton == null || updateButton == null)
                {
                    throw new Exception("Не удалось найти один или несколько Button элементов по AutomationId");
                }
                System.Diagnostics.Debug.WriteLine("Успешно найдены все Button элементы");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при поиске Button: {ex.Message}");
                throw;
            }
        }
    }
}