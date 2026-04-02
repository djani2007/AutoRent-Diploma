using System.ComponentModel.DataAnnotations;

namespace AutoRent.Web.ViewModels
{
    public class CreateRentalViewModel
    {
        public int CarId { get; set; }

        public string CarName { get; set; } = string.Empty;

        public decimal PricePerDay { get; set; }

        [Required(ErrorMessage = "Началната дата е задължителна")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Крайната дата е задължителна")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        public decimal TotalPrice { get; set; }
    }
}