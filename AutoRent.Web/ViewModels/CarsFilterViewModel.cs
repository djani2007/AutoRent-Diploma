using AutoRent.Core.Entities;

namespace AutoRent.Web.ViewModels
{
    public class CarsFilterViewModel
    {
        public List<Car> Cars { get; set; } = new();

        public string? Brand { get; set; }
        public string? BodyType { get; set; }
        public string? TransmissionType { get; set; }
        public string? FuelType { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? Seats { get; set; }

        public List<string> AvailableBrands { get; set; } = new();
        public List<string> AvailableBodyTypes { get; set; } = new();
        public List<string> AvailableTransmissionTypes { get; set; } = new();
        public List<string> AvailableFuelTypes { get; set; } = new();
        public List<int> AvailableSeats { get; set; } = new();

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool HasPeriod => StartDate.HasValue && EndDate.HasValue;
    }
}