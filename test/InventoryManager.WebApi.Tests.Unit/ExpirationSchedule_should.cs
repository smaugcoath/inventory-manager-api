using System;
using InventoryManager.WebApi.Business.Services.ItemService;
using Xunit;

namespace InventoryManager.WebApi.Tests.Unit
{
    public class ExpirationSchedule_should
    {
        [Fact]
        public void Interpret_unspecified_expiration_date_as_utc()
        {
            // An ExpirationDate loaded through EF Core has Kind = Unspecified.
            // The scheduled instant must be the same clock value interpreted as UTC,
            // never shifted by the host machine's local offset.
            var expirationDate = new DateTime(2030, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

            var offset = ExpirationSchedule.ToScheduleOffset(expirationDate);

            Assert.Equal(TimeSpan.Zero, offset.Offset);
            Assert.Equal(expirationDate.Ticks, offset.UtcDateTime.Ticks);
            Assert.Equal(DateTimeKind.Utc, offset.UtcDateTime.Kind);
        }
    }
}
