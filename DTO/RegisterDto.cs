using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class RegisterDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

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
    }
}
