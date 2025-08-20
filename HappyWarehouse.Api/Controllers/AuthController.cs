using HappyWarehouse.Infrastructure.Auth;
using HappyWarehouse.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HappyWarehouse.Api.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _auth;
        private readonly IJwtTokenService _jwt;
        private readonly IConfiguration _config;

        public AuthController(IAuthService auth, IJwtTokenService jwt, IConfiguration config)
        {
            _auth = auth;
            _jwt = jwt;
            _config = config;
        }

        public record LoginRequest(string Email, string Password);

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _auth.ValidateUserAsync(req.Email, req.Password);
            if (user is null)
            {
                Log.Information("Failed login for {Email}", req.Email);
                return Unauthorized(new { message = "Invalid email or password" });
            }
            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Your account is disabled. Please contact support." });
            }

            var token = _jwt.GenerateToken(user,
                _config["Jwt:Issuer"]!, _config["Jwt:Audience"]!, _config["Jwt:Key"]!, 120);
            return Ok(new { token, user = new { user.Id, user.Email, user.FullName, Role = user.Role!.Name } });
        }
    }
}
