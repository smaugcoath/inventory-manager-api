namespace InventoryManager.WebApi.Infrastructure.Services.Mediator.Abstractions
{
    using global::MediatR;

    /// <summary>
    /// Subscriber to a <typeparamref name="TNotification"/> message. It derives from the
    /// provider's handler interface so that implementations are discovered when the
    /// mediator registration scans the assemblies it is given.
    /// </summary>
    /// <typeparam name="TNotification">The message this handler subscribes to.</typeparam>
    public interface IMessageHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : IMessage
    {
    }
}
