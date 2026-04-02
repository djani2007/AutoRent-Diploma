using System.ComponentModel.DataAnnotations;

namespace AutoRent.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден Email адрес")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Паролата е задължителна")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [MaxLength(100, ErrorMessage = "Името може да е максимум 100 символа")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [MaxLength(100, ErrorMessage = "Фамилията може да е максимум 100 символа")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден Email адрес")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонът е задължителен")]
        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Адресът е задължителен")]
        [MaxLength(200, ErrorMessage = "Адресът може да е максимум 200 символа")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Градът е задължителен")]
        [MaxLength(100, ErrorMessage = "Градът може да е максимум 100 символа")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Номерът на шофьорската книжка е задължителен")]
        [MaxLength(50, ErrorMessage = "Номерът на книжката може да е максимум 50 символа")]
        public string DriverLicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "ЕГН е задължително")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "ЕГН трябва да съдържа точно 10 цифри")]
        public string EGN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Датата на раждане е задължителна")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Номерът на личната карта е задължителен")]
        [MaxLength(50, ErrorMessage = "Номерът на личната карта може да е максимум 50 символа")]
        public string IdentityCardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Датата на издаване на книжката е задължителна")]
        [DataType(DataType.Date)]
        public DateTime DriverLicenseIssueDate { get; set; }

        [Required(ErrorMessage = "Паролата е задължителна")]
        [MinLength(6, ErrorMessage = "Паролата трябва да е поне 6 символа")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потвърждението на паролата е задължително")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Името е задължително")]
        [MaxLength(100, ErrorMessage = "Името може да е максимум 100 символа")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилията е задължителна")]
        [MaxLength(100, ErrorMessage = "Фамилията може да е максимум 100 символа")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефонът е задължителен")]
        [Phone(ErrorMessage = "Невалиден телефонен номер")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Адресът е задължителен")]
        [MaxLength(200, ErrorMessage = "Адресът може да е максимум 200 символа")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Градът е задължителен")]
        [MaxLength(100, ErrorMessage = "Градът може да е максимум 100 символа")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Номерът на шофьорската книжка е задължителен")]
        [MaxLength(50, ErrorMessage = "Номерът на книжката може да е максимум 50 символа")]
        public string DriverLicenseNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "ЕГН е задължително")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "ЕГН трябва да съдържа точно 10 цифри")]
        public string EGN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Датата на раждане е задължителна")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Номерът на личната карта е задължителен")]
        [MaxLength(50, ErrorMessage = "Номерът на личната карта може да е максимум 50 символа")]
        public string IdentityCardNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Датата на издаване на книжката е задължителна")]
        [DataType(DataType.Date)]
        public DateTime DriverLicenseIssueDate { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}