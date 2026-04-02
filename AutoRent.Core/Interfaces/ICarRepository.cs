using AutoRent.Core.Entities;

namespace AutoRent.Core.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync();
        Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand);
        Task<IEnumerable<Car>> GetCarsByBodyTypeAsync(string bodyType);

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