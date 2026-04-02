using AutoRent.Core.Entities;

namespace AutoRent.Core.Interfaces
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(string userId);
        Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId);
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalsByStatusAsync(string status);
        Task<IEnumerable<Rental>> GetRentalsWithDetailsAsync();
        Task<Rental?> GetRentalWithDetailsAsync(int id);
    }
}
