namespace Microsoft.Extensions.DependencyInjection
{
    using AutoMapper;
    using InventoryManager.WebApi.Business.Services.ItemService;
    using System.Data;

    /// <summary>
    /// Extension methods that register the inventory business services.
    /// </summary>
    public static class BusinessServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the business services, their mappers, and the data-access layer they sit on.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="dbConnection">The connection that identifies the persistence store.</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IDbConnection dbConnection)
            => services
            .AddDataServices(dbConnection)
            .AddMappers()
            .AddServices();

        /// <summary>
        /// Registers the business services themselves.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddServices(this IServiceCollection services)
            => services
            .AddScoped<IItemService, ItemService>();

        /// <summary>
        /// Registers the AutoMapper profiles declared in this assembly.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddMappers(this IServiceCollection services)
            => services.AddAutoMapper(new[] { typeof(BusinessServiceCollectionExtensions) });

    }
}
