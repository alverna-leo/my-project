using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        // ✅ ADDED
        [Required]
        public string DressName { get; set; } = string.Empty;

        // ✅ KEEP
        [Required]
        public string DeliveryAddress { get; set; } = string.Empty;

        // ✅ KEEP (COD / GPay)
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
