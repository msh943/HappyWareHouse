using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HappyWarehouse.Domain.Entities
{
    public class WarehouseItem : BaseEntity
    {
        [Required]
        public string ItemName { get; set; } = string.Empty;

        public string? SkuCode { get; set; }

        [Range(1, int.MaxValue)]
        public int Qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MsrpPrice { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; } = null!;
    }
}
