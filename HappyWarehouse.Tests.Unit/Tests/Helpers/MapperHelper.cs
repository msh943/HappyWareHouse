

using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace HappyWarehouse.Tests.Unit.Tests.Helpers
{
    public static class MapperHelper
    {
        public static IMapper CreateMapper(params Type[] profileAssemblyMarkers)
        {
            var expr = new MapperConfigurationExpression();

            foreach (var t in profileAssemblyMarkers)
                expr.AddMaps(t.Assembly);

            var config = new MapperConfiguration(expr, NullLoggerFactory.Instance);

            return config.CreateMapper();
        }
    }
}
