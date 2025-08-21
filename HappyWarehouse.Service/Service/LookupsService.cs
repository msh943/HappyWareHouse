using HappyWarehouse.Domain.Dto;
using HappyWarehouse.Domain.Entities;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Service.IService;

namespace HappyWarehouse.Service.Service
{
    public class LookupsService : ILookupsService
    {
        private readonly IRepository<Role> _roles;
        private readonly IRepository<Country> _countries;

        public LookupsService(IRepository<Role> roles, IRepository<Country> countries)
        {
            _roles = roles;
            _countries = countries;
        }

        public Task<List<LookupDto>> GetRolesAsync() =>
            _roles.GetAllLookupsAsync(r => new LookupDto(r.Id, r.Name));

        public Task<List<LookupDto>> GetCountriesAsync() =>
            _countries.GetAllLookupsAsync(c => new LookupDto(c.Id, c.Name));
    }
}
