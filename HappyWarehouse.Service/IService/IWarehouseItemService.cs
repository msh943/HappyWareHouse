using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.IService
{
    public interface IWarehouseItemService
    {
        Task<(IEnumerable<WarehouseItem> items, int total)> GetAllAsync(int warehouseId, int page, int pageSize);
        Task<WarehouseItem?> GetByIdAsync(int id);
        Task<WarehouseItem> CreateAsync(WarehouseItem warehouseItem);
        Task UpdateAsync(WarehouseItem warehouseItem);
        Task DeleteAsync(int id);
        Task<IEnumerable<Warehouse>> GetWarehousesAsync();
    }
}
