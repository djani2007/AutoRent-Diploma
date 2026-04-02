using Microsoft.EntityFrameworkCore;
using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;
using AutoRent.Infrastructure.Data;

namespace AutoRent.Infrastructure.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _dbSet.Where(c => c.IsAvailable).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand)
        {
            return await _dbSet.Where(c => c.Brand.ToLower() == brand.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByBodyTypeAsync(string bodyType)
        {
            return await _dbSet.Where(c => c.BodyType.ToLower() == bodyType.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Car>> SearchCarsAsync(
            string? brand,
            string? bodyType,
            string? transmissionType,
            string? fuelType,
            decimal? minPrice,
            decimal? maxPrice,
            int? seats)
        {
            var query = _dbSet.Where(c => c.IsAvailable).AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
            {
                var value = brand.Trim().ToLower();
                query = query.Where(c => c.Brand != null && c.Brand.Trim().ToLower().Contains(value));
            }

            if (!string.IsNullOrWhiteSpace(bodyType))
            {
                var value = bodyType.Trim().ToLower();
                query = query.Where(c => c.BodyType != null && c.BodyType.Trim().ToLower() == value);
            }

            if (!string.IsNullOrWhiteSpace(transmissionType))
            {
                var value = transmissionType.Trim().ToLower();
                query = query.Where(c => c.TransmissionType != null && c.TransmissionType.Trim().ToLower() == value);
            }

            if (!string.IsNullOrWhiteSpace(fuelType))
            {
                var value = fuelType.Trim().ToLower();
                query = query.Where(c => c.FuelType != null && c.FuelType.Trim().ToLower() == value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);
            }

            if (seats.HasValue)
            {
                query = query.Where(c => c.Seats == seats.Value);
            }

            return await query.OrderBy(c => c.Brand).ThenBy(c => c.Model).ToListAsync();
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
        {
            var car = await _dbSet.FindAsync(carId);
            if (car == null || !car.IsAvailable)
                return false;

            var hasOverlappingRentals = await _context.Rentals
                .AnyAsync(r => r.CarId == carId &&
                               r.Status != "Cancelled" &&
                               r.Status != "Completed" &&
                               startDate.Date <= r.EndDate.Date &&
                               endDate.Date >= r.StartDate.Date);

            return !hasOverlappingRentals;
        }
    }
}