namespace InventoryManager.WebApi.Mvc.Models
{
    using InventoryManager.WebApi.Mvc.Models.Abstractions;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The Request counterpart of <see cref="GetAsyncResponse"/>. The endpoint binds the name
    /// straight from the route, which MVC and Swagger handle without a dedicated model, so this
    /// type only records the shape the request would take once the endpoint takes more input.
    /// </summary>
    public class GetAsyncRequest : BaseRequest<GetAsyncRequest.Item>
    {
        /// <summary>
        /// The item to look up.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// The name that identifies the item.
            /// </summary>
            [Required]
            [MinLength(1)]
            [MaxLength(100)]
            public string Name { get; set; }
        }
    }
}
