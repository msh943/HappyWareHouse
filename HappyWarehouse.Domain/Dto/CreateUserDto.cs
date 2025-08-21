using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HappyWarehouse.Domain.Dto
{
    public class CreateUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required, RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
        ErrorMessage = "Password must be ≥ 8 chars, include uppercase, number, and symbol.")]
        public string Password { get; set; } = string.Empty;
        [Required]
        public int RoleId { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
