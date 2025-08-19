using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.IService
{
    public interface IWarehouseService
    {
        Task<(IEnumerable<Warehouse> items, int total)> GetAllAsync(int page, int pageSize);
        Task<Warehouse?> GetByIdAsync(int id);
        Task<Warehouse> CreateAsync(Warehouse warehouse);
        Task UpdateAsync(Warehouse warehouse);
        Task DeleteAsync(int id);
    }
}
