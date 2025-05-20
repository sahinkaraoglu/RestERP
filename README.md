# RestERP - Restaurant Management System - SOON!

RestERP is a comprehensive Enterprise Resource Planning (ERP) system developed for restaurant businesses. This application is built with **.NET Core 9.0** and follows Clean Architecture principles.

<div align="center">
   <div align="center">
    <table>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/9a1214bd-2772-48dc-81a9-474f6c58d589" alt="HomePage" width="%100"/>
            </td>
        </tr>
    </table>
</div>
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

### Development Tools
- **Visual Studio 2022**: Primary IDE
- **Git**: Version control
- **Azure DevOps**: CI/CD pipeline

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

## 🚀 Getting Started

### Prerequisites
- .NET Core 9.0 SDK
- Visual Studio 2022
- SQL Server 2019 or later
