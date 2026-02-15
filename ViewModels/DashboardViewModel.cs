
using skillSewa.Models;

namespace skillSewa.ViewModels;

public class DashboardViewModel
{
    // User Section
    public int TotalSkillsOffered { get; set; }
    public int TotalSkillsLearned { get; set; }
    public int PotentialMatches { get; set; }
    public List<UserSkill> RecentMatches { get; set; } = new List<UserSkill>();

    // Admin Section
    public bool IsAdmin { get; set; }
    public int TotalUsers { get; set; }
    public int TotalSkills { get; set; }
}
