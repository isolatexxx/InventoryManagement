using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement
{

    public partial class InventoryForm : Form
    {
        private InventoryManager inventoryManager;
        private Label nameLabel;
        private Label quantityLabel;
        private Label priceLabel;
        private Label categoryLabel;
        private TextBox nameTextBox;
        private TextBox quantityTextBox;
        private TextBox priceTextBox;
        private TextBox categoryTextBox;
        private Button addItemButton;
        private Button removeItemButton;
        private Button updateQuantityButton;
        private ListBox itemsListBox;

        public InventoryForm()
        {
            this.Text = "Управление инвентарём";
            this.Width = 500;
            this.Height = 400;


            nameLabel = new Label
            {
                Location = new Point(10, 10),
                Text = "Название",
                Width = 150
            };

            quantityLabel = new Label
            {
                Location = new Point(170, 10),
                Width = 80,
                Text = "Количество"
            };

            priceLabel = new Label
            {
                Location = new Point(260, 10),
                Width = 100,
                Text = "Цена"
            };

            categoryLabel = new Label
            {
                Location = new Point(370, 10),
                Width = 100,
                Text = "Категория"
            };

            nameTextBox = new TextBox
            {
                Location = new Point(10, 30),
                Width = 150
            };

            quantityTextBox = new TextBox
            {
                Location = new Point(170, 30),
                Width = 80
            };

            priceTextBox = new TextBox
            {
                Location = new Point(260, 30),
                Width = 100
            };

            categoryTextBox = new TextBox
            {
                Location = new Point(370, 30),
                Width = 100
            };

            addItemButton = new Button
            {
                Location = new Point(10, 60),
                Text = "Добавить",
                Width = 100
            };
            addItemButton.Click += AddItemButton_Click;

            removeItemButton = new Button
            {
                Location = new Point(110, 60),
                Text = "Удалить",
                Width = 100
            };
            removeItemButton.Click += RemoveItemButton_Click;

            updateQuantityButton = new Button
            {
                Location = new Point(210, 60),
                Text = "Обновить",
                Width = 100
            };
            updateQuantityButton.Click += UpdateQuantityButton_Click;

            itemsListBox = new ListBox
            {
                Location = new Point(10, 90),
                Width = 460,
                Height = 200
            };

            Controls.Add(nameTextBox);
            Controls.Add(quantityTextBox);
            Controls.Add(priceTextBox);
            Controls.Add(categoryTextBox);
            Controls.Add(addItemButton);
            Controls.Add(removeItemButton);
            Controls.Add(updateQuantityButton);
            Controls.Add(itemsListBox);
            Controls.Add(nameLabel);
            Controls.Add(quantityLabel);
            Controls.Add(priceLabel);
            Controls.Add(categoryLabel);

            inventoryManager = new InventoryManager();
            UpdateItemsList();
        }

        private void UpdateItemsList()
        {
            itemsListBox.Items.Clear();
            foreach (var item in inventoryManager.Items)
            {
                itemsListBox.Items.Add($"{item.Name} - Количество: {item.Quantity} | Цена: {item.Price} руб. | Категория: {item.Category}");
            }
        }

        private void AddItemButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) ||
                string.IsNullOrEmpty(quantityTextBox.Text) ||
                string.IsNullOrEmpty(priceTextBox.Text) ||
                string.IsNullOrEmpty(categoryTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(quantityTextBox.Text, out int quantity) ||
                !decimal.TryParse(priceTextBox.Text, out decimal price))
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
        }

        private void RemoveItemButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите товар для удаления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedItem = itemsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string name = parts[0].Trim();
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
        }

        private void UpdateQuantityButton_Click(object sender, EventArgs e)
        {
            if (itemsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите товар для обновления!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string selectedItem = itemsListBox.SelectedItem.ToString();
            string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
            if (parts.Length >= 2)
            {
                string name = parts[0].Trim();
                var itemToUpdate = inventoryManager.Items.Find(i => i.Name == name);
                if (itemToUpdate != null)
                {
                    if (string.IsNullOrEmpty(quantityTextBox.Text))
                    {
                        MessageBox.Show("Введите новое количество!");
                        return;
                    }

                    if (!int.TryParse(quantityTextBox.Text, out int newQuantity))
                    {
                        MessageBox.Show("Неверный формат количества!");
                        return;
                    }

                    try
                    {
                        inventoryManager.UpdateItemQuantity(itemToUpdate, newQuantity);
                        UpdateItemsList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
