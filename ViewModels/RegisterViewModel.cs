using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShopTracker.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [DisplayName("Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DisplayName("Password")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be atleast 4 character long")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

}
