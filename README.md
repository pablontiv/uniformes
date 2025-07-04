# Uniformes Management System

A comprehensive system for managing employee uniforms, tracking inventory, and handling warehouse transactions.

## Project Structure

- **UniformesSystem.Database**: Entity Framework Core database project
- **UniformesSystem.API**: REST API for handling business logic and data access
- **UniformesSystem.Web**: Blazor Server frontend with MudBlazor UI

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- SQL Server (or Docker for containerized deployment)
- Visual Studio 2022 or VS Code

### Development Environment Setup

1. Clone the repository
2. Navigate to the solution folder
3. Run the following commands:

```bash
# Restore dependencies
dotnet restore

# Run Entity Framework migrations
dotnet ef database update --project UniformesSystem.Database --startup-project UniformesSystem.API

# Run the API
dotnet run --project UniformesSystem.API

# In a separate terminal, run the Blazor Server app
dotnet run --project UniformesSystem.Web
```

### Docker Setup

To run the application using Docker:

```bash
docker-compose up -d
```

This will start:

- SQL Server database container
- API container
- Blazor Server container

## Authentication

The system uses JWT (JSON Web Token) authentication with role-based authorization:

- Administrator: Full access to all system features
- Inventory Manager: Access to inventory and warehouse features
- HR Staff: Access to employee management features

## Features

- **User Authentication & Authorization**: Secure access based on roles
- **Employee Management**: Track employees with groups and classifications
- **Inventory Management**: Maintain catalog of uniform items with size tracking
- **Warehouse Transactions**: Record inventory movements (additions and reductions)
- **Reporting**: Generate reports for inventory levels, movements, and employee items

## Contributing

Please follow the project's version control strategy:

- Direct commits to main branch
- Each commit should represent a distinct, functioning iteration as outlined in the PRD
