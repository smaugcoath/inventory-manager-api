namespace InventoryManager.WebApi.Business.EventHandlers
{
    using InventoryManager.WebApi.Business.Services.ItemService;
    using InventoryManager.WebApi.Infrastructure.Services.Mediator.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ItemDeletedHandler : IMessageHandler<ItemDeletedNotification>
    {
        public Task Handle(ItemDeletedNotification notification, CancellationToken cancellationToken)
        {
            // The reaction to a deletion belongs to whatever the surrounding system needs:
            // call another service, send an email or a push notification, publish a message
            // onto a bus. Nothing is wired up in this sample, so the handler completes.
            return Task.CompletedTask;
        }
    }
}
