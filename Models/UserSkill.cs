
namespace skillSewa.Models;

public class UserSkill
{
    // UserSkill relationship ko unique ID
    public int Id { get; set; }

    // Kun User ho tyo denote garcha
    public int UserId { get; set; }
    public User? User { get; set; }

    // Kun Skill ho tyo denote garcha
    public int SkillId { get; set; }
    public Skill? Skill { get; set; }

    // K user le yo skill sikauna sakcha? (True/False)
    public bool CanTeach { get; set; }

    // K user le yo skill sikna chahancha? (True/False)
    public bool WantToLearn { get; set; }
}
