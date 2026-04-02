using AutoRent.Core.Entities;

namespace AutoRent.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalCars { get; set; }
        public int TotalUsers { get; set; }
        public int TotalRentals { get; set; }
        public int ActiveRentals { get; set; }
        public int UnreadMessagesCount { get; set; }

        public List<Rental> RecentRentals { get; set; } = new();
        public List<ContactMessage> RecentMessages { get; set; } = new();
    }

    public class AdminUserDetailsViewModel
    {
        public ApplicationUser User { get; set; } = null!;
        public string Role { get; set; } = "Client";

        public int TotalRentals { get; set; }
        public int ActiveRentals { get; set; }
        public decimal TotalSpent { get; set; }

        public List<Rental> Rentals { get; set; } = new();
    }
}