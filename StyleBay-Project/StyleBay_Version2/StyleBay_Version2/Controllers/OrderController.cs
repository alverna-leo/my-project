using Domain.Data;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static StyleBay_Version2.Controllers.AdminController;

namespace StyleBay_Version2.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= REVIEW (GET – BUY NOW) =================
        [HttpGet]
        public IActionResult Review(Guid dressId)
        {
            var dress = _context.Dresses.FirstOrDefault(d => d.Id == dressId);
            if (dress == null) return NotFound();

            ViewBag.Quantity = 1;

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            ViewBag.UserAddress = $"{user?.Address}, {user?.City}, {user?.State} - {user?.PinCode}";
            ViewBag.Phone = user?.PhoneNumber;

            return View(dress);
        }

        // ================= REVIEW (POST – CART / BUY NOW) =================
        [HttpPost]
        public IActionResult Review(Guid? cartItemId, Guid? dressId, int? quantity)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.UserAddress = $"{user?.Address}, {user?.City}, {user?.State} - {user?.PinCode}";
            ViewBag.Phone = user?.PhoneNumber;

            // FROM CART
            if (cartItemId.HasValue)
            {
                var cartItem = _context.CartItems
                    .Include(c => c.Dress)
                    .FirstOrDefault(c => c.Id == cartItemId.Value);

                if (cartItem == null) return NotFound();

                ViewBag.Quantity = cartItem.Quantity;
                ViewBag.CartItemId = cartItem.Id;

                return View(cartItem.Dress);
            }

            // FROM BUY NOW
            if (dressId.HasValue)
            {
                var dress = _context.Dresses.FirstOrDefault(d => d.Id == dressId.Value);
                if (dress == null) return NotFound();

                ViewBag.Quantity = quantity ?? 1;
                ViewBag.CartItemId = null;

                return View(dress);
            }

            return BadRequest();
        }

        // ================= PLACE ORDER =================
        [HttpPost]
        public IActionResult BuyNow(
            Guid id,
            int quantity,
            string addressType,
            string? streetArea,
            string? cityState,
            string? pinCode,
            string? phoneNumber,
            string paymentMethod,
            Guid? cartItemId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            if (quantity < 1) quantity = 1;

            var dress = _context.Dresses.FirstOrDefault(d => d.Id == id);
            if (dress == null) return NotFound();

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            string finalAddress;

            if (addressType == "new")
            {
                if (string.IsNullOrWhiteSpace(streetArea) ||
                    string.IsNullOrWhiteSpace(cityState) ||
                    string.IsNullOrWhiteSpace(pinCode) ||
                    string.IsNullOrWhiteSpace(phoneNumber))
                {
                    TempData["Error"] = "Please fill all address fields.";
                    return RedirectToAction("Review", new { dressId = id });
                }

                finalAddress = $"{streetArea}, {cityState} - {pinCode} Phone: {phoneNumber}";
            }
            else
            {
                finalAddress =
                    $"{user?.Address}, {user?.City}, {user?.State} - {user?.PinCode} Phone: {user?.PhoneNumber}";
            }

            // CREATE ORDER
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = dress.Price * quantity,
                DeliveryAddress = finalAddress,
                PaymentMethod = paymentMethod,
                DressName = dress.Name!
            };

            _context.Orders.Add(order);
            _context.SaveChanges();


            // mail

            var userr = _context.Users.FirstOrDefault(u => u.Id == userId);
            var orderUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (orderUser != null)
            {
                EmailHelper.SendEmail(
                    orderUser.Email,
                    "Order Confirmation",
                    $"Hello {orderUser.Name},<br/><br/>" +
                    $"Your order has been placed successfully.<br/><br/>" +
                    $"<b>Order ID:</b> {order.Id}<br/>" +
                    $"<b>Dress:</b> {dress.Name}<br/>" +
                    $"<b>Quantity:</b> {quantity}<br/>" +
                    $"<b>Total Amount:</b> ₹{order.TotalAmount}<br/>" +
                    $"<b>Payment Method:</b> {paymentMethod}<br/>" +
                    $"<b>Delivery Address:</b> {finalAddress}<br/><br/>" +
                    "Thank you for shopping with us!<br/><br/>" +
                    "Regards,<br/>Admin Team"
                );
            }

            //


            // ORDER DETAILS
            var detail = new OrderDetail
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                DressId = dress.Id,
                Quantity = quantity,
                Price = dress.Price,
                DressName = dress.Name!,
                DeliveryAddress = finalAddress,
                PaymentMethod = paymentMethod
            };

            _context.OrderDetails.Add(detail);

            var cartItem = _context.CartItems
                          .FirstOrDefault(c => c.UserId == userId && c.DressId == id);


            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }

            _context.SaveChanges();

            return RedirectToAction("Success", new { orderId = order.Id });

        }
        // ================= ORDER SUCCESS =================
        public IActionResult Success(Guid orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }


        // ================= SINGLE ORDER DETAILS =================
        public IActionResult OrderDetails(Guid orderId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            var order = _context.Orders
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Dress!)
                .FirstOrDefault(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // ================= MY ORDERS (HISTORY) =================
        public IActionResult MyOrders()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return RedirectToAction("Index", "Login");

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Dress!)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}
