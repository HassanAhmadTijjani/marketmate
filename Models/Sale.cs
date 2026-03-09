using System;

namespace ShopTracker.Models;

public class Sale
{
     public int SaleId { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateSold { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; }

    public decimal TotalAmount { get; set; }

    public List<SaleItem> SaleItems { get; set; } = [];
}
