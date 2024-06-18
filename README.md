# Multi-Database .NET Web App Implementation with Entity Framework Core

## Overview
This project demonstrates the implementation of a .NET web application that interacts with multiple databases using Entity Framework Core. The primary goal is to showcase how a .NET application can manage relationships across different databases and provide a clean, maintainable code structure that adheres to best practices and the SOLID principles.

## Problem
In modern enterprise applications, it is not uncommon to have data spread across multiple databases. For instance, a company might store user data in one database, product information in another, and transactions in a third. Managing relationships and ensuring data integrity across these databases can be challenging, particularly when using an ORM like Entity Framework Core, which traditionally assumes a single database context.

## Solution Overview
This project provides a comprehensive solution to this problem by:
- Implementing a three-tier architecture (Presentation, Service, and Data layers) to ensure clean separation of concerns.
- Utilizing Entity Framework Core to interact with multiple databases.
- Providing a mechanism to handle relationships across different databases outside of EF Core's foreign key constraints.

## Project Structure
The project is divided into three main layers:
1. **Data (Data Layer)**:
   - Contains the data models and DbContext configurations.
   - Manages the database interactions using EF Core.

2. **Services (Service Layer)**:
   - Implements business logic and data manipulation.
   - Includes a unit of work and repository pattern to abstract data access.
   - Handles mapping of data transfer objects (DTOs).

3. **BlazorWebUI (Presentation Layer)**:
   - ASP.NET Core Blazor application for the user interface.
   - Includes pages for managing users, products, transactions, and categories.
   - Handles form validation and user interactions.

## Getting Started
You can debug the app locally to see how it all work. You can view the web application here: [Live Demo](https://blazorwebui20240617152704.azurewebsites.net/), and you can also explore the web API here: [Web API](https://services20240617150841.azurewebsites.net/). Please note that it's possible that the sites are deactivated when you visit it. In that case, please contact me so I can reactivate it.

However, if you'd like to run it locally or if you want to debug it, please continue reading.

### Prerequisites
- .NET 8 SDK
- Visual Studio 2022 (or later)

### Installation
1. **Clone the repository**:
   ```sh
   git clone https://github.com/azayrahmad/3TierAppWithMultiDatabases.git
   cd 3TierAppWithMultiDatabases
   ```

2. **Configure the databases**:
   Run these commands to generate embedded SQLite databases.
   ```sh
   dotnet ef migrations add InitialCreate --project Data --context ProductDbContext --startup-project Services
   dotnet ef migrations add InitialCreate --project Data --context UserDbContext --startup-project Services
   dotnet ef migrations add InitialCreate --project Data --context TransactionDbContext --startup-project Services

   dotnet ef database update --project Data --context ProductDbContext --startup-project Services
   dotnet ef database update --project Data --context UserDbContext --startup-project Services
   dotnet ef database update --project Data --context TransactionDbContext --startup-project Services
   ```
     
3. **Run the application**:
     - Open the solution in Visual Studio.
     - Set `Services` and `BlazorWebUI` as the startup projects and run both of them.
     - Alternatively run these commands each in two different consoles.
      ```sh
      dotnet run --project Services -lp https
      dotnet run --project WebUI -lp https
      ```

If all goes well, you will be greeted with the homepage.

![image](https://github.com/azayrahmad/3TierAppWithMultiDatabases/assets/10110227/af043458-731b-40f5-921e-098862ef6626)

If you run it locally, you also might be able to see the Swagger page for the backend.

![image](https://github.com/azayrahmad/3TierAppWithMultiDatabases/assets/10110227/01fae5f3-084b-4a14-8b70-dfdeff969cfe)



## Conclusion
This project provides a robust solution for managing a .NET application with multiple databases using Entity Framework Core. By following the provided steps, you can deploy a scalable and maintainable web application that demonstrates best practices and effective use of modern technologies. 

For any questions or contributions, please refer to the issues section of the repository.
