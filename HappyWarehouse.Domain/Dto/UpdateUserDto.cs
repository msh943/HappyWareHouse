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
    public class UpdateUserDto
    {
        [Required]
        public int Id { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        [Required]
        public int RoleId { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public string? Password { get; set; }
    }
}
