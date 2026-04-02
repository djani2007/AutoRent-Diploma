using AutoRent.Core.DTOs;
using AutoRent.Core.Entities;

namespace AutoRent.Core.Interfaces
{
    public interface IRentalService
    {
        Task<Rental?> GetRentalByIdAsync(int id);
        Task<IEnumerable<Rental>> GetAllRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(string userId);
        Task<Rental> CreateRentalAsync(RentalCreateDto rentalDto);
        Task UpdateRentalStatusAsync(int id, string status);
        Task CancelRentalAsync(int id);
        Task<decimal> CalculateTotalPriceAsync(int carId, DateTime startDate, DateTime endDate);
    }
}
