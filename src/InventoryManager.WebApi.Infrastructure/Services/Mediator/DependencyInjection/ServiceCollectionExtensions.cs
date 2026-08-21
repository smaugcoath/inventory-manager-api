namespace Microsoft.Extensions.DependencyInjection
{
    using InventoryManager.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using InventoryManager.WebApi.Infrastructure.Services.Mediator.MediatR;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions methods for <see cref="IServiceCollection"/> regarding different mediator providers.
    /// </summary>
    public static class MediatorServiceCollectionExtensions
    {
        /// <summary>
        /// Injects <see cref="IMediatorService"/> using MediatR as mediator provider.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="assemblies">The assemblies scanned for messages and their handlers. Defaults to the assemblies already loaded in the current application domain.</param>
        /// <returns>The instance of <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddMediatRMediatorService(this IServiceCollection services, IEnumerable<Assembly> assemblies = null)
        {
            assemblies = assemblies ?? AppDomain.CurrentDomain.GetAssemblies().AsEnumerable();

            services.AddMediatR(assemblies.ToArray());

            // Scoped, not singleton: handlers take the same scoped dependencies as the
            // service that published the message, and a singleton mediator would have to
            // resolve them from the root container.
            services.AddScoped<IMediatorService, MediatRMediatorService>();

            return services;
        }
    }
}
