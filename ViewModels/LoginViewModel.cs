
using System.ComponentModel.DataAnnotations;

namespace skillSewa.ViewModels;

public class LoginViewModel
{
    // Required email field
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    // Required password field
    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
