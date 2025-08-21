using HappyWarehouse.Domain.Dto;

namespace HappyWarehouse.Service.IService
{
    public interface ILookupsService
    {
        public Task<List<LookupDto>> GetRolesAsync();

        public Task<List<LookupDto>> GetCountriesAsync();
    }
}
