using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Api.Controllers
{

    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _svc;

        public UsersController(IUserService svc) => _svc = svc;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (users, total) = await _svc.GetAllAsync(page, pageSize);
            return Ok(new
            {
                items = users.Select(u => new {
                    u.Id,
                    u.Email,
                    u.FullName,
                    u.IsActive,
                    Role = u.Role!.Name
                }),
                total
            });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public Task<User?> Get(int id) => _svc.GetByIdAsync(id);

        public record CreateUserRequest(string Email, string FullName, int RoleId, bool Active, string Password);

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
        {
            var user = new User { Email = req.Email, FullName = req.FullName, RoleId = req.RoleId, IsActive = req.Active };
            var created = await _svc.CreateAsync(user, req.Password);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new { created.Id, created.Email, created.FullName, created.IsActive, RoleId = created.RoleId });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            await _svc.UpdateAsync(user);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] string newPassword)
        {
            await _svc.ChangePasswordAsync(id, newPassword);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteAsync(id);
            return NoContent();
        }
    }
}
