# User API with JWT Authentication

This is a simple Web API built with ASP.NET Core.
It allows:
- Registering users
- Logging in and generating JWT tokens
- Viewing a protected list of users

## How to run
1. Clone the repo
2. Open in Visual Studio
3. Press F5 or run the project

Use Postman to test endpoints like:
- POST /api/login
- GET /api/users (requires Bearer token)
