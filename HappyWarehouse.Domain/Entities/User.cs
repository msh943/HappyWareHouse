using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HappyWarehouse.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string FullName { get; set; } = string.Empty;
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        [Required]
        public Role? Role { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
    }
}
