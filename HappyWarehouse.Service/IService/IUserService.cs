using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.IService
{
    public interface IUserService
    {
        Task<(IEnumerable<User> users, int total)> GetAllAsync(int page, int pageSize);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user, string Password);
        Task UpdateAsync(User user);
        Task ChangePasswordAsync(int id, string Password);
        Task DeleteAsync(int id);
    }
}
