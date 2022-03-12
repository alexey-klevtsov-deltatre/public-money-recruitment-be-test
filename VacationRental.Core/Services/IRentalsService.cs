using VacationRental.Core.Models;

namespace VacationRental.Core.Services
{
    public interface IRentalsService
    {
        RentalViewModel Get(int rentalId);
        ResourceIdViewModel Rent(RentalBindingModel model);
    }
}