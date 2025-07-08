# Uniform Management System

A comprehensive system for managing employee uniforms, tracking inventory, and handling warehouse transactions. This application helps HR departments control the distribution of uniforms to employees, maintain accurate inventory records, and generate reports for decision-making.

## Overview

The Uniform Management System is designed to help organizations efficiently manage the complete lifecycle of uniforms: from acquisition and storage to employee assignment and eventual return or disposal. It's particularly useful for organizations that need precise control of uniform assignments, such as manufacturing companies, hospitals, schools, and security forces.

### Purpose

- Control available uniform inventory
- Manage uniform assignments to employees
- Record returns and write-offs
- Generate reports for decision-making
- Maintain a movement history for audits

## Project Structure

- **UniformesSystem.Database**: Entity Framework Core database project with models and migrations
- **UniformesSystem.API**: REST API for handling business logic and data access
- **UniformesSystem.Web**: Blazor WebAssembly frontend with MudBlazor UI components

## Architecture

The system follows a three-tier architecture to ensure scalability, maintainability, and security:

### Presentation Layer (Frontend)
- Developed with **Blazor WebAssembly**
- User interface built with **MudBlazor** (Material Design components)
- Implements Single Page Application (SPA) design pattern

### Service Layer (Backend)
- RESTful API developed with **ASP.NET Core**
- JWT authentication for secure access
- Global exception handling middleware
- Domain-oriented controllers

### Data Layer
- **Entity Framework Core** as ORM
- **SQL Server** database
- Migrations for database version control

### Architecture Diagram

```
┌─────────────────┐      ┌─────────────────┐      ┌─────────────────┐
│                 │      │                 │      │                 │
│  UniformesWeb   │ ──── │  UniformesAPI   │ ──── │ UniformesDB     │
│  (Blazor WASM)  │  HTTP│  (ASP.NET Core) │  EF  │ (SQL Server)    │
│                 │      │                 │      │                 │
└─────────────────┘      └─────────────────┘      └─────────────────┘
```

## Technologies Used

- **.NET 8**: Development framework
- **Blazor WebAssembly**: Frontend
- **ASP.NET Core**: Backend API
- **Entity Framework Core**: ORM
- **SQL Server**: Database
- **MudBlazor**: UI Components
- **Docker**: Containerization
- **JWT**: Authentication

## Data Model

The system is based on a relational data model representing the main business entities:

### Key Entities

1. **Employee**: Represents employees receiving uniforms
   - Properties: ID, Name, Group, Hire Date, etc.

2. **Group**: Departments to which employees belong
   - Properties: ID, Name, Employee Type, etc.

3. **Item**: Represents uniform types or elements
   - Properties: ID, Name, Description, etc.

4. **ItemType**: Item categories (shirt, pants, shoes, etc.)
   - Properties: ID, Name, Description, etc.

5. **Size**: Available sizes for items
   - Properties: ID, Name (S, M, L, XL, etc.)

6. **Inventory**: Available inventory records
   - Properties: ID, ItemID, SizeID, Quantity, etc.

7. **WarehouseMovement**: Inventory input/output movements
   - Properties: ID, Type (input/output), Quantity, Date, etc.

8. **Issuance**: Record of uniform assignments to employees
   - Properties: ID, EmployeeID, Date, Items assigned, etc.

9. **Return**: Record of uniform returns
   - Properties: ID, IssuanceID, Date, Items returned, etc.

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- Docker and Docker Compose (for containerized deployment)
- Modern web browser
- Minimum 4GB RAM available
- At least 10GB free disk space

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

# In a separate terminal, run the Blazor app
dotnet run --project UniformesSystem.Web
```

### Docker Setup (Recommended)

To run the application using Docker:

```bash
docker-compose up -d
```

This will start:
- SQL Server database container
- API container
- Blazor WebAssembly container

### Verifying Services

Check that containers are running properly:

```bash
docker ps
```

You should see three containers running:
- uniformes-db
- uniformes-api
- uniformes-web

### Accessing the Application

Open your web browser and access:

```
http://localhost:5000
```

## Authentication

The system uses JWT authentication with role-based authorization. The following test accounts are preconfigured:

### Administrator Account
- **Username:** admin
- **Password:** admin123
- **Role:** Administrator
- **Permissions:** Full access to all system features

### Inventory Manager Account
- **Username:** inventory
- **Password:** inventory123
- **Role:** Inventory Manager
- **Permissions:** Inventory management and warehouse transaction access

### HR Staff Account
- **Username:** hr
- **Password:** hr123
- **Role:** HR Staff
- **Permissions:** Employee management and uniform assignment access

## Key Features

### Employee Management
- Create, edit, and delete employee records
- Assign employees to groups/departments
- Track assigned uniforms history

### Inventory Management
- Track uniform stock by type and size
- Record warehouse inputs and outputs
- Set and monitor minimum stock alerts

### Uniform Assignment
- Record uniform deliveries to employees
- Assign based on predefined profiles
- Optional digital signature for receipt

### Returns
- Record uniform returns from employees
- Assess returned item condition
- Return to inventory or write-off

### Reporting
- Current inventory status
- Movement history
- Assignments by employee/department
- Future needs projections

### Administration and Security
- User and role management
- Granular permissions by feature
- Action audit log

## Test Cases

These test cases demonstrate the system's functionality according to HR department requirements:

### Test Case 1: Preconfigured Catalogs Verification
1. Log in as administrator (admin/admin123)
2. Explore the following catalogs to verify preconfigured data:
   - Employee groups (should include groups A, B, C, D, E, and Z)
   - Item types
   - Available sizes

### Test Case 2: Employee Management by Group
1. Log in as administrator (admin/admin123)
2. Navigate to "Employees" section
3. Create a new "Administrative" employee (Group Z)
4. Create a new "Unionized" employee (Groups A, B, C, D, or E)
5. Verify they were created correctly and types match assigned groups

### Test Case 3: Inventory Control
1. Log in as administrator (admin/admin123)
2. Navigate to "Inventory" section
3. Check current item availability
4. Register a new inventory entry (e.g., a purchase order)
5. Verify stock updated correctly

### Test Case 4: Uniform Assignment by Employee Type
1. Log in as administrator (admin/admin123)
2. Navigate to "Issuances" section
3. Create a new issuance for an Administrative employee
   - Verify you can only assign appropriate items (shirts, jackets, pants)
   - Verify you cannot assign specialized protection equipment
4. Create a new issuance for a Unionized employee
   - Verify you can only assign appropriate items (helmets, boots, gloves, overalls)
   - Verify you cannot assign administrative items
5. Verify both employee types can receive common items (like reflective vests)

### Test Case 5: Movement Reports
1. Log in as administrator (admin/admin123)
2. Navigate to "Reports" section
3. Generate a warehouse movement report
4. Verify it shows registered inputs and outputs
5. Check that it indicates which employees received deliveries

## Business Requirements

This application is designed to meet the following business requirements from the HR department:

- Control employee uniforms to prevent waste and maintain accurate inventory
- Track each employee by personal code and group
- Distinguish between "Administrative" employees (Group Z, office staff) and "Unionized" employees (Groups A-E, field workers)
- Ensure Administrative employees receive only company-branded clothing (shirts, jackets, pants)
- Ensure Unionized employees receive specialized protective equipment (helmets, safety boots, gloves, overalls)
- Prevent cross-assignment of items between employee types, except for common items like reflective vests
- Track items by size according to supplier notation (Mexican, American, European)
- Maintain minimum stock levels by item type and size
- Record warehouse entries by purchase orders, recounts, or returns
- Record warehouse exits through employee deliveries
- Generate reports for decision-making

## Troubleshooting

### Application Not Starting Correctly
1. Verify that ports 1433, 5000, and 80 are not being used by other applications
2. Check that Docker is running
3. Restart containers:
   ```bash
   docker-compose down
   docker-compose up -d
   ```

### Database Connection Issues
1. Verify the database container is running:
   ```bash
   docker ps | grep db
   ```
2. Check logs for possible errors:
   ```bash
   docker logs uniformes-db
   ```

### Login Errors
1. Make sure you're using the correct credentials
2. Check if the API is functioning:
   ```bash
   docker logs uniformes-api
   ```

If problems persist, you can completely restart the application by removing all containers and volumes:

```bash
docker-compose down -v
docker-compose up -d
```

Note: This command will delete all previously stored data.

## Contributing

Please follow the project's version control strategy:
- Direct commits to main branch
- Each commit should represent a distinct, functioning iteration as outlined in the PRD
