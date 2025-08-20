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
    public class UserDto
    {
        [Required]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        [Required]
        public string? RoleName { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
