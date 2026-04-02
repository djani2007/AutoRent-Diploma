using AutoRent.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoRent.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.DriverLicenseNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EGN).IsRequired().HasMaxLength(10);
                entity.Property(e => e.IdentityCardNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.DriverLicenseIssueDate).IsRequired();
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Brand).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(100);
                entity.Property(e => e.BodyType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.TransmissionType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.FuelType).HasMaxLength(50);
                entity.Property(e => e.PricePerDay).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(500);

                entity.HasOne(r => r.User)
                    .WithMany(u => u.Rentals)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Car)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(r => r.CarId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Subject).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            });

            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Brand = "BMW",
                    Model = "320i",
                    BodyType = "Sedan",
                    TransmissionType = "Automatic",
                    ImageUrl = "/images/cars/bmw-320i.jpg",
                    PricePerDay = 120.00m,
                    IsAvailable = true,
                    Description = "Luxury sedan with premium features",
                    Year = 2023,
                    FuelType = "Petrol",
                    Seats = 5,
                    HasPaidVignette = true,
                    HasUnlimitedMileage = true,
                    VatIncluded = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Car
                {
                    Id = 2,
                    Brand = "Mercedes-Benz",
                    Model = "C200",
                    BodyType = "Coupe",
                    TransmissionType = "Automatic",
                    ImageUrl = "/images/cars/mercedes-c200.jpg",
                    PricePerDay = 150.00m,
                    IsAvailable = true,
                    Description = "Elegant coupe for style lovers",
                    Year = 2023,
                    FuelType = "Petrol",
                    Seats = 4,
                    HasPaidVignette = true,
                    HasUnlimitedMileage = true,
                    VatIncluded = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Car
                {
                    Id = 3,
                    Brand = "Audi",
                    Model = "A4",
                    BodyType = "Sedan",
                    TransmissionType = "Manual",
                    ImageUrl = "/images/cars/audi-a4.jpg",
                    PricePerDay = 110.00m,
                    IsAvailable = true,
                    Description = "Sporty sedan with dynamic performance",
                    Year = 2022,
                    FuelType = "Diesel",
                    Seats = 5,
                    HasPaidVignette = true,
                    HasUnlimitedMileage = true,
                    VatIncluded = true,
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
    }
}