using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace HappyWarehouse.Service.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _users;

        public AuthService(IRepository<User> users)
        {
            _users = users;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _users.Query().Include(u => u.Role).FirstOrDefaultAsync(x => x.Email == email);
            if (user is null) return null;
            if (!user.IsActive) return user;
            var ok = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return ok ? user : null;
        }
    }
}
