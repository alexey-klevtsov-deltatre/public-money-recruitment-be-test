using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public sealed class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAGetReturnsTheUpdatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync("/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await AddBookings(postResult.Id);

            request = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 3
            };
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
            }

            using var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);

            var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
            Assert.Equal(request.Units, getResult.Units);
            Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);

            using var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postResult.Id}&start=2002-01-01&nights=8");
            Assert.True(getCalendarResponse.IsSuccessStatusCode);
            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();

            var date = getCalendarResult.Dates[3];
            Assert.Single(date.PreparationTimes);
            date = getCalendarResult.Dates[4];
            Assert.Equal(2, date.PreparationTimes.Count);
            date = getCalendarResult.Dates[5];
            Assert.Equal(2, date.PreparationTimes.Count);
            date = getCalendarResult.Dates[6];
            Assert.Single(date.PreparationTimes);
            date = getCalendarResult.Dates[7];
            Assert.Empty(date.PreparationTimes);
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPostReturnsErrorWhenThereIsOverlapping()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync("/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await AddBookings(postResult.Id);

            request = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 4
            };

            await Assert.ThrowsAsync<RentalOverlappedException>(async () =>
            {
                using var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request);
            });

            request = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 1
            };

            await Assert.ThrowsAsync<RentalOverlappedException>(async () =>
            {
                using var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request);
            });
        }

        private async Task AddBookings(int rentalId)
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = 4,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = 6,
                Start = new DateTime(2002, 01, 07)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }
        }
    }
}
