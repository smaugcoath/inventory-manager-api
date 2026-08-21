namespace InventoryManager.WebApi.Mvc.Models
{
    using InventoryManager.WebApi.Mvc.Models.Abstractions;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The item to add to the inventory.
    /// </summary>
    public class AddAsyncRequest : BaseRequest<AddAsyncRequest.Item>
    {
        /// <summary>
        /// The inventory item carried by the request.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// The name that identifies the item. Unique across the inventory.
            /// </summary>
            [Required]
            [MinLength(1)]
            [MaxLength(100)]
            public string Name { get; set; }

            /// <summary>
            /// The instant, in UTC, at which the item expires and is removed from the inventory.
            /// </summary>
            public DateTime ExpirationDate { get; set; }

            /// <summary>
            /// The category the item belongs to.
            /// </summary>
            public byte Type { get; set; }
        }
    }
}
