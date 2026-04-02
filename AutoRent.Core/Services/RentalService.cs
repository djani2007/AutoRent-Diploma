using AutoRent.Core.DTOs;
using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;

namespace AutoRent.Core.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly ICarRepository _carRepository;

        public RentalService(IRentalRepository rentalRepository, ICarRepository carRepository)
        {
            _rentalRepository = rentalRepository;
            _carRepository = carRepository;
        }

        public async Task<Rental?> GetRentalByIdAsync(int id)
        {
            return await _rentalRepository.GetRentalWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Rental>> GetAllRentalsAsync()
        {
            return await _rentalRepository.GetRentalsWithDetailsAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(string userId)
        {
            return await _rentalRepository.GetRentalsByUserIdAsync(userId);
        }

        public async Task<Rental> CreateRentalAsync(RentalCreateDto rentalDto)
        {
            if (rentalDto.StartDate.Date < DateTime.Today)
                throw new InvalidOperationException("Началната дата не може да бъде в миналото.");

            if (rentalDto.EndDate.Date < rentalDto.StartDate.Date)
                throw new InvalidOperationException("Крайната дата не може да бъде преди началната дата.");

            var isAvailable = await _carRepository.IsCarAvailableAsync(
                rentalDto.CarId,
                rentalDto.StartDate.Date,
                rentalDto.EndDate.Date);

            if (!isAvailable)
                throw new InvalidOperationException("Автомобилът не е свободен за избрания период.");

            var totalPrice = await CalculateTotalPriceAsync(
                rentalDto.CarId,
                rentalDto.StartDate.Date,
                rentalDto.EndDate.Date);

            

            var rental = new Rental
            {
                UserId = rentalDto.UserId,
                CarId = rentalDto.CarId,
                StartDate = rentalDto.StartDate.Date,
                EndDate = rentalDto.EndDate.Date,
                TotalPrice = totalPrice,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                Notes = rentalDto.Notes
            };

            return await _rentalRepository.AddAsync(rental);
        }

        public async Task UpdateRentalStatusAsync(int id, string status)
        {
            var rental = await _rentalRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Rental with id {id} not found");

            rental.Status = status;
            await _rentalRepository.UpdateAsync(rental);
        }

        public async Task CancelRentalAsync(int id)
        {
            await UpdateRentalStatusAsync(id, "Cancelled");
        }

        public async Task<decimal> CalculateTotalPriceAsync(int carId, DateTime startDate, DateTime endDate)
        {
            var car = await _carRepository.GetByIdAsync(carId)
                ?? throw new KeyNotFoundException($"Car with id {carId} not found");

            var days = (endDate.Date - startDate.Date).Days + 1;
            return days * car.PricePerDay;
        }
    }
}