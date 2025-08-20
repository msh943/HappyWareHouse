using AutoMapper;
using HappyWarehouse.Domain.Dto;
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
        private readonly IMapper _mapper;

        public UsersController(IUserService svc, IMapper mapper)
        {
            _svc = svc;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (users, total) = await _svc.GetAllAsync(page, pageSize);
            return Ok(new
            {
                data = users.Select(u => new {
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
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _svc.GetByIdAsync(id);
            if (user is null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }

       

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = _mapper.Map<User>(dto);
            var created = await _svc.CreateAsync(entity, dto.Password);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, _mapper.Map<UserDto>(created));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateUserDto dto)
        {
            var entity = await _svc.GetByIdAsync(id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _svc.UpdateAsync(entity);
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
