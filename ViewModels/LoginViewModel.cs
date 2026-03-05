using System;
using System.ComponentModel.DataAnnotations;

namespace ShopTracker.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
