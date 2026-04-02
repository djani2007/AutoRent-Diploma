using AutoRent.Core.DTOs;
using AutoRent.Core.Entities;

namespace AutoRent.Core.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task<Car> CreateCarAsync(CarCreateDto carDto);
        Task UpdateCarAsync(int id, CarUpdateDto carDto);
        Task DeleteCarAsync(int id);

        Task<IEnumerable<Car>> SearchCarsAsync(
            string? brand,
            string? bodyType,
            string? transmissionType,
            string? fuelType,
            decimal? minPrice,
            decimal? maxPrice,
            int? seats);

        Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate);
    }
}