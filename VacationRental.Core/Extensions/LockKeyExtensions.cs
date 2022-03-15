using VacationRental.Core.Models;

namespace VacationRental.Core.Extensions
{
    internal static class LockKeyExtensions
    {
        public static string LockKey(this RentalViewModel rental) => $"rental-{rental.Id}";
    }
}
