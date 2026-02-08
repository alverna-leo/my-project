using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }
        public Order? Order { get; set; }

        public Guid DressId { get; set; }
        public Dress? Dress { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}