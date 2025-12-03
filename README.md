# Firmness - Construction and Rental Management System

## Overview

Firmness is a comprehensive web application designed to manage the sale of construction materials and vehicle rentals. The system consists of a RESTful API built with ASP.NET Core and a modern frontend developed with Vue.js.

## Main Features

- **Product Management:** Full CRUD for products.
- **Customer Management and Authentication:** User registration, login, and role-based authentication (Admin, Client) using JWT.
- **Purchase Process:** Shopping cart and checkout flow to create sales.
- **Email Notifications:** Automatic email sending for registration and purchase confirmations.
- **API Documentation:** Endpoints documented and ready to test via Swagger.

## Architecture and Technologies

- **Backend (API):**
  - **Framework:** ASP.NET Core 8
  - **Database:** PostgreSQL
  - **Authentication:** ASP.NET Core Identity with JSON Web Tokens (JWT)
  - **Object Mapping:** AutoMapper
  - **Unit Testing:** xUnit and Moq

- **Frontend (Client):**
  - **Framework:** Vue.js 3 (with Vite)
  - **State Management:** Pinia
  - **Routing:** Vue Router
  - **HTTP Requests:** Axios

- **Containerization:**
  - Docker and Docker Compose

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js and npm](https://nodejs.org/) (v18 or higher)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (Optional, for containerized execution)

## Installation and Execution

### 1. Local Execution

**a) Start the Database (if using Docker for the DB):**

```sh
# From the project root
docker-compose up -d db
```

**b) Start the Backend API:**

```sh
# Navigate to the API folder
cd FIrmnessAPI

# Restore dependencies and run the API
dotnet run
```
The API will be available at `http://localhost:5000` and `https://localhost:5001`.
Swagger UI will be at `http://localhost:5000/swagger`.

**c) Start the Frontend:**

```sh
# Navigate to the client folder
cd firmness-client

# Install dependencies
npm install

# Run the development server
npm run dev
```
The client application will be available at `http://localhost:5173`.

### 2. Execution with Docker Compose (Full)

This method starts the database, the API, and the client, all in containers.

```sh
# From the project root
docker-compose up --build
```

- **Frontend:** `http://localhost:5173`
- **API (Swagger):** `http://localhost:5000/swagger`

## Project Structure

```
/
├── FIrmnessAPI/         # API Project (ASP.NET Core)
├── firmness-client/     # Client Project (Vue.js)
├── Firmness.Core/      # Shared business logic, models, and services
├── Firmness.Tests/        # Unit tests
├── Firmness.ViewModels/   # Shared DTOs and ViewModels
├── Firmness.Web/          # Original project (Razor Pages)
├── docker-compose.yml   # Container orchestration
└── README.md            # This file
```

## Main API Endpoints

All endpoints (except `login` and `register`) require JWT authentication.

- `POST /api/Auth/register`: Registers a new user.
- `POST /api/Auth/login`: Authenticates and provides a JWT.
- `GET /api/Products`: Gets the list of all products.
- `POST /api/Sales`: Creates a new sale from the items in the cart.
```