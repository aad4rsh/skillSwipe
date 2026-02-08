
namespace skillSewa.Models;

public class Skill
{
    // Skill ko unique ID
    public int Id { get; set; }

    // Skill ko Naam (jastai C#, Cooking, etc.)
    public string SkillName { get; set; } = string.Empty;
}