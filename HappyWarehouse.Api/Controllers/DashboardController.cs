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
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _svc;
        private readonly IMapper _mapper;
        public DashboardController(IDashboardService svc, IMapper mapper) { _svc = svc; _mapper = mapper; }

        [HttpGet]
        public Task<IEnumerable<WarehouseStatusDto>> GetStatus() => _svc.GetWarehouseStatusAsync();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> HighItems([FromQuery] int top = 10)
        {
            var entities = await _svc.GetTopHighItemsAsync(top);
            var dto = _mapper.Map<IEnumerable<WarehouseItemDto>>(entities);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarehouseItemDto>>> LowItems([FromQuery] int top = 10)
        {
            var entities = await _svc.GetTopLowItemsAsync(top);
            var dto = _mapper.Map<IEnumerable<WarehouseItemDto>>(entities);
            return Ok(dto);
        }
    }
}
