using System.ComponentModel.DataAnnotations;

namespace AutoRent.Web.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Въведете текущата парола.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Въведете нова парола.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Новата парола трябва да е поне 6 символа.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потвърдете новата парола.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Новата парола и потвърждението не съвпадат.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}