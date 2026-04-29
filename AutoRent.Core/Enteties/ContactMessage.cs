using System.ComponentModel.DataAnnotations;

namespace AutoRent.Core.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // За заявки за промяна на профил
        public string? RequestType { get; set; }
        public string? RequestStatus { get; set; }
        public string? UserId { get; set; }

        public string? NewPhoneNumber { get; set; }
        public string? NewAddress { get; set; }
        public string? NewCity { get; set; }
        public string? NewIdentityCardNumber { get; set; }
        public string? NewDriverLicenseNumber { get; set; }
        public DateTime? NewDriverLicenseIssueDate { get; set; }
    }
}