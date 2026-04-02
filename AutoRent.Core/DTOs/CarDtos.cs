using System.ComponentModel.DataAnnotations;

namespace AutoRent.Core.DTOs
{
    public class CarCreateDto
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
        public int Year { get; set; }

        public string? FuelType { get; set; }

        [Range(1, 10)]
        public int Seats { get; set; } = 5;

        public bool HasPaidVignette { get; set; } = true;
        public bool HasUnlimitedMileage { get; set; } = true;
        public bool VatIncluded { get; set; } = true;
    }

    public class CarUpdateDto
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

        public bool IsAvailable { get; set; }

        public string? Description { get; set; }

        [Range(1900, 2100)]
        public int Year { get; set; }

        public string? FuelType { get; set; }

        [Range(1, 10)]
        public int Seats { get; set; }

        public bool HasPaidVignette { get; set; }
        public bool HasUnlimitedMileage { get; set; }
        public bool VatIncluded { get; set; }
    }

    public class CarListDto
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public string TransmissionType { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public string FullName => $"{Brand} {Model}";
    }
}
