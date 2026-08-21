namespace InventoryManager.WebApi.Mvc.Models.Abstractions
{
    /// <summary>
    /// Base response following the json:api conventions. <see href="https://jsonapi.org/"/>
    /// </summary>
    /// <typeparam name="TResponse">The payload carried by the response.</typeparam>
    public abstract class BaseResponse<TResponse> where TResponse : new()
    {
        /// <summary>
        /// The primary data of the response.
        /// </summary>
        public TResponse Data { get; set; } = new TResponse();
    }
}
