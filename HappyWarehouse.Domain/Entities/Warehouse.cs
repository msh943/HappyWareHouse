using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        public int CountryId { get; set; }
        [Required]
        public Country Country { get; set; } = null!;

        public ICollection<WarehouseItem> Items { get; set; } = new List<WarehouseItem>();
    }
}

