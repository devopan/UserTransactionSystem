# Transaction API

A RESTful API for a transaction data system built with .NET 8, C#, and PostgreSQL. This project implements a layered architecture with API, Services, and Infrastructure layers, following SOLID principles, OOP patterns like Polymorphism and generics. It utilizes the repository pattern with unit of work and the AutoMapper library.

## Project Purpose

This DEMO API provides endpoints for managing users and transactional actions in an transaction data system. It allows for:

Database operations
- User management (CRUD operations)
- Recording financial transactions (debits and credits)

Complex logic operations
- Generating reports on transactions per user and per transaction type as well as identify transactions above a certain threshold amount

The system is designed with clean architecture principles, making it maintainable, testable, and scalable.

## Technologies Used

- .NET 8
- C#
- Entity Framework Core
- PostgreSQL
- AutoMapper
- Swagger/OpenAPI
- xUnit (for testing)
- Moq (for mocking in tests)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- Visual Studio 2022 or another IDE that supports .NET development

## Setup Instructions

### 1. Clone the Repository

\`\`\`bash
git clone https://github.com/devopan/user-transaction-data-system.git
cd user-transaction-data-system
\`\`\`

### 2. Database Configuration

Update the connection string in `UserTransactionSystem.Web/appsettings.json` with your PostgreSQL credentials:

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=user-transaction-data-system;Username=your_username;Password=your_password"
  }
}
\`\`\`

### 3. Database Migrations

There are two ways to run the database migrations:

#### Option 1: Using the .NET CLI

Navigate to the Infrastructure project directory and run:

\`\`\`bash
cd UserTransactionSystem.Infrastructure
dotnet ef database update --project UserTransactionSystem.Infrastructure -s UserTransactionSystem.Web
\`\`\`

#### Option 2: Using the Package Manager Console in Visual Studio

1. Open the solution in Visual Studio
2. Select `UserTransactionSystem.Infrastructure` as the Default Project in Package Manager Console
3. Run the following command:

\`\`\`
Update-Database -StartupProject UserTransactionSystem.Web
\`\`\`

#### Option 3: Automatic Migrations

The application is configured to apply pending migrations automatically when it starts. Simply run the application, and the migrations will be applied.

### 4. Building and Running the Application

#### Using .NET CLI

\`\`\`bash
cd UserTransactionSystem.Web
dotnet build
dotnet run
\`\`\`

#### Using Visual Studio

1. Set `UserTransactionSystem.Web` as the startup project
2. Press F5 or click the "Run" button

### 5. Accessing the API

Once the application is running, you can access:

- API endpoints at `https://localhost:7101/api/`
- Swagger documentation at `https://localhost:7101/swagger`

## Project Structure

\`\`\`
UserTransactionSystem.sln
├── UserTransactionSystem.Web
│   ├── Controllers
│   │   ├── BaseController.cs
│   │   ├── UsersController.cs
│   │   ├── TransactionsController.cs
│   │   └── ReportingController.cs
│   ├── Middleware
│   │   └── ExceptionHandlingMiddleware.cs
│   ├── Models
│   │   ├── ErrorResponse.cs
|   |   └── GenericException.cs
│   ├── Extensions
│   │   └── ExceptionHandlingExtensions.cs
│   ├── Program.cs
│   ├── appsettings.json
|   └── README.md
├── UserTransactionSystem.Domain
│   ├── Entities
│   │   ├── User.cs
│   │   └── Transaction.cs
│   └── Enums
│       └── TransactionTypeEnum.cs
├── UserTransactionSystem.Infrastructure
│   ├── Data
│   │   ├── ApplicationDbContext.cs
|   |   └── ApplicationDbScopedFactory.cs
│   ├── Repositories
│   │   ├── IRepository.cs
│   │   └── Repository.cs
│   ├── UnitOfWork
│   │   ├── IUnitOfWork.cs
│   │   └── UnitOfWork.cs
│   └── Migrations
│       ├── InitialCreate.cs
|       └── ApplicationDbContextModelSnapshot.cs
├── UserTransactionSystem.Services
│   ├── DTOs
│   │   ├── CreateActionDto.cs
│   │   ├── CreateUserDto.cs
│   │   ├── HighVolumeTransactionReportDto.cs
│   │   ├── TransactionTypeTotalAmountReportDto.cs
│   │   ├── UpdateUserDto.cs
│   │   ├── UserDto.cs
│   │   └── UserTotalAmountReportDto.cs
│   ├── Interfaces
│   │   ├── IUserService.cs
│   │   ├── ITransactionService.cs
│   │   └── IReportingService.cs
│   ├── Services
│   │   ├── UserService.cs
│   │   ├── TransactionService.cs
│   │   └── ReportingService.cs
│   └── Mapping
│       └── MappingProfile.cs
├── UserTransactionSystem.Services.Test.Unit
│   └── Services
│       ├── UserServiceTests.cs
|       └── ReportingServiceTests.cs
└── UserTransactionSystem.Web.Test.Integration
    ├── CustomWebApplicationFactory.cs
    └── Controllers
        ├── UsersControllerTests.cs
        ├── TransactionsControllerTests.cs
        └── ReportingControllerTests.cs
\`\`\`

## API Endpoints

### Users

- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get a specific user
- `POST /api/users` - Create a new user
- `PUT /api/users/{id}` - Update a user
- `DELETE /api/users/{id}` - Delete a user

### Transactions

- `GET /api/transactions` - Get all transactions
- `GET /api/transactions/{id}` - Get a specific transaction
- `POST /api/transactions` - Create a new transaction

### Reporting

- `GET /api/reporting/users/transactionAmountTotal` - Get total transactions per user
- `GET /api/reporting/transactionTypes/transactionAmountTotal` - Get total transactions per transactions type
- `GET /api/reporting/high-volume-transactions?from={dd/MM/yyyy}&to={dd/MM/yyyy}&thresholdAmount={number}` - Get transactions that exceed the specified high volume threshold limit amount within the date range

## Error Handling

The API implements global exception handling to provide consistent error responses:

- All exceptions are caught by the global exception handling middleware
- PostgreSQL database errors are translated to user-friendly messages

## Creating Custom Migrations

If you need to create a new migration after modifying the entity models, use the following commands:

### Using .NET CLI

\`\`\`bash
cd UserTransactionSystem.Infrastructure
dotnet ef migrations add <MigrationName> --project UserTransactionSystem.Infrastructure -s UserTransactionSystem.Web
dotnet ef database update --project UserTransactionSystem.Infrastructure -s UserTransactionSystem.Web
\`\`\`

### Using Package Manager Console in Visual Studio

you may have to install package `Microsoft.EntityFrameworkCore.Tools` in the `UserTransactionSystem.Infrastructure` project first.

\`\`\`
Add-Migration <MigrationName> -Project UserTransactionSystem.Infrastructure -StartupProject UserTransactionSystem.Web
Update-Database -Project UserTransactionSystem.Infrastructure -StartupProject UserTransactionSystem.Web
\`\`\`

## Testing

The solution includes both unit tests and integration tests to ensure code quality and functionality check.

### Unit Tests

The `UserTransactionSystem.Services.Test.Unit` project contains unit tests for the service layer using xUnit and Moq for mocking dependencies. These tests focus on testing individual components in isolation.

#### Running Unit Tests

\`\`\`bash
cd UserTransactionSystem.Services.Test.Unit
dotnet test
\`\`\`

### Integration Tests

The `UserTransactionSystem.Web.Test.Integration` project contains integration tests that test the API endpoints end-to-end. These tests use the `WebApplicationFactory` to create a test server with an in-memory database.

#### Running Integration Tests

\`\`\`bash
cd UserTransactionSystem.Web.Test.Integration
dotnet test
\`\`\`

#### Integration Test Setup

The integration tests use:
- An in-memory database for testing
- Custom `WebApplicationFactory` to configure the test environment
- Seeded test data for consistent test results

## Design Principles

This project follows:

- **SOLID Principles**: Single responsibility, Open-closed, Liskov substitution, Interface segregation, and Dependency inversion
- **Command Query Segregation Principle (CQRS)**: Separate models for reading and writing data
- **Repository Pattern with Unit of Work**: Abstraction over data access with transaction support
- **Layered Architecture**: Clear separation of concerns between API, Services, and Infrastructure layers
- **Exception Handling**: Global exception handling
