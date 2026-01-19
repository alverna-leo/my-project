using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(50)]
        public string? Name { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required, StringLength(200)]
        public string? Address { get; set; }

        [Required, StringLength(50)]
        public string? City { get; set; }

        [Required, StringLength(50)]
        public string? State { get; set; }

        [Required, StringLength(6)]
        public string? PinCode { get; set; }

        [Required, StringLength(10)]
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<CartItem>? CartItems { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
