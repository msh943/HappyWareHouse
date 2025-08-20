using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Domain.Dto
{
    public class UpdateWarehouseItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; } = string.Empty;

        public string? SkuCode { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }
        [Required]
        public decimal CostPrice { get; set; }
        public decimal? MsrpPrice { get; set; }

        [Required]
        public int WarehouseId { get; set; }
    }
}
