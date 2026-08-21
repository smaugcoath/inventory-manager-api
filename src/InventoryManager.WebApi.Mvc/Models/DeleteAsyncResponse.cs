namespace InventoryManager.WebApi.Mvc.Models
{
    using InventoryManager.WebApi.Mvc.Models.Abstractions;
    using System;

    /// <summary>
    /// The item that was removed from the inventory.
    /// </summary>
    public class DeleteAsyncResponse : BaseResponse<DeleteAsyncResponse.Item>
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
            /// The instant, in UTC, at which the item expired.
            /// </summary>
            public DateTime ExpirationDate { get; set; }

            /// <summary>
            /// The category the item belongs to.
            /// </summary>
            public byte Type { get; set; }
        }
    }
}
