using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InventoryManagement
{

    public partial class InventoryForm : Form
    {
        private InventoryManager inventoryManager;

        // для UI тестов (только геттеры)
        public TextBox TestNameTextBox => nameTextBox;
        public TextBox TestQuantityTextBox => quantityTextBox;
        public TextBox TestPriceTextBox => priceTextBox;
        public TextBox TestCategoryTextBox => categoryTextBox;
        public Button TestAddButton => addItemButton;
        public Button TestRemoveButton => removeItemButton;
        public Button TestUpdateButton => updateItemButton;
        public ListBox TestItemsListBox => itemsListBox;

        public InventoryForm()
        {
            InitializeComponent();  // вызывает дизайнер
            inventoryManager = new InventoryManager();
            UpdateItemsList();
        }

        public List<InventoryItem> GetAllItems()
        {
            return inventoryManager.Items;
        }


        // Метод для добавления вещей в тестах
        public void AddItem(string name, string quantity, string price, string category)
        {
            Console.WriteLine($"Строка: имя={name} | количество={quantity} | цена={price} | категория={category}");

            nameTextBox.Text = name;
            quantityTextBox.Text = quantity;
            priceTextBox.Text = price;
            categoryTextBox.Text = category;

            AddItemButton_Click(addItemButton, EventArgs.Empty);

            Console.WriteLine($"Кол-во строчек после клика = {inventoryManager.Items.Count}");
        }
        // Метод для проверки кол-ва добавленных вещей
        public int ReturnItemsCount() 
        { 
            return inventoryManager.Items.Count; 
        }
        // Метод для выделения объекта 
        public void SelectItem (int index)
        {
            itemsListBox.SetSelected(index, true);
        }
        // Метод для удаления товаров
        public void RemoveItem ()
        {
            RemoveItemButton_Click(removeItemButton, EventArgs.Empty);
        }
        // Метод для обновления товаров
        public void UpdateItem ()
        {
            Console.WriteLine($"Обновление: количество={quantityTextBox.Text} | цена={priceTextBox.Text}");
            UpdateItemButton_Click(updateItemButton, EventArgs.Empty);
            Console.WriteLine($"Кол-во строчек после клика = {inventoryManager.Items.Count}");
        }

        private void UpdateItemsList()
        {
            itemsListBox.Items.Clear();
            foreach (var item in inventoryManager.Items)
            {
                itemsListBox.Items.Add($"Название: {item.Name} | Количество: {item.Quantity} | Цена: {item.Price} руб. | Категория: {item.Category}");
            }
        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {

            Console.WriteLine($"\nAddItemButton_Click сработал. Строчка: '{nameTextBox.Text}', " +
                $"'{quantityTextBox.Text}', '{priceTextBox.Text}', '{categoryTextBox.Text}'");

            if (string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(quantityTextBox.Text) ||
                string.IsNullOrEmpty(priceTextBox.Text) ||
                string.IsNullOrEmpty(categoryTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(quantityTextBox.Text, out int quantity) ||
                !decimal.TryParse(priceTextBox.Text, out decimal price) || 
                quantity < 0 || price < 0)
            {
                MessageBox.Show("Неверный формат количества или цены!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InventoryItem newItem = new InventoryItem(nameTextBox.Text, quantity, price, categoryTextBox.Text);
            try
            {
                inventoryManager.AddItem(newItem);
                nameTextBox.Clear();
                quantityTextBox.Clear();
                priceTextBox.Clear();
                categoryTextBox.Clear();
                UpdateItemsList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            nameTextBox.Text = string.Empty;
            quantityTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            categoryTextBox.Text = string.Empty;
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите товар для удаления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedItem = itemsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '|' }, StringSplitOptions.None);
            if (parts.Length == 4)
            {
                string name = parts[0].Replace("Название: ", "").Trim();
                var itemToRemove = inventoryManager.Items.Find(i => i.Name == name);
                if (itemToRemove != null)
                {
                    try
                    {
                        inventoryManager.RemoveItem(itemToRemove);
                        UpdateItemsList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            nameTextBox.Text = string.Empty;
            quantityTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            categoryTextBox.Text = string.Empty;
        }

        private void UpdateItemButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите товар для обновления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedItem = itemsListBox.SelectedItem.ToString();

            string[] parts = selectedItem.Split('|');

            if (parts.Length >= 4)
            {
                string name = parts[0].Replace("Название: ", "").Trim();
                var itemToUpdate = inventoryManager.Items.Find(i => i.Name == name);

                if (itemToUpdate == null)
                {
                    MessageBox.Show("Товар не найден!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool hasError = false;

                if (!string.IsNullOrEmpty(quantityTextBox.Text))
                {
                    if (!int.TryParse(quantityTextBox.Text, out int newQuantity))
                    {
                        MessageBox.Show("Неверный формат количества!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        hasError = true;
                    }
                    else if (newQuantity < 0)
                    {
                        MessageBox.Show("Количество не может быть отрицательным!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        hasError = true;
                    }
                    else
                    {
                        inventoryManager.UpdateItemQuantity(itemToUpdate, newQuantity);
                    }
                }

                if (!string.IsNullOrEmpty(priceTextBox.Text) && !hasError)
                {
                    if (!decimal.TryParse(priceTextBox.Text, out decimal newPrice))
                    {
                        MessageBox.Show("Неверный формат цены!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        hasError = true;
                    }
                    else if (newPrice < 0)
                    {
                        MessageBox.Show("Цена не может быть отрицательной!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        hasError = true;
                    }
                    else
                    {
                        inventoryManager.UpdateItemPrice(itemToUpdate, newPrice);
                    }
                }

                if (!hasError)
                {
                    UpdateItemsList();
                    quantityTextBox.Clear();
                    priceTextBox.Clear();
                    MessageBox.Show("Товар успешно обновлён!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Не удалось разобрать данные товара!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            nameTextBox.Text = string.Empty;
            quantityTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            categoryTextBox.Text = string.Empty;
        }

        private void ItemsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1) { return; }

            string selectedItem = itemsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split('|');

            if (parts.Length >= 4)
            {
                string name = parts[0].Replace("Название: ", "").Trim();
                string quantityStr = parts[1].Replace("Количество: ", "").Trim(); // кол-во
                string priceStr = parts[2].Replace("Цена: ", "").Replace(" руб.", "").Trim(); // цена
                string category = parts[3].Replace("Категория: ", "").Trim();
                nameTextBox.Text = name;
                quantityTextBox.Text = quantityStr;
                priceTextBox.Text = priceStr;
                categoryTextBox.Text = category;
            }
        }
    }
}
