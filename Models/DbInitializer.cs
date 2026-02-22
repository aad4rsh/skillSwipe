
using skillSewa.Models;

namespace skillSewa.Models;

public static class DbInitializer
{
    public static void Initialize(SkillSwapContext context)
    {
        // Check if ANY admin exists
        if (context.User.Any(u => u.Role == "Admin"))
        {
            return; // Admin already exists
        }

        // Create default admin
        var admin = new User
        {
            Name = "System Admin",
            Email = "admin@skillsewa.com",
            Role = "Admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
        };

        context.User.Add(admin);
        context.SaveChanges();
    }
}
