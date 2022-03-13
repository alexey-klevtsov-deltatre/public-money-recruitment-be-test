using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Core.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class GetCalendarTests
    {
        private readonly HttpClient _client;

        public GetCalendarTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenGetCalendar_ThenAGetReturnsTheCalculatedCalendar()
        {
            var postRentalRequest = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync("/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 2,
                 Start = new DateTime(2000, 01, 02)
            };

            ResourceIdViewModel postBooking1Result;
            using (var postBooking1Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
                postBooking1Result = await postBooking1Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 2,
                Start = new DateTime(2000, 01, 03)
            };

            ResourceIdViewModel postBooking2Result;
            using (var postBooking2Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
                postBooking2Result = await postBooking2Response.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postRentalResult.Id}&start=2000-01-01&nights=7");
            Assert.True(getCalendarResponse.IsSuccessStatusCode);

            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
                
            Assert.Equal(postRentalResult.Id, getCalendarResult.RentalId);
            Assert.Equal(7, getCalendarResult.Dates.Count);

            var date = getCalendarResult.Dates[0];
            Assert.Equal(new DateTime(2000, 01, 01), date.Date);
            Assert.Empty(date.Bookings);

            date = getCalendarResult.Dates[1];
            Assert.Equal(new DateTime(2000, 01, 02), date.Date);
            Assert.Single(date.Bookings);
            Assert.Contains(date.Bookings, x => x.Id == postBooking1Result.Id && x.Unit == 1);
            Assert.Empty(date.PreparationTimes);

            date = getCalendarResult.Dates[2];
            Assert.Equal(new DateTime(2000, 01, 03), date.Date);
            Assert.Equal(2, date.Bookings.Count);
            Assert.Contains(date.Bookings, x => x.Id == postBooking1Result.Id && x.Unit == 1);
            Assert.Contains(date.Bookings, x => x.Id == postBooking2Result.Id && x.Unit == 2);
            Assert.Empty(date.PreparationTimes);

            date = getCalendarResult.Dates[3];
            Assert.Equal(new DateTime(2000, 01, 04), date.Date);
            Assert.Single(date.Bookings);
            Assert.Contains(date.Bookings, x => x.Id == postBooking2Result.Id && x.Unit == 2);
            Assert.Single(date.PreparationTimes);
            Assert.Contains(date.PreparationTimes, x => x.Unit == 1);

            date = getCalendarResult.Dates[4];
            Assert.Equal(new DateTime(2000, 01, 05), date.Date);
            Assert.Empty(date.Bookings);
            Assert.Equal(2, date.PreparationTimes.Count);
            Assert.Contains(date.PreparationTimes, x => x.Unit == 1);
            Assert.Contains(date.PreparationTimes, x => x.Unit == 2);

            date = getCalendarResult.Dates[5];
            Assert.Equal(new DateTime(2000, 01, 06), date.Date);
            Assert.Empty(date.Bookings);
            Assert.Single(date.PreparationTimes);
            Assert.Contains(date.PreparationTimes, x => x.Unit == 2);

            date = getCalendarResult.Dates[6];
            Assert.Equal(new DateTime(2000, 01, 07), date.Date);
            Assert.Empty(date.Bookings);
            Assert.Empty(date.PreparationTimes);
        }
    }
}
