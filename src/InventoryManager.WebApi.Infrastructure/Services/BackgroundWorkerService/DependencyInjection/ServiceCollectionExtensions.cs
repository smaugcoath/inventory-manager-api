namespace Microsoft.Extensions.DependencyInjection
{
    using InventoryManager.WebApi.Infrastructure.Services.BackgroundWorkerService.Abstractions;
    using InventoryManager.WebApi.Infrastructure.Services.BackgroundWorkerService.Hangfire;
    using Hangfire;
    using Hangfire.MemoryStorage;

    /// <summary>
    /// Extensions methods for <see cref="IServiceCollection"/> regarding different background workers providers.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Injects <see cref="IBackgroundWorkerService"/> to enqueue and schedule background work.
        /// The implementation is backed by Hangfire with in-memory storage, so the Hangfire
        /// dashboard can be exposed by mapping it in the MVC endpoint extension methods.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <returns>The instance of <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddHangFireInMemoryBackgroundWorker(this IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMemoryStorage());

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddSingleton<IBackgroundWorkerService, HangfireBackgroundWorkerService>();

            return services;
        }
    }
}
