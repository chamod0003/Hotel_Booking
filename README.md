#  Hotel Booking System API

A comprehensive hotel booking management system built with Clean Architecture principles in .NET 8.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp)
![License](https://img.shields.io/badge/license-MIT-green)

## Overview

This project demonstrates enterprise-level software architecture and design patterns through a hotel booking system. It features user authentication, hotel management, room availability tracking, and payment processing with multiple payment methods.

##  Architecture

Built using **Clean Architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│     Presentation Layer (Web API)        │  ← Controllers, Middleware
├─────────────────────────────────────────┤
│     Application Layer                   │  ← Services, DTOs, Business Logic
├─────────────────────────────────────────┤
│     Domain Layer                        │  ← Entities, Interfaces, Enums
├─────────────────────────────────────────┤
│     Infrastructure Layer                │  ← Repositories, EF Core, Data Access
└─────────────────────────────────────────┘
```

## Layer Responsibilities:
- **Domain Layer**: Core business entities and repository interfaces
- **Application Layer**: Business logic, services, and data transfer objects
- **Infrastructure Layer**: Data access implementation, EF Core repositories
- **Presentation Layer**: ASP.NET Core Web API controllers and configuration

 ## Key Features

- **Hotel Management**: Create, update, and search hotels with rooms and amenities
- **User Authentication**: JWT-based auth + Google OAuth integration
- **Booking System**: Real-time room availability checking and reservation management
- **Payment Processing**: Multiple payment methods using Factory Pattern
- **Advanced Search**: Search by location, price range, amenities, and room types
- **Caching**: Memory caching for improved performance
- **RESTful API**: Clean API design with Swagger documentation

 ## Tech Stack

| Category | Technology |
|----------|------------|
| **Framework** | .NET 8, ASP.NET Core Web API |
| **Database** | SQL Server, Entity Framework Core 8 |
| **Authentication** | JWT Bearer Tokens, Google OAuth 2.0 |
| **Patterns** | Repository, Factory, Strategy, Dependency Injection |
| **Caching** | IMemoryCache |
| **Testing** | xUnit, Moq |
| **Documentation** | Swagger/OpenAPI |
| **Security** | BCrypt password hashing |

##  Design Patterns Implemented

1. **Repository Pattern**: Abstraction layer for data access
2. **Factory Pattern**: `PaymentProcessorFactory` for payment method creation
3. **Strategy Pattern**: Multiple payment processor implementations
4. **Dependency Injection**: Constructor injection throughout
5. **DTO Pattern**: Separation between entities and API models

##  Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/chamod0003/Hotel_Booking.git
   cd Hotel_Booking
   ```

2. **Update connection string**
   
   Edit `Presentation_Layer/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=HotelProject;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
   }
   ```

3. **Apply database migrations**
   ```bash
   cd Infrastructure_Layer
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access Swagger UI**
   
   Navigate to: `https://localhost:5001/swagger`

##  API Endpoints

### Hotels
- `GET /api/hotel` - Get all hotels
- `GET /api/hotel/{id}` - Get hotel by ID
- `POST /api/hotel` - Create new hotel
- `PUT /api/hotel/{id}` - Update hotel
- `DELETE /api/hotel/{id}` - Delete hotel
- `GET /api/hotel/search?name={name}` - Search hotels by name
- `GET /api/hotel/nearby?lat={lat}&lng={lng}&radius={km}` - Find nearby hotels

### Bookings
- `GET /api/booking` - Get all bookings
- `GET /api/booking/{id}` - Get booking by ID
- `POST /api/booking` - Create new booking
- `PUT /api/booking/{id}` - Update booking
- `DELETE /api/booking/{id}` - Cancel booking
- `GET /api/booking/user/{userId}` - Get user's bookings

### Authentication
- `POST /api/user/register` - Register new user
- `POST /api/user/login` - Login with email/password
- `POST /api/user/google-login` - Login with Google

### Payments
- `POST /api/payment/process/{bookingId}` - Process payment
- `POST /api/payment/refund/{bookingId}` - Refund payment

##  Testing

Run unit tests:
```bash
cd TestProject
dotnet test
```

##  Database Schema

Key entities:
- **Hotel**: Hotel information with rooms, amenities, and pictures
- **Booking**: Reservation details with pricing and status
- **User**: Customer information and authentication
- **Payment**: Payment records and transactions
- **RoomType**: Room categories (Single, Double, Suite, etc.)

##  Security Features

- Password hashing with BCrypt
- JWT token-based authentication
- OAuth 2.0 integration (Google)
- CORS configuration
- SQL injection prevention via EF Core

##  Performance Optimizations

- Memory caching for frequently accessed data
- Async/await throughout for non-blocking operations
- EF Core query optimization with Include()
- Computed properties to reduce database calls

##  Future Enhancements

-  Implement Unit of Work pattern
-  Add comprehensive integration tests
-  Implement global exception handling middleware
-  Add AutoMapper for entity-DTO mapping
-  Add API versioning
-  Implement health checks endpoint
-  Add Redis caching for distributed scenarios
-  Add email notifications for bookings
-  Implement payment webhooks

##  Project Structure

```
Hotel_Booking/
├── Domain_Layer/              # Core business entities
│   ├── Models/Entity/
│   ├── Interface/
│   └── Enums/
├── Application_Layer/         # Business logic
│   ├── Services/
│   ├── DTO/
│   └── Interface/
├── Infrastructure_Layer/      # Data access
│   ├── Repositories/
│   ├── Data/
│   └── Migrations/
├── Presentation_Layer/        # Web API
│   ├── Controllers/
│   └── Program.cs
└── TestProject/              # Unit tests
```

##  Contributing

This is a portfolio project, but suggestions and feedback are welcome!


## 👨‍💻 Author

**Chamod**
- GitHub: [@chamod0003](https://github.com/chamod0003)

##  Acknowledgments

- Built as a demonstration of Clean Architecture principles
- Inspired by real-world hotel booking systems
- Follows SOLID principles and design patterns

---

⭐ If you find this project helpful, please consider giving it a star!
