# Multi-Database .NET Web App implementation with Entity Framework Core

## Purpose
This project demonstrates the implementation of a .NET web application that interacts with multiple databases using Entity Framework Core. The primary goal is to showcase how a .NET application can manage relationships across different databases and provide a clean, maintainable code structure that adheres to best practices and the SOLID principles.

## Problem
In modern enterprise applications, it is not uncommon to have data spread across multiple databases. For instance, a company might store user data in one database, product information in another, and transactions in a third. Managing relationships and ensuring data integrity across these databases can be challenging, particularly when using an ORM like Entity Framework Core, which traditionally assumes a single database context.

## Solution Overview
This project provides a comprehensive solution to this problem by:
- Implementing a three-tier architecture (Presentation, Service, and Data layers) to ensure clean separation of concerns.
- Utilizing Entity Framework Core to interact with multiple databases.
- Providing a mechanism to handle relationships across different databases outside of EF Core's foreign key constraints.

## Features
- **User Management**: CRUD operations for users, including viewing associated transactions.
- **Product Management**: CRUD operations for products, categorized into various categories.
- **Transaction Management**: CRUD operations for transactions, ensuring integrity by verifying the existence of related users and products.
- **Category Management**: CRUD operations for product categories.

## Project Structure
The project is divided into three main layers:
1. **Entities (Data Layer)**:
   - Contains the data models and DbContext configurations.
   - Manages the database interactions using EF Core.

2. **Services (Service Layer)**:
   - Implements business logic and data manipulation.
   - Includes a unit of work and repository pattern to abstract data access.
   - Handles mapping of data transfer objects (DTOs).

3. **WebUI (Presentation Layer)**:
   - ASP.NET Core Blazor application for the user interface.
   - Includes pages for managing users, products, transactions, and categories.
   - Handles form validation and user interactions.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (or later)

### Installation
1. **Clone the repository**:
   ```sh
   git clone https://github.com/yourusername/multi-database-efcore-app.git
   cd multi-database-efcore-app
   ```

2. **Configure the databases**:
   - Update the connection strings in `appsettings.json` for the WebUI project to point to your SQLite databases.

3. **Run the application**:
     - Open the solution in Visual Studio.
     - Set `WebUI` as the startup project and run.

## Conclusion
This project provides a robust solution for managing a .NET application with multiple databases using Entity Framework Core. By following the provided steps, you can deploy a scalable and maintainable web application that demonstrates best practices and effective use of modern technologies. 

For any questions or contributions, please refer to the issues section of the repository.

See the live demo here: [Live Demo](https://blazorwebui20240617152704.azurewebsites.net/)
