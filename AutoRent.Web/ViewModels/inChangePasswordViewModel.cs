using System.ComponentModel.DataAnnotations;
using AutoRent.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using AutoRent.Core.Entities;

namespace AutoRent.Web.ViewModels
{
    public class AdminChangePasswordViewModel
    {
        public string UserId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Въведете нова парола.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Паролата трябва да е поне 6 символа.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Потвърдете новата парола.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}