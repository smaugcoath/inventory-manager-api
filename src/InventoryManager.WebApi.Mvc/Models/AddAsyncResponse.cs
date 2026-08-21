namespace InventoryManager.WebApi.Mvc.Models
{
    using InventoryManager.WebApi.Mvc.Models.Abstractions;
    using System;

    /// <summary>
    /// The item that was added to the inventory.
    /// </summary>
    public class AddAsyncResponse : BaseResponse<AddAsyncResponse.Item>
    {
        /// <summary>
        /// The inventory item carried by the response.
        /// </summary>
        public class Item
        {
            /// <summary>
            /// The name that identifies the item.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The instant, in UTC, at which the item expires.
            /// </summary>
            public DateTime ExpirationDate { get; set; }

            /// <summary>
            /// The category the item belongs to.
            /// </summary>
            public byte Type { get; set; }
        }
    }
}
