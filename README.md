# Cat API Project

## Description

This is an ASP.NET Core Web API application that "steals" cat images from the [The Cat API](https://thecatapi.com/) and stores them in a SQL Server database. It includes endpoints for retrieving cat images, managing pagination, and searching by tags.

## Prerequisites

- .NET 8 SDK
- Microsoft SQL Server
- Visual Studio 2022 or any IDE that supports .NET 8
- Postman or any HTTP request tool

## Installation Instructions

### Step 1: Clone the Repository

### Step 2: Set Up the Database
- Ensure that you have Microsoft SQL Server installed.
- Update appsettings.json with the correct database connection settings.
- Run the database migrations. (BASH --> dotnet ef database update OR VISUAL STUDIO 2022 --> (Tools -> NuGet Package Manager -> Package Manager Console -> Update-Database))
### Step 3: Set Up API Key
- To use the Cat API, you'll need an API Key. You can obtain one from [here](https://thecatapi.com/signup)).
- Add the API Key to appsettings.json in the Token field.

## API Usage

### 1. Fetch 25 Cat Images and Store

**Endpoint:** `POST /api/cats/fetch`

This endpoint calls the Cat API to fetch 25 cat images and stores them in the database.

### 2. Retrieve a Cat by ID

**Endpoint:** `GET /api/cats/{id}`

This endpoint provides information about a cat from the database based on the id.

### 3. Retrieve Cats with Paging Support

**Endpoint:** `GET /api/cats?page={page}&pageSize={pageSize}`

This endpoint returns a list of cats with paging support.

### 4. Retrieve Cats with Specific Tag And Paging Support

**Endpoint:** `GET /api/cats?tag={tag}&page={page}&pageSize={pageSize}`

This endpoint returns cats that have a specific tag with paging support.

## Swagger
- API documentation is available at the /swagger endpoint: https://localhost:{Your_Port}/swagger/v1/swagger.json
