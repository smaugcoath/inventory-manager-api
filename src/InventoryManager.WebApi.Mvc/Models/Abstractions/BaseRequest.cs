namespace InventoryManager.WebApi.Mvc.Models.Abstractions
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Base request following the json:api conventions. <see href="https://jsonapi.org/"/>
    /// </summary>
    /// <typeparam name="TRequest">The payload carried by the request.</typeparam>
    public abstract class BaseRequest<TRequest> where TRequest : new()
    {
        /// <summary>
        /// The primary data of the request.
        /// </summary>
        [Required]
        public TRequest Data { get; set; } = new TRequest();
    }
}
