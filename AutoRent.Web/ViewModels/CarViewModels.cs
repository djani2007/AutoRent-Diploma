using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using AutoRent.Core.Entities;

namespace AutoRent.Web.ViewModels
{
   
    public class RentCarViewModel
    {
        public int CarId { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string? CarImage { get; set; }
        public decimal PricePerDay { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public string? Notes { get; set; }
    }

    public class CarCreateViewModel
    {
        [Required(ErrorMessage = "Brand is required")]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Body type is required")]
        public string BodyType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Transmission type is required")]
        public string TransmissionType { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal PricePerDay { get; set; }

        public string? Description { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; } = DateTime.Now.Year;

        public string? FuelType { get; set; }

        [Range(1, 10)]
        public int Seats { get; set; } = 5;

        public bool HasPaidVignette { get; set; } = true;
        public bool HasUnlimitedMileage { get; set; } = true;
        public bool VatIncluded { get; set; } = true;

        public bool IsAvailable { get; set; } = true;
    }

    public class CarEditViewModel : CarCreateViewModel
    {
        public int Id { get; set; }
    }
}
