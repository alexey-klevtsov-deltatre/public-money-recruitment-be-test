using System;
using VacationRental.Core.Models;
using VacationRental.Repository;

namespace VacationRental.Core.Services
{
    public sealed class RentalsService : IRentalsService
    {
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;

        public RentalsService(IVacationRepository<RentalViewModel> rentalRepository) => _rentalRepository = rentalRepository;

        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        public ResourceIdViewModel Rent(RentalBindingModel model)
        {
            if(model.Units < 1)
                throw new ApplicationException("Rental should have at least one unit");
            if (model.PreparationTimeInDays < 0)
                throw new ApplicationException("PreparationTimeInDays must be positive");

            var key = new ResourceIdViewModel { Id = _rentalRepository.NextId() };

            _rentalRepository.Insert(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });

            return key;
        }
    }
}
