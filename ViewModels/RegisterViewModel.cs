
using System.ComponentModel.DataAnnotations;

namespace skillSewa.ViewModels;

public class RegisterViewModel
{
    // Required name field
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    // Required valid email field
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    // Required password field
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    // Required confirm password field matching Password
    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; }
}
