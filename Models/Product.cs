using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShopTracker.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    [DisplayName("Name")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Cost price is required")]
    [DisplayName("Cost Price")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPrice { get; set; }
    
    [Required(ErrorMessage = "Selling price is required")]
    [DisplayName("Selling Price")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SellingPrice { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    [DisplayName("Quantity")]
    public int QuantityInStock { get; set; }

    [NotMapped]
    public decimal Profit => SellingPrice - CostPrice;
    // public int LowStock => QuantityInStock < 10 ? 10 - QuantityInStock : 0;

    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    [ValidateNever]
    public ApplicationUser User { get; set; } = null!;

    public DateTime DateCreated { get; set; } = DateTime.Now;




}
