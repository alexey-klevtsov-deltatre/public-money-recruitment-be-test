using Microsoft.AspNetCore.Mvc;
using VacationRental.Core.Models;
using VacationRental.Core.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public sealed class BookingsController : ControllerBase
    {
        private readonly IBookingsService _bookingsService;

        public BookingsController(IBookingsService bookingsService) => _bookingsService = bookingsService;

        [HttpGet]
        [Route("{bookingId:int}")]
        public BookingViewModel Get(int bookingId) => _bookingsService.Get(bookingId);

        [HttpPost]
        public ResourceIdViewModel Post(BookingBindingModel model) => _bookingsService.Book(model);
    }
}
