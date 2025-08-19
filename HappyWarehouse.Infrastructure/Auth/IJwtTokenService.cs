using HappyWarehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyWarehouse.Infrastructure.Auth
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user, string issuer, string audience, string key, int minutes = 120);
    }
}
