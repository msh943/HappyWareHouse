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
        public DashboardController(IDashboardService svc) { _svc = svc; }

        [HttpGet]
        public Task<IEnumerable<WarehouseStatusDto>> GetStatus() => _svc.GetWarehouseStatusAsync();

        [HttpGet]
        public Task<IEnumerable<WarehouseItem>> HighItems() => _svc.GetTopHighItemsAsync(10);

        [HttpGet]
        public Task<IEnumerable<WarehouseItem>> LowItems() => _svc.GetTopLowItemsAsync(10);
    }
}
