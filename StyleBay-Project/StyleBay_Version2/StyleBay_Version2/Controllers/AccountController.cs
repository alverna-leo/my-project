using AutoMapper;
using Domain.Data;
using Domain.DTO;
using Domain.Enums;
using Domain.Helper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static StyleBay_Version2.Controllers.AdminController;

namespace StyleBay_Version2.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ================= USER LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1️⃣ Find user by email
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            // 2️⃣ Check user & password
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            // 🚫 BLOCK ADMIN LOGIN HERE
            if (user.Role == UserRole.Admin)
            {
                ModelState.AddModelError("", "Admins must log in from the admin portal");
                return View(model);
            }

            // 3️⃣ Save session (USER ONLY)
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Role", user.Role.ToString());

            // 4️⃣ Redirect user
            return RedirectToAction("Index", "User");
        }

        
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AdminLogin(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = _context.Users
                .FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());

            if (admin == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash);

            if (!isValidPassword)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // ✅ Check Role
            if (admin.Role != UserRole.Admin) // or "Admin" if string
            {
                ModelState.AddModelError("", "Invalid email");
                return View(model);
            }

            // Save session
            HttpContext.Session.SetString("UserId", admin.Id.ToString());
            HttpContext.Session.SetString("Role", "Admin");

            return RedirectToAction("Dashboard", "Admin");
        }
        //user Registration
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 🔒 Block duplicate emails (Admin or User)
            if (_context.Users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(model);
            }

            // ✅ AutoMapper used
            var user = _mapper.Map<User>(model);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.Role = UserRole.User;

            _context.Users.Add(user);
            _context.SaveChanges();



            //mail
            EmailHelper.SendEmail(
        user.Email,
        "Welcome to Our Application",
        $"Hello {user.Name},<br/><br/>" +
        "Your account has been successfully created.<br/><br/>" +
        "You can now log in using your registered email.<br/><br/>" +
        "Regards,<br/>Admin Team"
    );


            // ✅ ADD SESSION HERE (AFTER SAVE)
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("Role", user.Role.ToString());

            // ✅ Redirect user after registration
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

