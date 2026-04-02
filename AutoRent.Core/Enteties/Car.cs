using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRent.Core.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string BodyType { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string TransmissionType { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public int Year { get; set; }

        [MaxLength(50)]
        public string? FuelType { get; set; }

        public int Seats { get; set; } = 5;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool HasPaidVignette { get; set; } = true;
        public bool HasUnlimitedMileage { get; set; } = true;
        public bool VatIncluded { get; set; } = true;

        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

        public string FullName => $"{Brand} {Model}";
    }
}
