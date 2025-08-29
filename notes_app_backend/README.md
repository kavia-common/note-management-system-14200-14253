# Notes App Backend (ASP.NET Core 8)

This is a ready-to-run Web API providing:
- User registration and login with JWT authentication
- CRUD for notes (owned by user)
- Swagger UI at /docs

## Run

- dotnet restore
- dotnet run
- Open http://localhost:3001/docs for API docs

## Auth

1. Register: POST /api/auth/register
2. Login: POST /api/auth/login -> copy token
3. Authorize in Swagger: click Authorize and enter `Bearer {token}`

## Environment/Configuration

- Add .env variables (or appsettings) for JWT if deploying to production (see .env.example).
- Currently uses EF Core InMemory provider for development.

## Endpoints

- GET / -> { "message": "Healthy" }
- POST /api/auth/register
- POST /api/auth/login
- GET /api/auth/me
- CRUD /api/notes (Authorized)

