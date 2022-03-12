using System;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Repository;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IVacationRepository<RentalViewModel> _rentalRepository;

        public RentalsController(IVacationRepository<RentalViewModel> rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId)
        {
            var rental = _rentalRepository.Get(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentalRepository.NextId() };

            _rentalRepository.Insert(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units
            });

            return key;
        }
    }
}
