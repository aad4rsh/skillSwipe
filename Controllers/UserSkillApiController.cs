using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skillSewa.Models;

namespace skillSewa.Controllers
{
    // API Controller ho - JSON return garcha, Views hoina
    // Route: /api/userskill
    [Route("api/userskill")]
    [ApiController]
    public class UserSkillApiController : ControllerBase
    {
        // Database context inject gareko
        private readonly SkillSwapContext _context;

        public UserSkillApiController(SkillSwapContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------
        // GET: /api/userskill
        // Sabai UserSkill records return garcha
        // User ra Skill ko details ni satha aaucha (Include)
        // -------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllUserSkills()
        {
            // User ra Skill information sanga join gareko data tanne
            var userSkills = await _context.UserSkill
                .Include(us => us.User)
                .Include(us => us.Skill)
                .Select(us => new
                {
                    us.Id,
                    UserId    = us.UserId,
                    UserName  = us.User != null ? us.User.Name : "Unknown",
                    SkillId   = us.SkillId,
                    SkillName = us.Skill != null ? us.Skill.SkillName : "Unknown",
                    us.CanTeach,
                    us.WantToLearn
                })
                .ToListAsync();

            return Ok(userSkills); // 200 OK with JSON list
        }

        // -------------------------------------------------------
        // GET: /api/userskill/{id}
        // Ek wata specific UserSkill return garcha ID bata
        // -------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUserSkillById(int id)
        {
            // ID le UserSkill khojcha, User ra Skill details sanga
            var userSkill = await _context.UserSkill
                .Include(us => us.User)
                .Include(us => us.Skill)
                .Where(us => us.Id == id)
                .Select(us => new
                {
                    us.Id,
                    UserId    = us.UserId,
                    UserName  = us.User != null ? us.User.Name : "Unknown",
                    SkillId   = us.SkillId,
                    SkillName = us.Skill != null ? us.Skill.SkillName : "Unknown",
                    us.CanTeach,
                    us.WantToLearn
                })
                .FirstOrDefaultAsync();

            // UserSkill fela pardaina bhaney 404 return garcha
            if (userSkill == null)
            {
                return NotFound(new { message = $"UserSkill with ID {id} not found." });
            }

            return Ok(userSkill); // 200 OK with data
        }

        // -------------------------------------------------------
        // POST: /api/userskill
        // Naya UserSkill entry create garcha
        // Body ma JSON pathauney:
        // {
        //   "userId": 1,
        //   "skillId": 2,
        //   "canTeach": true,
        //   "wantToLearn": false
        // }
        // -------------------------------------------------------
        [HttpPost]
        public async Task<ActionResult<UserSkill>> CreateUserSkill([FromBody] UserSkill userSkill)
        {
            // UserId ra SkillId valid chha ki chaina check garcha
            var userExists = await _context.User.AnyAsync(u => u.Id == userSkill.UserId);
            if (!userExists)
            {
                return BadRequest(new { message = $"User with ID {userSkill.UserId} does not exist." });
            }

            var skillExists = await _context.Skill.AnyAsync(s => s.Id == userSkill.SkillId);
            if (!skillExists)
            {
                return BadRequest(new { message = $"Skill with ID {userSkill.SkillId} does not exist." });
            }

            // CanTeach ra WantToLearn duitai false hunna bhayo bhaney validate garcha
            if (!userSkill.CanTeach && !userSkill.WantToLearn)
            {
                return BadRequest(new { message = "At least one of CanTeach or WantToLearn must be true." });
            }

            // Database ma add garcha
            _context.UserSkill.Add(userSkill);
            await _context.SaveChangesAsync();

            // 201 Created return garcha naya record ko location sanga
            return CreatedAtAction(nameof(GetUserSkillById), new { id = userSkill.Id }, userSkill);
        }

        // -------------------------------------------------------
        // PUT: /api/userskill/{id}
        // Existing UserSkill update garcha
        // Body ma JSON pathauney:
        // {
        //   "id": 1,
        //   "userId": 1,
        //   "skillId": 2,
        //   "canTeach": false,
        //   "wantToLearn": true
        // }
        // -------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserSkill(int id, [FromBody] UserSkill userSkill)
        {
            // URL ko ID ra body ko ID match hunu parcha
            if (id != userSkill.Id)
            {
                return BadRequest(new { message = "ID in URL and body do not match." });
            }

            // UserId valid chha ki chaina check
            var userExists = await _context.User.AnyAsync(u => u.Id == userSkill.UserId);
            if (!userExists)
            {
                return BadRequest(new { message = $"User with ID {userSkill.UserId} does not exist." });
            }

            // SkillId valid chha ki chaina check
            var skillExists = await _context.Skill.AnyAsync(s => s.Id == userSkill.SkillId);
            if (!skillExists)
            {
                return BadRequest(new { message = $"Skill with ID {userSkill.SkillId} does not exist." });
            }

            // CanTeach ra WantToLearn duitai false bhayo bhaney validate garcha
            if (!userSkill.CanTeach && !userSkill.WantToLearn)
            {
                return BadRequest(new { message = "At least one of CanTeach or WantToLearn must be true." });
            }

            // Database ma update garcha
            _context.Entry(userSkill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Record exist gardaina bhaney 404
                if (!UserSkillExists(id))
                {
                    return NotFound(new { message = $"UserSkill with ID {id} not found." });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = $"UserSkill with ID {id} updated successfully." }); // 200 OK
        }

        // -------------------------------------------------------
        // DELETE: /api/userskill/{id}
        // UserSkill delete garcha ID bata
        // -------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserSkill(int id)
        {
            // Record khojcha
            var userSkill = await _context.UserSkill.FindAsync(id);

            // Fela pardaina bhaney 404
            if (userSkill == null)
            {
                return NotFound(new { message = $"UserSkill with ID {id} not found." });
            }

            // Database bata delete garcha
            _context.UserSkill.Remove(userSkill);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"UserSkill with ID {id} deleted successfully." }); // 200 OK
        }

        // -------------------------------------------------------
        // Helper method - UserSkill exist garcha ki gardaina check
        // -------------------------------------------------------
        private bool UserSkillExists(int id)
        {
            return _context.UserSkill.Any(e => e.Id == id);
        }
    }
}
