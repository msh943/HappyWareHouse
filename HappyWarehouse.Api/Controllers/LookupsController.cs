using HappyWarehouse.Domain.Dto;
using HappyWarehouse.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyWarehouse.Api.Controllers
{

    public class LookupsController : BaseApiController
    {
        private readonly ILookupsService _svc;
        public LookupsController(ILookupsService svc) => _svc = svc;

        [HttpGet]
        public Task<List<LookupDto>> Roles() => _svc.GetRolesAsync();

        [HttpGet]
        public Task<List<LookupDto>> Countries() => _svc.GetCountriesAsync();
    }
}
