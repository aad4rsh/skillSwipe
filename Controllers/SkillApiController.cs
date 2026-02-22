using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skillSewa.Models;

namespace skillSewa.Controllers
{
    // API Controller ho - yo MVC Controller jasto View return gardaina, JSON return garcha
    // Route: /api/skill
    [Route("api/skill")]
    [ApiController]
    public class SkillApiController : ControllerBase
    {
        // Database context inject gareko
        private readonly SkillSwapContext _context;

        public SkillApiController(SkillSwapContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------
        // GET: /api/skill
        // Sabai skills ko list return garcha
        // -------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAllSkills()
        {
            // Database bata sabai skill fetch garcha
            var skills = await _context.Skill.ToListAsync();
            return Ok(skills); // 200 OK with JSON list
        }

        // -------------------------------------------------------
        // GET: /api/skill/{id}
        // Ek wata specific skill return garcha ID bata
        // -------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetSkillById(int id)
        {
            // ID le skill khojcha
            var skill = await _context.Skill.FindAsync(id);

            // Skill na fela paryo bhaney 404 Not Found
            if (skill == null)
            {
                return NotFound(new { message = $"Skill with ID {id} not found." });
            }

            return Ok(skill); // 200 OK with skill data
        }

        // -------------------------------------------------------
        // POST: /api/skill
        // Naya skill create garcha
        // Body ma JSON pathauney: { "skillName": "C#" }
        // -------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<Skill>> CreateSkill([FromBody] Skill skill)
        {
            // SkillName empty chha ki chaina check garcha
            if (string.IsNullOrWhiteSpace(skill.SkillName))
            {
                return BadRequest(new { message = "SkillName cannot be empty." });
            }

            // Skill database ma add garcha
            _context.Skill.Add(skill);
            await _context.SaveChangesAsync();

            // 201 Created + naya skill ko location header pathaucha
            return CreatedAtAction(nameof(GetSkillById), new { id = skill.Id }, skill);
        }

        // -------------------------------------------------------
        // PUT: /api/skill/{id}
        // Existing skill update garcha
        // Body ma JSON pathauney: { "id": 1, "skillName": "Python" }
        // -------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromBody] Skill skill)
        {
            // URL ko ID ra body ko ID match hunu parcha
            if (id != skill.Id)
            {
                return BadRequest(new { message = "ID in URL and body do not match." });
            }

            // SkillName empty chha ki chaina check garcha
            if (string.IsNullOrWhiteSpace(skill.SkillName))
            {
                return BadRequest(new { message = "SkillName cannot be empty." });
            }

            // Database ma update garcha
            _context.Entry(skill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Skill exist nai gardaina bhaney 404 return garcha
                if (!SkillExists(id))
                {
                    return NotFound(new { message = $"Skill with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = $"Skill with ID {id} updated successfully." }); // 200 OK
        }

        // -------------------------------------------------------
        // DELETE: /api/skill/{id}
        // Skill delete garcha ID bata
        // -------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            // Skill khojcha
            var skill = await _context.Skill.FindAsync(id);

            // Skill fela pardaina bhaney 404
            if (skill == null)
            {
                return NotFound(new { message = $"Skill with ID {id} not found." });
            }

            // Database bata remove garcha
            _context.Skill.Remove(skill);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Skill '{skill.SkillName}' deleted successfully." }); // 200 OK
        }

        // -------------------------------------------------------
        // Helper method - Skill exist garcha ki gardaina check
        // -------------------------------------------------------
        private bool SkillExists(int id)
        {
            return _context.Skill.Any(e => e.Id == id);
        }
    }
}
