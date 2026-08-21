namespace InventoryManager.WebApi.Business.Services.ItemService
{
    using System;

    /// <summary>
    /// Helpers to translate an item's expiration date into the absolute instant
    /// used to schedule its background expiration.
    /// </summary>
    internal static class ExpirationSchedule
    {
        /// <summary>
        /// Builds the <see cref="DateTimeOffset"/> at which an item expires.
        /// The stored <see cref="DateTime"/> has an unspecified <see cref="DateTimeKind"/>
        /// after an EF Core round-trip, so it is treated as UTC to avoid the offset of the
        /// host machine leaking into the scheduled instant.
        /// </summary>
        /// <param name="expirationDate">The item's expiration date.</param>
        /// <returns>The expiration instant expressed as a UTC <see cref="DateTimeOffset"/>.</returns>
        internal static DateTimeOffset ToScheduleOffset(DateTime expirationDate)
            => new DateTimeOffset(DateTime.SpecifyKind(expirationDate, DateTimeKind.Utc));
    }
}
