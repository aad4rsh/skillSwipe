using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using skillSewa.Models;

namespace skillSewa.Controllers
{
    // Login gareko user le matra access garna pauncha
    [Authorize]
    public class UserSkillController : Controller
    {
        private readonly SkillSwapContext _context;

        public UserSkillController(SkillSwapContext context)
        {
            _context = context;
        }

        // GET: UserSkill
        // Sabai UserSkill relationship haru list garne method
        public async Task<IActionResult> Index()
        {
            // Database bata UserSkill data tanne ani tyaslai User ra Skill table sanga join garne
            var skillSwapContext = _context.UserSkill.Include(u => u.Skill).Include(u => u.User);
            return View(await skillSwapContext.ToListAsync());
        }

        // GET: UserSkill/Details/5
        // Kunai euta specific UserSkill ko details herne method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserSkill == null)
            {
                return NotFound();
            }

            // Id ko adhar ma UserSkill data khojne
            var userSkill = await _context.UserSkill
                .Include(u => u.Skill)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSkill == null)
            {
                // Yedi data vetiyena bhane 'Not Found' return garne
                return NotFound();
            }

            return View(userSkill);
        }

        // GET: UserSkill/Create
        // Naya UserSkill create garne form dekhaune
        public IActionResult Create()
        {
            // User ra Skill ko dropdown list banaune
            ViewData["SkillId"] = new SelectList(_context.Skill, "Id", "SkillName");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name");
            return View();
        }

        // POST: UserSkill/Create
        // UserSkill form submit bhayepachi data save garne method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,SkillId,CanTeach,WantToLearn")] UserSkill userSkill)
        {
            if (ModelState.IsValid)
            {
                // Data valid cha bhane database ma add garne
                _context.Add(userSkill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Data invalid bhaye feri tehi form dekhaune error sahit
            ViewData["SkillId"] = new SelectList(_context.Skill, "Id", "SkillName", userSkill.SkillId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", userSkill.UserId);
            return View(userSkill);
        }

        // GET: UserSkill/Edit/5
        // UserSkill edit garne form dekhaune
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserSkill == null)
            {
                return NotFound();
            }

            // Edit garna lako data database bata tanne
            var userSkill = await _context.UserSkill.FindAsync(id);
            if (userSkill == null)
            {
                return NotFound();
            }
            ViewData["SkillId"] = new SelectList(_context.Skill, "Id", "SkillName", userSkill.SkillId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", userSkill.UserId);
            return View(userSkill);
        }

        // POST: UserSkill/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,SkillId,CanTeach,WantToLearn")] UserSkill userSkill)
        {
            if (id != userSkill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userSkill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserSkillExists(userSkill.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SkillId"] = new SelectList(_context.Skill, "Id", "SkillName", userSkill.SkillId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", userSkill.UserId);
            return View(userSkill);
        }

        // GET: UserSkill/Delete/5
        // Delete garne confirmation page dekhaune
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserSkill == null)
            {
                return NotFound();
            }

            var userSkill = await _context.UserSkill
                .Include(u => u.Skill)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userSkill == null)
            {
                return NotFound();
            }

            return View(userSkill);
        }

        // POST: UserSkill/Delete/5
        // UserSkill lai database bata haraune (Delete confirmed)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserSkill == null)
            {
                return Problem("Entity set 'SkillSwapContext.UserSkill'  is null.");
            }
            var userSkill = await _context.UserSkill.FindAsync(id);
            if (userSkill != null)
            {
                // Data vetiyo bhane delete garne
                _context.UserSkill.Remove(userSkill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserSkillExists(int id)
        {
          return (_context.UserSkill?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
