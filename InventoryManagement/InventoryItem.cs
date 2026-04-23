using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class InventoryItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }

    public InventoryItem(string name, int quantity, decimal price, string category)
    {
        if (quantity < 0)
            throw new ArgumentException("Количество не может быть отрицательным!");

        if (price < 0)
            throw new ArgumentException("Цена не может быть отрицательной!");

        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Название не может быть пустым!");

        if (string.IsNullOrEmpty(category))
            throw new ArgumentException("Категория не может быть пустой!");

        Name = name;
        Quantity = quantity;
        Price = price;
        Category = category;
    }

    public bool IsAvailable ()
    {
        return Quantity >= 0 && Price >= 0 && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Category);
    }

    public override string ToString ()
    {
        return $"Название: {Name} | Количество: {Quantity} | Цена: {Price} руб. | Категория: {Category}";
    }
}