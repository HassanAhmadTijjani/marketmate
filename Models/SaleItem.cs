using System;

namespace ShopTracker.Models;

public class SaleItem
{
    public int SaleItemId { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal PriceAtSale { get; set; }
    public decimal CostAtSale { get; set; }

    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
}
