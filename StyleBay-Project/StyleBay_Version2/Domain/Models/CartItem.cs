using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public Guid DressId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public User? User { get; set; }
        public Dress? Dress { get; set; }
    }
}