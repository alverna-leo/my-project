using Domain.Data;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StyleBay_Version2.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }

        // ================= HOME / PRODUCT LIST =================
        public IActionResult Index(string search, Guid? categoryId, Guid? priceCategoryId)
        {
            var query = _context.Dresses
                .Include(d => d.DressCategory)
                .Include(d => d.PriceCategory)
                .AsQueryable();

            // 🔍 Search (UNCHANGED)
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(d =>
                    (d.Name != null && d.Name.ToLower().Contains(lower)) ||
                    (d.Description != null && d.Description.ToLower().Contains(lower)));
            }

            // 🎯 Category filter
            if (categoryId.HasValue)
                query = query.Where(d => d.DressCategoryId == categoryId);

            // 💰 Price filter
            if (priceCategoryId.HasValue)
                query = query.Where(d => d.PriceCategoryId == priceCategoryId);

            // Dropdown data
            ViewBag.DressCategories = _context.DressCategories.ToList();
            ViewBag.PriceCategories = _context.PriceCategories.ToList();

            return View(query.ToList());
        }

        // ================= PROFILE =================
        public IActionResult Profile()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound();

            return View(user);
        }

        // ==================== EDIT PROFILE (GET) ====================
        [HttpGet]
        public IActionResult EditProfile()
        {
            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Login");

            Guid userId = Guid.Parse(userIdString);

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            return View(user);
        }
        // ================= EDIT PROFILE =================
        [HttpPost]
        public IActionResult EditProfile(User user)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            var existingUser = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (existingUser == null) return NotFound();

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                existingUser.PasswordHash =
                    _passwordHasher.HashPassword(existingUser, user.PasswordHash);
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        // ================= LOGOUT =================
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
