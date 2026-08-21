using System;
using System.Threading.Tasks;
using InventoryManager.WebApi.Business.Models;
using InventoryManager.WebApi.Business.Services.ItemService;
using InventoryManager.WebApi.Infrastructure.Services.Mediator.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InventoryManager.WebApi.Tests.Unit
{
    public class Mediator_should
    {
        [Fact]
        public async Task Dispatch_a_notification_to_the_handler_that_subscribes_to_it()
        {
            // The whole event-handler design rests on handlers being discovered from the
            // assemblies passed to the mediator registration, and on them resolving their
            // own scoped dependencies. Both are exercised here.
            var itemService = new ItemServiceSpy();

            var provider = new ServiceCollection()
                .AddMediatRMediatorService(new[] { typeof(ItemExpiredNotification).Assembly })
                .AddScoped<IItemService>(_ => itemService)
                .BuildServiceProvider(validateScopes: true);

            using (var scope = provider.CreateScope())
            {
                var mediatorService = scope.ServiceProvider.GetRequiredService<IMediatorService>();

                await mediatorService.Publish(new ItemExpiredNotification("expired-item"));
            }

            Assert.Equal("expired-item", itemService.DeletedName);
        }

        private sealed class ItemServiceSpy : IItemService
        {
            public string DeletedName { get; private set; }

            public Task<Item> DeleteAsync(string name)
            {
                DeletedName = name;

                return Task.FromResult(new Item { Name = name });
            }

            public Task<Item> AddAsync(Item item) => throw new NotSupportedException();

            public Task<bool> ExistsAsync(string name) => throw new NotSupportedException();

            public Task<Item> GetAsync(string name) => throw new NotSupportedException();
        }
    }
}
