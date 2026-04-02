using Microsoft.EntityFrameworkCore;
using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;
using AutoRent.Infrastructure.Data;

namespace AutoRent.Infrastructure.Repositories
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId)
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.CarId == carId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.Status == "Active" || r.Status == "Pending")
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByStatusAsync(string status)
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.User)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsWithDetailsAsync()
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Rental?> GetRentalWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(r => r.Car)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
