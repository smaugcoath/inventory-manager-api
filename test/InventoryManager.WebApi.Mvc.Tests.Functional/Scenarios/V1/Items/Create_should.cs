namespace InventoryManager.WebApi.Mvc.Tests.Functional
{
    using FluentAssertions;
    using InventoryManager.WebApi.Business.Models;
    using InventoryManager.WebApi.Business.Services.ItemService;
    using InventoryManager.WebApi.Mvc.Controllers;
    using InventoryManager.WebApi.Mvc.Models;
    using InventoryManager.WebApi.Mvc.Tests.Functional.SeedWork;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.TestHost;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    [Collection(nameof(ServerCollectionFixture))]
    [ResetDatabase]
    public class Create_should
    {
        private const string ItemName = "ItemName";

        private readonly ServerFixture _serverFixture;

        public Create_should(ServerFixture serverFixture)
        {
            _serverFixture = serverFixture ?? throw new ArgumentNullException(nameof(serverFixture));
        }

        [Fact]
        public async Task Return_created_result_with_location_header()
        {
            var request = BuildRequest(ItemName);

            var response = await _serverFixture.Server.CreateHttpApiRequest<ItemsController>(c => c.AddAsync(request))
                .SendAsync(HttpMethods.Post);

            response.StatusCode.Should().Be(StatusCodes.Status201Created);
            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location.ToString().Should().EndWith($"api/items/{ItemName}");
        }

        [Fact]
        public async Task Return_conflict_if_name_is_duplicated()
        {
            var itemService = _serverFixture.GetService<IItemService>();
            await itemService.AddAsync(new Item { Name = ItemName, Type = default, ExpirationDate = default });

            var request = BuildRequest(ItemName);

            var response = await _serverFixture.Server.CreateHttpApiRequest<ItemsController>(c => c.AddAsync(request))
                .SendAsync(HttpMethods.Post);

            response.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        }

        private static AddAsyncRequest BuildRequest(string name)
            => new AddAsyncRequest
            {
                Data = new AddAsyncRequest.Item
                {
                    ExpirationDate = DateTime.UtcNow.AddMinutes(1),
                    Name = name,
                    Type = 1
                }
            };
    }
}
