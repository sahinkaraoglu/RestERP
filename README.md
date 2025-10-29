# RestERP - Restaurant Management System

RestERP is a comprehensive Enterprise Resource Planning (ERP) system developed for restaurant businesses. This application is built with **.NET Core 9.0** and follows Clean Architecture principles.

<div align="center">
    <table>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/14fcac76-30d6-4b66-a919-d73a881c8e60" alt="mainpage" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/4a868afa-651f-49c2-9358-d9650db4b7b2" alt="management" width="400"/>
            </td>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/74c4cadd-49ca-43e6-8060-00a5ba1af2b2" alt="menu" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/8db0fe5f-c4d0-41a5-abc3-c5aa52bf1d85" alt="login" width="400"/>
            </td>
        </tr>
    </table>
</div>

## Project Structure

The project has a layered architecture in line with Clean Architecture principles:

| Layer | Description |
|-------|-------------|
| **RestERP.Core** | Core entities, enum values, and domain logic |
| **RestERP.Application** | Contains application services, validation rules, and business logic |
| **RestERP.Infrastructure** | Contains database connection, repository implementations, and external services |
| **RestERP.Web** | Contains user interface, controllers, and views |
| **RestERP.API** | Contains RESTful API controllers with Swagger/OpenAPI documentation |

## 🛠️ Technologies

### Core Framework
- **.NET Core 9.0**: Cross-platform framework
- **ASP.NET Core MVC**: Web application framework
- **Entity Framework Core**: ORM (Object-Relational Mapping) tool
- **SQL Server**: Database
- **Repository Pattern & Unit of Work**: Data access layer pattern
- **In-Memory Caching**: Caching for frequently accessed data
- **JWT Authentication**: Token-based authentication for API and Web applications

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

## 👥 Person Roles

The system defines the following employee roles:
- Customer
- Person

## 📊 Order Statuses

The following statuses are defined for orders:
- New
- InProgress
- Completed
- Cancelled

## 🔄 Repository Pattern & Unit of Work

The project uses Generic Repository and Unit of Work pattern, providing:
- A standard interface for database operations
- Prevention of code duplication
- Increased testability
- Simplified transaction management
- **Automatic Migration**: The application automatically applies database migrations on startup

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
