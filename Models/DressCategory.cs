using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class DressCategory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(50)]
        public string? Name { get; set; }

        public virtual ICollection<Dress>? Dresses { get; set; }
    }
}