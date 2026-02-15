
namespace skillSewa.Models;

public class User
{
    // User ko unique ID
    public int Id { get; set; }

    // User ko Naam (Name)
    public string Name { get; set; } = string.Empty;

    // User ko Email address
    public string Email { get; set; } = string.Empty;


    // User ko password hash gareko value
    public string PasswordHash { get; set; } = string.Empty;

    // User ko Role (Admin, User, etc.)
    public string Role { get; set; } = "User";
}