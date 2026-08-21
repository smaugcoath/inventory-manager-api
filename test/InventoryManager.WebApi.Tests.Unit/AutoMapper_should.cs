using AutoMapper;
using Xunit;

namespace InventoryManager.WebApi.Tests.Unit
{
    public class AutoMapper_should
    {
        [Fact]
        public void Configure_valid_mappers()
        {
            // Both profiles are loaded into a single configuration, exactly as the
            // application does through its two AddAutoMapper registrations.
            var configuration = new MapperConfiguration(x =>
            {
                x.AddProfile<Business.Mappers.ItemProfile>();
                x.AddProfile<Mvc.Mappers.ItemProfile>();
            });

            configuration.AssertConfigurationIsValid();
        }
    }
}
