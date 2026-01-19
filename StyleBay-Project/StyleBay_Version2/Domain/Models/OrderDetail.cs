using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public Guid DressId { get; set; }
        public Dress? Dress { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        // ✅ Dress name snapshot
        [Required]
        public string DressName { get; set; } = string.Empty;

        // ✅ Delivery address snapshot
        [Required]
        public string DeliveryAddress { get; set; } = string.Empty;

        // ✅ Payment method snapshot (COD / GPay)
        [Required]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
