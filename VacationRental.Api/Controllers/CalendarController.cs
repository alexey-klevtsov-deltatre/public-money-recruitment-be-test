using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Core.Models;
using VacationRental.Core.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService) => _calendarService = calendarService;

        [HttpGet]
        public CalendarViewModel Get(int rentalId, DateTime start, int nights) => _calendarService.Get(rentalId, start, nights);
    }
}
