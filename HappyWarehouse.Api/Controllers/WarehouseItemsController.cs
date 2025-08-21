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
    public class WarehouseItemsController : BaseApiController
    {
        private readonly IWarehouseItemService _svc;
        private readonly IMapper _mapper;
        public WarehouseItemsController(IWarehouseItemService svc, IMapper mapper) { _svc = svc; _mapper = mapper; }


        [HttpGet]
        public async Task<IActionResult> GetAll(int warehouseId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (items, total) = await _svc.GetAllAsync(warehouseId, page, pageSize);
            var warehouseItems = _mapper.Map<IEnumerable<WarehouseItemDto>>(items);
            var warehouses = warehouseItems
                            .GroupBy(i => new { i.WarehouseId, i.WarehouseName })
                            .Select(g => new {
                                id = g.Key.WarehouseId,
                                name = g.Key.WarehouseName,
                                warehouseItems = g.Select(i => new {
                                    i.Id,
                                    i.ItemName,
                                    i.SkuCode,
                                    i.Qty,
                                    i.CostPrice,
                                    i.MsrpPrice
                                }).ToList()
                            });
            return Ok(new { warehouses, total });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseItemDto>> Get(int id)
        {
            var item = await _svc.GetByIdAsync(id);
            if (item is null) return NotFound();
            return Ok(_mapper.Map<WarehouseItemDto>(item));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseItemDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = _mapper.Map<WarehouseItem>(dto);
            var created = await _svc.CreateAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, _mapper.Map<WarehouseItemDto>(created));
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWarehouseItemDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var item = await _svc.GetByIdAsync(id);
            if (item is null) return NotFound();

            _mapper.Map(dto, item);
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
