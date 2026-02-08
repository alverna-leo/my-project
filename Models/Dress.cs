using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Dress
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(50)]
        public string? Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? ImageName { get; set; }

        public Guid DressCategoryId { get; set; }
        public virtual DressCategory? DressCategory { get; set; }

        public Guid PriceCategoryId { get; set; }
        public virtual PriceCategory? PriceCategory { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<CartItem>? CartItems { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}