using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement
{
    public class InventoryManager
    {
        public List<InventoryItem> Items { get; private set; }

        public InventoryManager()
        {
            Items = new List<InventoryItem>();
            LoadItems();
        }

        public void AddItem(InventoryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            Items.Add(item);
            SaveItems();
        }

        public void RemoveItem(InventoryItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            Items.Remove(item);
            SaveItems();
        }

        public void UpdateItemQuantity(InventoryItem item, int newQuantity)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            item.Quantity = newQuantity;
            SaveItems();
        }

        private void SaveItems()
        {
            File.WriteAllLines("inventory.txt", Items.Select(i =>
    $"{i.Name}|{i.Quantity}|{i.Price}|{i.Category}"));
        }

        private void LoadItems()
        {
            if (File.Exists("inventory.txt"))
            {
                var lines = File.ReadAllLines("inventory.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 4)
                    {
                        int quantity;
                        decimal price;
                        if (int.TryParse(parts[1], out quantity) && decimal.TryParse(parts[2], out price))
                        {
                            Items.Add(new InventoryItem(parts[0], quantity, price, parts[3]));
                        }
                    }
                }
            }
        }
    }
}
