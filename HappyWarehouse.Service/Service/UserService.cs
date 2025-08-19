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
    public class UserService : IUserService
    {
        private readonly IRepository<User> _users;

        public UserService(IRepository<User> users)
        {
            _users = users;
        }

        public async Task<(IEnumerable<User> users, int total)> GetAllAsync(int page, int pageSize)
        {
            var total = await _users.CountAsync();
            var items = await _users.GetAllAsync(page: page, pageSize: pageSize, includes: x => x.Role!);
            return (items, total);
        }

        public Task<User?> GetByIdAsync(int id) => _users.GetByIdAsync(id, x => x.Role!);

        public async Task<User> CreateAsync(User user, string plainPassword)
        {
            var exists = await _users.ExistsAsync(x => x.Email == user.Email);
            if (exists) throw new InvalidOperationException("Email already exists");
            user.Password = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            return await _users.AddAsync(user);
        }

        public Task UpdateAsync(User user) => _users.UpdateAsync(user);

        public async Task ChangePasswordAsync(int id, string newPassword)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");

            if(string.IsNullOrEmpty(newPassword))
                throw new ArgumentException(nameof(newPassword), "New password cannot be empty or null.");
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException(nameof(newPassword), "New password cannot be empty or space.");
            var user = await _users.GetByIdAsync(id);
            if (user is null) throw new InvalidOperationException("User not found");
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _users.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "ID must be greater than zero.");

            var user = await _users.GetByIdAsync(id, x => x.Role!);
            if (user is null) return;
            if (user.Role?.Name == "Admin" && user.Email == "admin@happywarehouse.com")
                throw new InvalidOperationException("Default admin user cannot be deleted.");
            await _users.DeleteAsync(id);
        }
    }
}
