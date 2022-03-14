using Microsoft.AspNetCore.Mvc;
using VacationRental.Core.Models;
using VacationRental.Core.Services;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public sealed class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;

        public RentalsController(IRentalsService rentalsService) => _rentalsService = rentalsService;

        [HttpGet]
        [Route("{rentalId:int}")]
        public RentalViewModel Get(int rentalId) => _rentalsService.Get(rentalId);

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model) => _rentalsService.AddRental(model);

        [HttpPut]
        [Route("{rentalId:int}")]
        public RentalViewModel Put(int rentalId, [FromBody]RentalBindingModel model) => _rentalsService.UpdateRental(rentalId, model);
    }
}
