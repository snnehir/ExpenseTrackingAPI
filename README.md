# ExpenseTrackingAPI

## Project Topic

ExpenseTrackingAPI is a backend API for tracking personal expenses, designed to meet specific requirements for managing financial transactions. The API provides CRUD operations for managing expenses and users, calculates total expenses per user, and includes features for authentication, logging, scheduled background jobs, and performance optimization. The API is built using .NET Core.

## Project Structure

The project follows an N-tier architecture, separating concerns into different layers:

- **WebAPI Layer**: Contains API controllers for handling HTTP requests and responses.
- **Application Layer**: Contains business logic and service classes for handling operations related to expenses and users.
- **Data Access Layer**: Includes the Entity Framework Core context, repositories, and database migrations.
- **Domain Layer**: Defines the data models for the application, including `User` and `Expense`.

## Used Technologies
- **Logging Middlewares**: Custom middleware for request logging using Serilog.
- **Jobs**: Scheduled background jobs using Hangfire for aggregating user expenses
- **Mapster**: A high-performance object mapping library used to map between data transfer objects (DTOs) and entities.
- **Jwt Authentication**: JSON Web Token (JWT) based authentication for securing the API endpoints and verifying user identities.

## Features

- **RESTful API**: Exposing services via RESTful endpoints for easy integration with other systems.
- **CRUD Operations**: Create, Read, Update, and Delete operations for managing expenses and users.
- **Authentication**: JWT-based authentication to ensure secure access to the API.
- **Background Jobs**: Scheduled jobs using Hangfire to aggregate user expenses on a daily, weekly, and monthly basis.
- **Logging**: Request logging using Serilog for better monitoring and debugging.
- **Documentation**: API documentation generated using Swagger.

## Getting Started

1. **Clone the repository:**
   ```bash
   git clone https://github.com/yourusername/ExpenseTrackingAPI.git
   cd ExpenseTrackingAPI
   ```
  
2. **Run Redis container for refresh token mechanism:**
    ```bash
    docker run --name expense-redis -d -p 6379:6379 redis
    ```

3. **Access the API documentation:**
Navigate to https://localhost:7168/swagger to view and interact with the API documentation.

4. **View the Hangfire Dashboard:**
Navigate to https://localhost:7168/hangfire to view Hangfire dashboard.
