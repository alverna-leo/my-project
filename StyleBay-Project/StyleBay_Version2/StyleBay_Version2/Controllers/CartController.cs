using Domain.Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StyleBay_Version2.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= ADD TO CART =================
        [HttpPost]
        public IActionResult Add(Guid id) // DressId
        {
            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Login");

            Guid userId = Guid.Parse(userIdString);

            // Check if the product is already in cart
            bool exists = _context.CartItems
                .Any(c => c.UserId == userId && c.DressId == id);

            if (!exists)
            {
                // Add product to cart with quantity 1
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DressId = id,
                    Quantity = 1
                };

                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }

            // Do nothing if product is already in cart
            return RedirectToAction("Index");
        }

        // ================= CART PAGE =================
        public IActionResult Index()
        {
            string? userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
                return RedirectToAction("Login", "Login");

            Guid userId = Guid.Parse(userIdString);

            // Get all cart items with dress details
            var cartItems = _context.CartItems
                .Include(c => c.Dress)
                .Where(c => c.UserId == userId)
                .ToList();

            return View("Cart", cartItems);
        }

        // ================= REMOVE FROM CART =================
        [HttpPost]
        public IActionResult Remove(Guid id) // CartItem Id
        {
            var item = _context.CartItems.FirstOrDefault(c => c.Id == id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
