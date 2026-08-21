namespace Microsoft.Extensions.DependencyInjection
{
    using InventoryManager.WebApi.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Data;

    /// <summary>
    /// Extension methods that register the inventory data-access services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the <see cref="DatabaseContext"/> with SQL Server as the persistence technology.
        /// A different store is selected by adding another extension method next to this one.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="connection">The connection that identifies the persistence store.</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDataServices(this IServiceCollection services, IDbConnection connection)
            => services
            .AddDatabaseContext(connection);

        /// <summary>
        /// Registers the context and puts the schema in place.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="connection">The connection that identifies the persistence store.</param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IDbConnection connection)
        {
            services
                  .AddDbContext<DatabaseContext>(options =>
                   options.UseSqlServer(
                       connection.ConnectionString,
                       sqlOptions => sqlOptions
                          .MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)
                          .EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)
                      ));

            RecreateDatabase(services);

            return services;
        }

        /// <summary>
        /// Drops and recreates the schema so that every run starts from the same known state,
        /// which is what makes the sample convenient to run from a clean container. A real
        /// deployment applies migrations instead, as part of the release.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        private static void RecreateDatabase(IServiceCollection services)
        {
            using (var provider = services.BuildServiceProvider())
            {
                var context = provider.GetRequiredService<DatabaseContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}
