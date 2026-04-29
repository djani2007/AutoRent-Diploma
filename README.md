# AUTORENT - Car Rental System

A web-based car rental system built with ASP.NET Core MVC.

## Project Structure

```
AutoRent/
├── AutoRent.Core/           # Business Logic Layer
│   ├── Entities/            # Domain entities (User, Car, Rental)
│   ├── Interfaces/          # Service and Repository interfaces
│   ├── Services/            # Business logic services
│   └── DTOs/                # Data Transfer Objects
│
├── AutoRent.Infrastructure/ # Data Access Layer
│   ├── Data/                # DbContext
│   ├── Repositories/        # Repository implementations
│   └── Configurations/      # EF Core configurations
│
└── AutoRent.Web/            # Presentation Layer
    ├── Controllers/         # MVC Controllers
    ├── Views/               # Razor Views
    ├── ViewModels/          # View Models
    └── wwwroot/             # Static files (CSS, JS, images)
```

## Technologies Used

- **Framework:** ASP.NET Core 8.0 MVC
- **Language:** C#
- **Database:** MS SQL Server
- **ORM:** Entity Framework Core
- **Frontend:** Bootstrap 5, Bootstrap Icons

## Features

### Client Features
- User registration and login
- Browse available cars
- View car details (with features like paid vignette, unlimited mileage, VAT included)
- Rent a car
- View rental history
  TEST ACCOUNT
  
 Name: Krasimir
Email: krasimir@krasimir.com
Password: krasimir123

Name: Iliq
Email: iliq@iliq.bg
Password: iliqiliq0


### Admin Features
- Car management (add, edit, delete)
- User management
- Rental history and status management
- Dashboard with statistics

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB or full version)
- Visual Studio 2022 (recommended)

### Installation

1. **Clone or extract the project**

2. **Update the connection string** in `AutoRent.Web/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AutoRentDb;Trusted_Connection=True;"
   }
   ```

3. **Open the solution** in Visual Studio:
   - Open `AutoRent.sln`

4. **Restore NuGet packages**:
   ```bash
   dotnet restore
   ```

5. **Run migrations** (if needed):
   ```bash
   cd AutoRent.Web
   dotnet ef database update
   ```

6. **Run the application**:
   ```bash
   dotnet run
   ```
   Or press F5 in Visual Studio.

## Default Admin Account

- **Email:** admin@autorent.com
- **Password:** admin123

## Car Features (Bulgarian)

Each car includes:
- **Платена винетка** (Paid Vignette)
- **Неограничен пробег** (Unlimited Mileage)
- **Включен ДДС 20%** (VAT 20% Included)

## License

This project is created for educational purposes.

## Author

MiniMax Agent
