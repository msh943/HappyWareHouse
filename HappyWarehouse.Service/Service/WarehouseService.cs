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
    public class WarehouseService : IWarehouseService
    {
        private readonly IRepository<Warehouse> _repo;
        public WarehouseService(IRepository<Warehouse> repo) { _repo = repo; }

        public async Task<(IEnumerable<Warehouse> items, int total)> GetAllAsync(int page, int pageSize)
        {
            var total = await _repo.CountAsync();
            var items = await _repo.GetAllAsync(page: page, pageSize: pageSize, includes: x => x.Country);
            return (items, total);
        }

        public Task<Warehouse?> GetByIdAsync(int id) => _repo.GetByIdAsync(id, x => x.Country, x => x.Items);

        public Task<Warehouse> CreateAsync(Warehouse wh) => _repo.AddAsync(wh);

        public Task UpdateAsync(Warehouse wh) => _repo.UpdateAsync(wh);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
