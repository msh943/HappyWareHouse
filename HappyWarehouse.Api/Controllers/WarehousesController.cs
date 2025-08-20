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
    public class WarehousesController : BaseApiController
    {
        private readonly IWarehouseService _svc;
        private readonly IMapper _mapper;
        public WarehousesController(IWarehouseService svc, IMapper mapper) { _svc = svc; _mapper = mapper; }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (warehouses, total) = await _svc.GetAllAsync(page, pageSize);
            return Ok(new 
            { 
                data = warehouses.Select(w=> new{
                    w.Id,
                    w.Name,
                    w.Address,
                    w.City,
                    Country = w.Country!.Name,
                })
                ,total });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseDto>> Get(int id) 
        {
            var warehouse = await _svc.GetByIdAsync(id);
            if (warehouse is null) return NotFound();
            return Ok(_mapper.Map<WarehouseDto>(warehouse));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Create([FromBody] CreateWarehouseDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = _mapper.Map<Warehouse>(dto);
            var created = await _svc.CreateAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, _mapper.Map<WarehouseDto>(created));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateWarehouseDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var warehouse = await _svc.GetByIdAsync(id);
            if (warehouse is null) return NotFound();
            _mapper.Map(dto, warehouse);
            await _svc.UpdateAsync(warehouse);
            return NoContent();
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "Admin")]
        public Task Delete(int id) => _svc.DeleteAsync(id);
    }
}
