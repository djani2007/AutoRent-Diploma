using AutoRent.Core.DTOs;
using AutoRent.Core.Entities;
using AutoRent.Core.Interfaces;

namespace AutoRent.Core.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync()
        {
            return await _carRepository.GetAvailableCarsAsync();
        }
        public async Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        {
            return await _carRepository.GetAvailableCarsAsync(startDate, endDate);
        }

        public async Task<Car?> GetCarByIdAsync(int id)
        {
            return await _carRepository.GetByIdAsync(id);
        }

        public async Task<Car> CreateCarAsync(CarCreateDto carDto)
        {
            var car = new Car
            {
                Brand = carDto.Brand,
                Model = carDto.Model,
                BodyType = carDto.BodyType,
                TransmissionType = carDto.TransmissionType,
                ImageUrl = carDto.ImageUrl,
                PricePerDay = carDto.PricePerDay,
                Description = carDto.Description,
                Year = carDto.Year,
                FuelType = carDto.FuelType,
                Seats = carDto.Seats,
                HasPaidVignette = carDto.HasPaidVignette,
                HasUnlimitedMileage = carDto.HasUnlimitedMileage,
                VatIncluded = carDto.VatIncluded,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow
            };

            return await _carRepository.AddAsync(car);
        }

        public async Task UpdateCarAsync(int id, CarUpdateDto carDto)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
                throw new KeyNotFoundException($"Car with id {id} not found");

            car.Brand = carDto.Brand;
            car.Model = carDto.Model;
            car.BodyType = carDto.BodyType;
            car.TransmissionType = carDto.TransmissionType;
            car.ImageUrl = carDto.ImageUrl;
            car.PricePerDay = carDto.PricePerDay;
            car.IsAvailable = carDto.IsAvailable;
            car.Description = carDto.Description;
            car.Year = carDto.Year;
            car.FuelType = carDto.FuelType;
            car.Seats = carDto.Seats;
            car.HasPaidVignette = carDto.HasPaidVignette;
            car.HasUnlimitedMileage = carDto.HasUnlimitedMileage;
            car.VatIncluded = carDto.VatIncluded;

            await _carRepository.UpdateAsync(car);
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
                throw new KeyNotFoundException($"Car with id {id} not found");

            await _carRepository.DeleteAsync(car);
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
            return await _carRepository.SearchCarsAsync(
                brand,
                bodyType,
                transmissionType,
                fuelType,
                minPrice,
                maxPrice,
                seats);
        }

       
        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
        {
            return await _carRepository.IsCarAvailableAsync(carId, startDate, endDate);
        }
    }
}