# RestERP - Restaurant Management System - SOON!

RestERP is a comprehensive Enterprise Resource Planning (ERP) system developed for restaurant businesses. This application is built with **.NET Core 9.0** and follows Clean Architecture principles.

<div align="center">
    <table>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/9a1214bd-2772-48dc-81a9-474f6c58d589" alt="mainpage" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/40f36f92-aae3-4131-b013-e1bf24ee0f15" alt="management" width="400"/>
            </td>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/74c4cadd-49ca-43e6-8060-00a5ba1af2b2" alt="category" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/81eaf971-96bb-4d21-ac66-956578259a7a" alt="panel" width="400"/>
            </td>
        </tr>
    </table>
</div>

## Project Structure

The project has a layered architecture in line with Clean Architecture principles:

| Layer | Description |
|-------|-------------|
| **RestERP.Domain** | Contains core entities, enum values, and domain logic |
| **RestERP.Application** | Contains application services, validation rules, and business logic |
| **RestERP.Infrastructure** | Contains database connection, repository implementations, and external services |
| **RestERP.Web** | Contains user interface, controllers, and views |

## 🛠️ Technologies

### Core Framework
- **.NET Core 9.0**: Modern, cross-platform framework
- **ASP.NET Core MVC**: Web application framework
- **Entity Framework Core**: ORM (Object-Relational Mapping) tool
- **SQL Server**: Database
- **Repository Pattern & Unit of Work**: Data access layer pattern
- **In-Memory Caching**: Optimizes performance by caching frequently accessed data

### Development Tools
- **Visual Studio 2022**: Primary IDE
- **Git**: Version control

## ✨ Core Features

### 🍽️ Order Management
- Create, edit, and track orders
- Monitor order status (New, In Progress, Completed, Cancelled)
- Table-based order management

### 🍕 Food Management
- Add, edit, and categorize food items
- Price management

### 👥 Customer Management
- Create and edit customer records
- View customer order history

### 👨‍💼 Employee Management
- Employee records and role assignments

### Table Management
- Table status tracking
- Table-based order viewing

## 📦 Basic Entities

The project uses the following core entities:

| Entity | Description |
|--------|-------------|
| **FoodCategory** | Food categories |
| **Food** | Food information |
| **Customer** | Customer information |
| **Employee** | Employee information |
| **Order** | Order information |
| **OrderItem** | Order items |
| **Table** | Table information |

### Base Entity Properties
All entities are derived from the `BaseEntity` class and contain the following properties:
- `Id` (int): Unique identifier
- `IsDeleted` (bool): Deletion status
- `CreatedById` (long?): Creator user
- `CreatedDate` (DateTime?): Creation date
- `UpdatedById` (long?): Updater user
- `UpdatedDate` (DateTime?): Update date

## 👥 Employee Roles

The system defines the following employee roles:
- Manager
- Chef
- Waiter
- Cashier

## 📊 Order Statuses

The following statuses are defined for orders:
- New
- InProgress
- Completed
- Cancelled

## 🔄 Repository Pattern

The project uses Generic Repository and Unit of Work pattern, providing:
- A standard interface for database operations
- Prevention of code duplication
- Increased testability
- Simplified transaction management

## 🚀 Performance Optimizations

### Memory Caching Strategy
- Implementation of ASP.NET Core's built-in Memory Cache
- Caching for frequently accessed data like food categories, food items, and images
- Different cache durations based on data change frequency
- Manual cache invalidation capabilities for data consistency

## 🚀 Getting Started

### Prerequisites
- .NET Core 9.0 SDK
- Visual Studio 2022
- SQL Server 2019 or later
