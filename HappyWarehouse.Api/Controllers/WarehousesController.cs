using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Api.Controllers
{
    [Authorize]
    public class WarehousesController : BaseApiController
    {
        private readonly IWarehouseService _svc;
        public WarehousesController(IWarehouseService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (items, total) = await _svc.GetAllAsync(page, pageSize);
            return Ok(new { items, total });
        }

        [HttpGet("{id}")]
        public Task<Warehouse?> Get(int id) => _svc.GetByIdAsync(id);

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public Task<Warehouse> Create([FromBody] Warehouse warehouse) => _svc.CreateAsync(warehouse);

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Update([FromBody] Warehouse warehouse)
        {
            await _svc.UpdateAsync(warehouse);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public Task Delete(int id) => _svc.DeleteAsync(id);
    }
}
