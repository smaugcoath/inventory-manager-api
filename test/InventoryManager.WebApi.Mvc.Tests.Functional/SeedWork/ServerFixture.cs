namespace InventoryManager.WebApi.Mvc.Tests.Functional.SeedWork
{
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;
    using System;

    /// <summary>
    /// Owns the in-process server shared by every test in the collection, and the Respawn
    /// checkpoint used to bring the database back to a known state between tests.
    /// </summary>
    public class ServerFixture : IDisposable
    {
        private static readonly Checkpoint _checkpoint = new Checkpoint
        {
            TablesToIgnore = new string[] { "__EFMigrationsHistory" },
            WithReseed = true
        };

        private const string AppSettingsFileName = "appsettings.json";
        private const string ConnectionStringName = "Default";

        private readonly IServiceScope _scope;

        /// <summary>
        /// The server under test.
        /// </summary>
        public TestServer Server { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerFixture"/> class.
        /// </summary>
        public ServerFixture()
        {
            Server = TestServerFactory.Create<TestStartup>(null, BuildConfiguration());
            _scope = Server.Services.CreateScope();
        }

        /// <summary>
        /// Resolves a service from the scope that lives as long as the fixture. The scope has to
        /// outlive the call: a scoped service handed back from an already-disposed scope carries
        /// a disposed database context with it.
        /// </summary>
        /// <typeparam name="TService">The service to resolve.</typeparam>
        /// <returns>The resolved service.</returns>
        public TService GetService<TService>()
            => _scope.ServiceProvider.GetService<TService>();

        /// <summary>
        /// Releases the scope and the server once the test collection is finished.
        /// </summary>
        public void Dispose()
        {
            _scope.Dispose();
            Server.Dispose();
        }

        internal static void ResetDatabase()
        {
            var configuration = BuildConfiguration();

            _checkpoint
                .Reset(configuration.GetConnectionString(ConnectionStringName))
                .GetAwaiter()
                .GetResult();
        }

        private static IConfiguration BuildConfiguration()
            => new ConfigurationBuilder()
            .AddJsonFile(AppSettingsFileName)
            .AddEnvironmentVariables()
            .Build();
    }
}
