using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRent.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DriverLicenseNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(10)]
        public string EGN { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string IdentityCardNumber { get; set; } = string.Empty;

        [Required]
        public DateTime DriverLicenseIssueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        [NotMapped]
        public string Role { get; set; } = "Client";

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}