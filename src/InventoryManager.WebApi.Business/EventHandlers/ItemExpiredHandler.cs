namespace InventoryManager.WebApi.Business.EventHandlers
{
    using InventoryManager.WebApi.Business.Services.ItemService;
    using InventoryManager.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ItemExpiredHandler : IMessageHandler<ItemExpiredNotification>
    {
        private readonly IItemService _itemService;

        // Public so the container can activate the handler; the type itself stays internal.
        public ItemExpiredHandler(IItemService itemService)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
        }

        public async Task Handle(ItemExpiredNotification notification, CancellationToken cancellationToken)
        {
            // For instance, the action to be done here could be to delete the item from the inventory.
            await _itemService.DeleteAsync(notification.Name);

            // Call another service to process the notification. For example: send an email, publish a message onto a bus, etc.
        }
    }
}
