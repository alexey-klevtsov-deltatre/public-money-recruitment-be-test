using VacationRental.Core.Models;

namespace VacationRental.Core.Services
{
    public interface IRentalsService
    {
        RentalViewModel Get(int rentalId);
        ResourceIdViewModel AddRental(RentalBindingModel model);
        RentalViewModel UpdateRental(int rentalId, RentalBindingModel model);
    }
}
