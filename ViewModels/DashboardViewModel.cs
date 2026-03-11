using System;
using ShopTracker.Models;

namespace ShopTracker.ViewModels;

public class DashboardViewModel
{
    // Total Revenue for the shop
    public decimal TotalRevenue { get; set; }

    // Total Cost for products sold
    public decimal TotalCost { get; set; }

    // Total Profit
    public decimal TotalProfit => TotalRevenue - TotalCost;

    // Number of products sold
    public int TotalItemsSold { get; set; }

    // Number of products in stock
    public int TotalProductsInStock { get; set; }

    // Products that are completely sold out
    public List<string> SoldOutProducts { get; set; } = new List<string>();

public decimal WeeklyRevenue { get; set; }
public decimal WeeklyProfit { get; set; }
public int WeeklyItemsSold { get; set; }
public decimal DailyRevenue { get; set; }
public decimal DailyProfit { get; set; }
public int DailyItemsSold { get; set; }

    // public decimal TotalRevenue { get; set; }      // Total money earned from sales
    //         public decimal TotalCost { get; set; }         // Total cost of sold items
    //         public int TotalItemsSold { get; set; }        // Number of items sold
    //         public int TotalProductsInStock { get; set; }  // Total remaining stock
    //         public List<string> SoldOutProducts { get; set; } = new List<string>(); // Names of sold-out products

    //         public decimal Profit => TotalRevenue - TotalCost; // Profit calculation
}
