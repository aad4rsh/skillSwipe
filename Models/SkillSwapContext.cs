
using Microsoft.EntityFrameworkCore;

namespace skillSewa.Models;

public class SkillSwapContext : DbContext
{
    // DbContext ko constructor jasma options pass garinchha
    public SkillSwapContext(DbContextOptions<SkillSwapContext> options)
        : base(options)
    {
    }

    // User table database ma represent garcha
    public DbSet<User> User { get; set; } = default!;

    // Skill table database ma represent garcha
    public DbSet<Skill> Skill { get; set; } = default!;

    // UserSkill table database ma represent garcha
    public DbSet<UserSkill> UserSkill { get; set; } = default!;
}
