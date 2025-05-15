# RestERP - *Coming Soon!*

RestERP is a comprehensive Enterprise Resource Planning (ERP) system developed for restaurant businesses. This application is an ASP.NET Core-based web application developed in accordance with Clean Architecture principles.

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

### Product Management
- Add, edit, and categorize products
- Price management

### Customer Management
- Create and edit customer records
- View customer order history

### Employee Management
- Employee records and role assignments

### Table Management
- Table status tracking
- Table-based order viewing


Basic entities used in the project:

- **Category**: Product categories
- **Customer**: Customer information
- **Employee**: Employee information
- **Order**: Order information
- **OrderItem**: Order items
- **Product**: Product information
- **Table**: Table information

All entities are derived from the `BaseEntity` class and basically contain the following properties:
- Id
- IsDeleted
- CreatedById, CreatedDate
- UpdatedById, UpdatedDate

### Repository Pattern

The project uses Generic Repository and Unit of Work pattern. This provides:

- A standard interface for database operations
- Prevention of code duplication
- Increased testability
- Simplified transaction management
