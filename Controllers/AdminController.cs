using Domain.Data;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace StyleBay_Version2.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDressService _dressService;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly IPriceCategoryService _priceCategoryService;
        private readonly IDressCategoryService _dressCategoryService;
        //private readonly IOrderService _orderService;
        public AdminController(ApplicationDbContext context, IWebHostEnvironment env, IDressService dressService, IPriceCategoryService priceCategoryService, IDressCategoryService dressCategoryService)
        {
            _context = context;
            _env = env;
            _dressService = dressService;
            _priceCategoryService = priceCategoryService;
            _dressCategoryService = dressCategoryService;
            //_orderService = orderService;
        }
        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _context.Users.Count(u => u.Role == UserRole.User);
            ViewBag.TotalOrders = 0;//need to vhange


            return View();
        }
        public IActionResult Categories()
        {
            return View();0
        }
        public async Task<IActionResult> Index()
        {
            var dresses = await _dressService.GetAllDressesAsync();
            return View(dresses);
        }
        [HttpGet]
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dress dress, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(dress);
            }

            Stream? stream = null;
            string? fileName = null;

            if (imageFile != null)
            {
                stream = imageFile.OpenReadStream();
                fileName = imageFile.FileName;
            }
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("imageFile", "Dress image is required");
            }

            var result = await _dressService.CreateDressAsync(
                dress,
                stream,
                fileName,
                _env.WebRootPath);

            if (!result.Success)
            {

                ModelState.AddModelError(nameof(Dress.Price), result.ErrorMessage);
                LoadDropdowns();
                return View(dress);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var dress = await _dressService.GetByIdAsync(id); // optional if you add method
            if (dress == null)
                return NotFound();

            LoadDropdowns();
            return View(dress);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Dress dress, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(dress);
            }

            Stream? stream = null;
            string? fileName = null;

            if (imageFile != null)
            {
                stream = imageFile.OpenReadStream();
                fileName = imageFile.FileName;
            }

            var result = await _dressService.UpdateDressAsync(
                dress,
                stream,
                fileName,
                _env.WebRootPath);

            if (!result.Success)
            {
                
                ModelState.AddModelError(nameof(Dress.Price), result.ErrorMessage);
                LoadDropdowns();
                return View(dress);
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Delete/5
        
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _dressService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _dressService.DeleteAsync(id, _env.WebRootPath);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Create Price Category
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(PriceCategory priceCategory)
        {
            if (!ModelState.IsValid)
                return View(priceCategory);

            await _priceCategoryService.AddAsync(priceCategory);

            return RedirectToAction("ListPriceCategory");
        }
        // LIST ListPriceCategory
        public async Task<IActionResult> ListPriceCategory()
        {
            var categories = await _priceCategoryService.GetAllAsync();
            return View(categories);
        }

        // GET Price Category Edit
        public async Task<IActionResult> EditPriceCategory(Guid id)
        {
            var category = await _priceCategoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPriceCategory(Guid id, PriceCategory model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            await _priceCategoryService.UpdateAsync(model);

            return RedirectToAction("ListPriceCategory");
        }

        // DELETE - GET Price Category
        public async Task<IActionResult> DeletePriceCategory(Guid id)
        {
            var category = await _priceCategoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // DELETE - POST
        [HttpPost, ActionName("DeletePriceCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePriceCategoryConfirmed(Guid id)
        {
            await _priceCategoryService.DeleteAsync(id);
            return RedirectToAction("ListPriceCategory");
        }
        // LIST Dress Categories
        public async Task<IActionResult> ListDressCategory()
        {
            var categories = await _dressCategoryService.GetAllAsync();
            return View(categories);
        }

        // DELETE - GET DressCategory
        public async Task<IActionResult> DeleteDressCategory(Guid id)
        {
            var category = await _dressCategoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // DELETE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDressCategoryConfirmed(Guid id)
        {
            await _dressCategoryService.DeleteAsync(id);
            return RedirectToAction("ListDressCategory");
        }
        // CREATE Dress Category- GET
        [HttpGet]
        public IActionResult CreateDressCategory()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDressCategory(DressCategory category)
        {
            if (!ModelState.IsValid)
                return View(category);

            await _dressCategoryService.AddAsync(category);

            return RedirectToAction("ListDressCategory");
        }
        // LIST ORDERS WITH DETAILS

        public async Task<IActionResult> ListOrders()
        {
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Order)
                    .ThenInclude(o => o.User)
                .Include(od => od.Dress)
                .ToListAsync();

            return View(orderDetails);
        }

        // ================= HELPER =================
        private void LoadDropdowns()
        {
            ViewBag.DressCategories = new SelectList(
                _context.DressCategories,
                "Id",
                "Name");

            ViewBag.PriceCategories = new SelectList(
                _context.PriceCategories,
                "Id",
                "Name");
        }
        public static class EmailHelper
        {
            public static void SendEmail(string toEmail, string subject, string body)
            {
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("stylebay.noreply@gmail.com", "fnog drkg hxbb xzgt"),
                    EnableSsl = true,
                };

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("stylebay.noreply@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                smtp.Send(mail);
            }
        }
        [HttpGet]
        public IActionResult UserList()
        {
            var users = _context.Users
                .Where(u => u.Role == UserRole.User) // ✅ FILTER USERS ONLY
                .ToList();

            return View(users);
        }
        [HttpPost]

        public IActionResult DeleteUser(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                EmailHelper.SendEmail(
                    user.Email,
                    "Account Removed by Admin",
                    $"Hello {user.Name},<br/><br/>" +
                    "Your account has been removed by the administrator.<br/><br/>" +
                    "If this is a mistake, please contact support.<br/><br/>" +
                    "Regards,<br/>Admin Team"
                );

                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("UserList");
        }

       

    }
}


 


 