namespace VacationRental.Core.Models
{
    public sealed class RentalViewModel
    {
        public RentalViewModel(int id, RentalBindingModel bindingModel)
        {
            Id = id;
            Units = bindingModel.Units;
            PreparationTimeInDays = bindingModel.PreparationTimeInDays;
        }

        public RentalViewModel()
        {
        }

        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }
    }
}
