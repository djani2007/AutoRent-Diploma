using System.ComponentModel.DataAnnotations;

namespace AutoRent.Core.DTOs
{
    public class RentalCreateDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        public string? Notes { get; set; }
    }

    public class RentalListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string CarName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalDays => (EndDate - StartDate).Days;
    }

    public class RentalDetailsDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public int CarId { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string CarImageUrl { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
        public int TotalDays => (EndDate - StartDate).Days;
    }
}
