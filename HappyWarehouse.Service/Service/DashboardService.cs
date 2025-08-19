using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<Warehouse> _whRepo;
        private readonly IRepository<WarehouseItem> _itemRepo;
        public DashboardService(IRepository<Warehouse> whRepo, IRepository<WarehouseItem> itemRepo)
        {
            _whRepo = whRepo;
            _itemRepo = itemRepo;
        }

        public async Task<IEnumerable<WarehouseStatusDto>> GetWarehouseStatusAsync()
        {
            var data = await _whRepo.Query()
                .Include(x => x.Items)
                .Select(x => new WarehouseStatusDto(x.Id, x.Name, x.Items.Sum(i => i.Qty)))
                .ToListAsync();
            return data;
        }

        public async Task<IEnumerable<WarehouseItem>> GetTopHighItemsAsync(int top = 10)
        {
            return await _itemRepo.Query().OrderByDescending(x => x.Qty).Take(top).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<WarehouseItem>> GetTopLowItemsAsync(int top = 10)
        {
            return await _itemRepo.Query().OrderBy(x => x.Qty).Take(top).AsNoTracking().ToListAsync();
        }
    }
}
