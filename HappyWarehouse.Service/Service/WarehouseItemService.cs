using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.Service
{
    public class WarehouseItemService : IWarehouseItemService
    {
        private readonly IRepository<WarehouseItem> _items;
        private readonly IRepository<Warehouse> _warehouses;

        public WarehouseItemService(IRepository<WarehouseItem> items, IRepository<Warehouse> warehouses)
        {
            _items = items;
            _warehouses = warehouses;
        }

        public async Task<(IEnumerable<WarehouseItem> items, int total)> GetAllAsync(int warehouseId, int page, int pageSize)
        {
            var total = await _items.CountAsync(x => x.WarehouseId == warehouseId);
            var items = await _items.GetAllAsync(x => x.WarehouseId == warehouseId, page: page, pageSize: pageSize, includes: x => x.Warehouse);
            return (items, total);
        }

        public Task<WarehouseItem?> GetByIdAsync(int id) => _items.GetByIdAsync(id, x => x.Warehouse);

        public Task<WarehouseItem> CreateAsync(WarehouseItem item) => _items.AddAsync(item);

        public Task UpdateAsync(WarehouseItem item) => _items.UpdateAsync(item);

        public Task DeleteAsync(int id) => _items.DeleteAsync(id);

        public Task<IEnumerable<Warehouse>> GetWarehousesAsync() => _warehouses.GetAllAsync();
    }
}
