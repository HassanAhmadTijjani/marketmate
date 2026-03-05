using System;

namespace ShopTracker.Models;

public class Sale
{
     public int SaleId { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public DateTime DateSold { get; set; } = DateTime.Now;

    public string? UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public List<SaleItem> SaleItems { get; set; } = [];
}
