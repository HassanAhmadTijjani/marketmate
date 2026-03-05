using System;

namespace ShopTracker.Models;

public class Dashboard
{
     public decimal TotalRevenue { get; set; }      // Total money earned from sales
        public decimal TotalCost { get; set; }         // Total cost of sold items
        public int TotalItemsSold { get; set; }        // Number of items sold
        public int TotalProductsInStock { get; set; }  // Total remaining stock
        public List<string> SoldOutProducts { get; set; } = new List<string>(); // Names of sold-out products

        public decimal Profit => TotalRevenue - TotalCost; // Profit calculation
}
