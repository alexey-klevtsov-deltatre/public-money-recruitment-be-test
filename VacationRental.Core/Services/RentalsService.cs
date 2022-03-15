using System;
using System.Linq;
using VacationRental.Core.Exceptions;
using VacationRental.Core.Models;
using VacationRental.Repository;

namespace VacationRental.Core.Services
{
    public sealed class RentalsService : IRentalsService
    {
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;
        private readonly IBookingsService _bookingsService;

        public RentalsService(IVacationRepository<RentalViewModel> rentalRepository, IBookingsService bookingsService)
        {
            _rentalRepository = rentalRepository;
            _bookingsService = bookingsService;
        }

        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        public ResourceIdViewModel AddRental(RentalBindingModel model)
        {
            Validate(model);

            var key = new ResourceIdViewModel { Id = _rentalRepository.NextId() };

            _rentalRepository.Insert(key.Id, new RentalViewModel(key.Id, model));

            return key;
        }

        public RentalViewModel UpdateRental(int rentalId, RentalBindingModel model)
        {
            Validate(model);

            var oldRental = _rentalRepository.Get(rentalId);
            if (oldRental == null)
                throw new ApplicationException("Rental not found");

            var newRental = new RentalViewModel(rentalId, model);
                var overlapped = _bookingsService.GetOverlappings(newRental).FirstOrDefault();
                if (overlapped != null)
                    throw new RentalOverlappedException(overlapped);

                return _rentalRepository.Update(rentalId, newRental);
        }

        private static void Validate(RentalBindingModel model)
        {
            if (model.Units < 1)
                throw new ApplicationException("Rental should have at least one unit");
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("PreparationTimeInDays must be positive");
        }
    }
}
