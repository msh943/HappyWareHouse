using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Domain.Dto
{
    public class CreateWarehouseItemDto
    {
        [Required]
        public string ItemName { get; set; } = string.Empty;

        public string? SkuCode { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Qty cannot be negative or zero.")]
        public int Qty { get; set; }
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Cost price cannot be negative or zero.")]

        public decimal CostPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "MSRP price cannot be negative.")]

        public decimal? MsrpPrice { get; set; }

        [Required]
        public int WarehouseId { get; set; }
    }
}
