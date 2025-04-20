AuthandProductCRUD API
A RESTful API built with ASP.NET Core that provides JWT authentication and CRUD operations for products. This application uses MongoDB as its database.
Features
	User registration and login with JWT authentication
	Role-based authorization
	Product management (Create, Read, Update, Delete)
	MongoDB integration for data persistence

Prerequisites
	.NET 9 SDK
	MongoDB installed and running on localhost:27017 
Required NuGet Packages
    Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
    Install-Package Microsoft.IdentityModel.Tokens
    Install-Package System.IdentityModel.Tokens.Jwt
    Install-Package MongoDB.Driver


Configuration
The application uses the following configuration in appsettings.json:
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "MongoDbSettings": {
        "ConnectionString": "mongodb://localhost:27017/",
        "DatabaseName": "AuthAndProductCRUD"
    },
    "Jwt": {
        "Key": "YourSecureKey32CharactersLong1234567890",
        "Issuer": "https://localhost:7063",
        "Audience": "https://localhost:7063"
    },
    "AllowedHosts": "*"
}



Project Structure
Controllers/ 
	AuthController.cs: Handles user registration and login
	ProductController.cs: Manages product CRUD operations
Models/ 
	User.cs: User data model
	LoginModel.cs: Login request model
	Product.cs: Product data model
Services/ 
	UserService.cs: User data access service
	ProductService.cs: Product data access service
	MongoDbSettings.cs: MongoDB configuration

Getting Started
1.	Clone the repository
2.	Ensure MongoDB is running
3.	Update the connection string in appsettings.json 
4.	Run the application:
dotnet run
The API will be available at https://localhost:7063 by default 



API Endpoints
Authentication
Register User
	POST /api/Auth/register
	Body: { "username": "vishnu", "password": "7788", "role": "Admin" }
	Body: { "username": "karu", "password": "7788", "role": "User" }

Login
	POST /api/Auth/login
	Body: { "username": " vishnu ", "7788": " Admin" }
	Returns: JWT token


Products 
All product endpoints require JWT authentication. Admin role is required for Create, Update, and Delete operations.

Get All Products
	GET /api/Product
	Authentication: JWT token in Authorization header

Get Product by ID
	GET /api/Product/{id}

Create Product (Admin only)
	POST /api/Product
	Body: { "name": "Product Name", "description": "Product Description" }

Update Product (Admin only)
	PUT /api/Product/{id}
	Body: { "name": "Updated Name", "description": "Updated Description" }

Delete Product (Admin only)
	DELETE /api/Product/{id}

Authentication
To authenticate, first make a login request to get a JWT token. Then include this token in the Authorization header of subsequent requests:
Authorization: Bearer <Token>
