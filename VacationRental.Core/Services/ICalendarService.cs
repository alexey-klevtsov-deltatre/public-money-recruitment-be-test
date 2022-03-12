using System;
using VacationRental.Core.Models;

namespace VacationRental.Core.Services
{
    public interface ICalendarService
    {
        CalendarViewModel Get(int rentalId, DateTime start, int nights);
    }
}