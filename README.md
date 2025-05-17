# RestERP - *Coming Soon!*

RestERP is a comprehensive Enterprise Resource Planning (ERP) system developed for restaurant businesses. This application is an ASP.NET Core-based web application developed in accordance with Clean Architecture principles.

<div align="center">
[![.NET Core](https://img.shields.io/badge/-.NET%20Core%208.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/-EF%20Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)](https://docs.microsoft.com/ef/core/)
</div>

<div align="center">
   <div align="center">
    <table>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/442c3b80-6ec3-4361-a92b-eeba9d05cfa1" alt="HomePage" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/fb6a0911-d1f6-4ab6-bda0-20864dcb230f" alt="Menu" width="400"/>
            </td>
        </tr>
        <tr>
            <td>
                <img src=https://github.com/user-attachments/assets/6430fa8b-d8a6-4750-97af-45b6d45a37db" alt="Login" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/bfa94f86-088c-44a1-aa07-d21fd3fa05e1" alt="Management" width="400"/>
            </td>
        </tr>
    </table>
</div>
</div>


## Project Structure

The project has a layered architecture in line with Clean Architecture principles:

- **RestERP.Domain**: Contains core entities, enum values, and domain logic.
- **RestERP.Application**: Contains application services, validation rules, and business logic.
- **RestERP.Infrastructure**: Contains database connection, repository implementations, and external services.
- **RestERP.Web**: Contains user interface, controllers, and views.

## Technologies

- **ASP.NET Core MVC**: Web application framework
- **Entity Framework Core**: ORM (Object-Relational Mapping) tool
- **SQL Server**: Database
- **Repository Pattern & Unit of Work**: Data access layer pattern

## Core Features

### Order Management
- Create, edit, and track orders
- Monitor order status (New, In Progress, Completed, Cancelled)
- Table-based order management

### Food Management
- Add, edit, and categorize food items
- Price management

### Customer Management
- Create and edit customer records
- View customer order history

### Employee Management
- Employee records and role assignments

### Table Management
- Table status tracking
- Table-based order viewing

## Basic Entities Used in the Project

- **FoodCategory**: Food categories
- **Food**: Food information
- **Customer**: Customer information
- **Employee**: Employee information
- **Order**: Order information
- **OrderItem**: Order items
- **Table**: Table information

All entities are derived from the `BaseEntity` class and basically contain the following properties:
- Id (int): Unique identifier
- IsDeleted (bool): Deletion status
- CreatedById (long?), CreatedDate (DateTime?): Creator user and date
- UpdatedById (long?), UpdatedDate (DateTime?): Updater user and date

## Employee Roles
The system defines the following employee roles:
- Manager
- Chef
- Waiter
- Cashier

## Order Statuses
The following statuses are defined for orders:
- New
- InProgress
- Completed
- Cancelled

### Repository Pattern
The project uses Generic Repository and Unit of Work pattern. This provides:
- A standard interface for database operations
- Prevention of code duplication
- Increased testability
- Simplified transaction management
