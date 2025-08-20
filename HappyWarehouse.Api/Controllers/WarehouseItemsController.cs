using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Api.Controllers
{
    [Authorize]
    public class WarehouseItemsController : BaseApiController
    {
        private readonly IWarehouseItemService _svc;
        public WarehouseItemsController(IWarehouseItemService svc) { _svc = svc; }

        [HttpGet]
        public async Task<IActionResult> GetAll(int warehouseId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (items, total) = await _svc.GetAllAsync(warehouseId, page, pageSize);
            return Ok(new { items, total });
        }

        [HttpGet("{id}")]
        public Task<WarehouseItem?> Get(int id) => _svc.GetByIdAsync(id);

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public Task<WarehouseItem> Create([FromBody] WarehouseItem item) => _svc.CreateAsync(item);

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Update([FromBody] WarehouseItem item)
        {
            await _svc.UpdateAsync(item);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public Task Delete(int id) => _svc.DeleteAsync(id);

        [HttpGet]
        public async Task<IEnumerable<Warehouse>> GetWarehouses() => await _svc.GetWarehousesAsync();
    }
}
