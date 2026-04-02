using System.ComponentModel.DataAnnotations;

namespace AutoRent.Web.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден email адрес")]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Темата е задължителна")]
        [MaxLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Съобщението е задължително")]
        [MaxLength(2000)]
        public string Message { get; set; } = string.Empty;
    }
}