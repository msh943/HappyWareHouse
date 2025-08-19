using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Service.IService
{
    public interface IAuthService
    {
        Task<User?> ValidateUserAsync(string email, string password);
    }
}
