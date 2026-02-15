
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using skillSewa.Models;
using skillSewa.ViewModels;
using System.Security.Claims;

namespace skillSewa.Controllers
{
    public class AccountController : Controller
    {
        private readonly SkillSwapContext _context;

        public AccountController(SkillSwapContext context)
        {
            _context = context;
        }

        // GET: Register
        // User registration page dekhaune
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        // User le bhareko data receive garera database ma save garne
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Email pahilai cha ki chaina check garne
                if (await _context.User.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    return View(model);
                }

                // Password hash garne
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                // Naya user create garne
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = passwordHash,
                    Role = "User" // Default role
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Login page ma pathaune
                return RedirectToAction("Login");
            }

            return View(model);
        }

        // GET: Login
        // Login page dekhaune
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        // Login credentials verify garne ra session create garne
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // User database ma khojne
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);

                // User verify garne (password match huncha ki hudaina)
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // User ko claims create garne (id, naam, role store garna)
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Browser close garda pani login rahane
                        ExpiresUtc = DateTime.UtcNow.AddDays(7)
                    };

                    // Cookie sign-in garne
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Paila ko page ma return garne yadi cha bhane, natra Dashboard ma
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                // Login fail bhayo bhane error dekhaune
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // Logout action
        // Session clear garera home page ma pathaune
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Dashboard action
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login");
            }

            int userId = int.Parse(userIdString);
            var isAdmin = User.IsInRole("Admin");

            var model = new DashboardViewModel
            {
                IsAdmin = isAdmin,
                TotalSkillsOffered = await _context.UserSkill.CountAsync(us => us.UserId == userId && us.CanTeach),
                TotalSkillsLearned = await _context.UserSkill.CountAsync(us => us.UserId == userId && us.WantToLearn)
            };

            // Calculate potential matches: Find users who teach what I want to learn
            var myLearningSkills = await _context.UserSkill
                .Where(us => us.UserId == userId && us.WantToLearn)
                .Select(us => us.SkillId)
                .ToListAsync();

            if (myLearningSkills.Any())
            {
                model.PotentialMatches = await _context.UserSkill
                    .CountAsync(us => us.UserId != userId && us.CanTeach && myLearningSkills.Contains(us.SkillId));

                model.RecentMatches = await _context.UserSkill
                    .Include(us => us.User)
                    .Include(us => us.Skill)
                    .Where(us => us.UserId != userId && us.CanTeach && myLearningSkills.Contains(us.SkillId))
                    .OrderByDescending(us => us.Id)
                    .Take(5)
                    .ToListAsync();
            }

            if (isAdmin)
            {
                model.TotalUsers = await _context.User.CountAsync();
                model.TotalSkills = await _context.Skill.CountAsync();
            }

            return View(model);
        }

        // Access Denied page
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
