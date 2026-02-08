using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using skillSewa.Models;

namespace skillSewa.Controllers
{
    public class SkillController : Controller
    {
        private readonly SkillSwapContext _context;

        public SkillController(SkillSwapContext context)
        {
            _context = context;
        }

        // GET: Skill
        // Sabai skill haru list garne
        public async Task<IActionResult> Index()
        {
              return _context.Skill != null ? 
                          View(await _context.Skill.ToListAsync()) :
                          Problem("Entity set 'SkillSwapContext.Skill'  is null.");
        }

        // GET: Skill/Details/5
        // Skill ko details herne method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Skill == null)
            {
                return NotFound();
            }

            var skill = await _context.Skill
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        // GET: Skill/Create
        // Naya Skill create garne form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Skill/Create
        // Skill save garne logic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SkillName")] Skill skill)
        {
            if (ModelState.IsValid)
            {
                // Skill database ma add garne
                _context.Add(skill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(skill);
        }

        // GET: Skill/Edit/5
        // Skill edit garne form dekhaune
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Skill == null)
            {
                return NotFound();
            }

            var skill = await _context.Skill.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            return View(skill);
        }

        // POST: Skill/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SkillName")] Skill skill)
        {
            if (id != skill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(skill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkillExists(skill.Id))
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
            return View(skill);
        }

        // GET: Skill/Delete/5
        // Skill delete garne confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Skill == null)
            {
                return NotFound();
            }

            var skill = await _context.Skill
                .FirstOrDefaultAsync(m => m.Id == id);
            if (skill == null)
            {
                return NotFound();
            }

            return View(skill);
        }

        // POST: Skill/Delete/5
        // Skill lai officially delete garne
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Skill == null)
            {
                return Problem("Entity set 'SkillSwapContext.Skill'  is null.");
            }
            var skill = await _context.Skill.FindAsync(id);
            if (skill != null)
            {
                _context.Skill.Remove(skill);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SkillExists(int id)
        {
          return (_context.Skill?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
