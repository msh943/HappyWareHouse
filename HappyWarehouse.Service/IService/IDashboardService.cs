using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.IService
{

    public record WarehouseStatusDto(int WarehouseId, string WarehouseName, int ItemCount);
    public interface IDashboardService
    {
        Task<IEnumerable<WarehouseStatusDto>> GetWarehouseStatusAsync();
        Task<IEnumerable<WarehouseItem>> GetTopHighItemsAsync(int top = 10);
        Task<IEnumerable<WarehouseItem>> GetTopLowItemsAsync(int top = 10);
    }
}
