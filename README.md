# Linh-Long ERP

Linh-Long ERP is a minimal full-stack sample project demonstrating clean layering, authentication (JWT + Refresh Token), and a React client. This repository is a coding-challenge style reference implementation showing practical patterns for a small enterprise app.

## Key features

- Clean architecture with layered projects (API, Application, Domain, Infrastructure)
- Authentication using ASP.NET Identity, JWT access tokens and refresh tokens
- CQRS-style organization with MediatR, FluentValidation and AutoMapper
- React frontend built with Vite + TypeScript
- Works with SQL Server (production) or SQLite (easy local dev)

## Tech stack

- Backend: ASP.NET Core 8, Entity Framework Core, ASP.NET Identity, MediatR, FluentValidation, AutoMapper, Serilog
- Frontend: React (Vite + TypeScript), Axios, Zustand (auth state)
- Database: SQL Server (recommended) or SQLite (recommended for quick demos)

## Repository layout

The repository contains a solution and two top-level folders for backend and frontend:

LinhLongERP/
	- LinhLongApi/             # ASP.NET Core solution and projects
		- LinhLong.Api/          # Web API (controllers, DI, middleware)
		- LinhLong.Application/  # Commands, Queries, Handlers, DTOs, Validators
		- LinhLong.Domain/       # Entities, value objects, domain rules
		- LinhLong.Infrastructure/# EF Core, Identity, Repositories, JWT, Migrations
	- LinhLongApp/             # React + Vite + TypeScript frontend

Paths in commands below assume you run them from the repository root.

## Prerequisites

- Git
- Node.js >= 18 and npm >= 9
- .NET SDK 8.x
- A database: SQL Server (local or container) or SQLite (recommended for quick local testing)

## Configuration / Environment variables

Backend config is in `LinhLongApi/appsettings.Development.json` (or via user secrets / environment variables). Pick one connection string: SQL Server or SQLite.

Example appsettings snippet:

```json
{
	"ConnectionStrings": {
		// SQL Server example (uncomment if used)
		// "Default": "Server=localhost,1433;Database=LinhLong;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;",

		// SQLite (simple dev)
		"Default": "Data Source=linhlong.db"
	},
	"Jwt": {
		"Issuer": "LinhLong.Issuer",
		"Audience": "LinhLong.Audience",
		"Key": "REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY"
	},
	"AllowedHosts": "*"
}
```

Or set environment variables (example, PowerShell):

```powershell
$env:ConnectionStrings__Default = 'Data Source=linhlong.db'
$env:Jwt__Issuer = 'LinhLong.Issuer'
$env:Jwt__Audience = 'LinhLong.Audience'
$env:Jwt__Key = 'REPLACE_WITH_A_LONG_RANDOM_SECRET_KEY'
```

Frontend expects a Vite env var in `LinhLongApp/.env`:

```
VITE_API_BASE_URL=https://localhost:5161
```

## Quick start (local)

Follow these steps from the repository root. Commands assume PowerShell on Windows.

1) Backend

```powershell
# Restore projects
dotnet restore

# Restore dotnet tools (if the repo uses EF CLI tools)
dotnet tool restore

# Apply EF Core migrations (points to the Infrastructure project and uses Api as startup)
dotnet ef database update -p ./LinhLong.Infrastructure -s ./LinhLong.Api

# Run the API
dotnet run --project ./LinhLong.Api
```

Default API base URL (development): https://localhost:5161

Swagger will be available at: https://localhost:5161/swagger

2) Frontend

Open a new terminal and run:

```powershell
cd LinhLongApp
# If an example env exists, copy it; otherwise create .env with VITE_API_BASE_URL
Copy-Item .env.example .env -ErrorAction SilentlyContinue
npm install
npm run dev
```

Frontend dev server default: https://localhost:5174 (reads `VITE_API_BASE_URL` to call the API)

## Common commands

Backend

```powershell
# Create a new migration (edit the name)
dotnet ef migrations add InitialIdentity -p ./LinhLong.Infrastructure -s ./LinhLong.Api -o Data/Migrations

# Update the database
dotnet ef database update -p ./LinhLong.Infrastructure -s ./LinhLong.Api

# Run the API
dotnet run --project ./LinhLong.Api
```

Frontend

```powershell
# Dev server
npm run dev

# Type-check, lint & format (if configured)
npm run lint
npm run format

# Production build & preview
npm run build
npm run preview
```

## Authentication (brief)

- The frontend sends POST /auth/login with user credentials.
- The API validates the user via ASP.NET Identity and returns an access token (JWT) and a refresh token.
- The frontend stores tokens in memory (Zustand) and Axios attaches the Authorization header: `Bearer <access_token>`.
- On 401 due to expiration, the frontend calls POST /auth/refresh-token, receives a new access token and retries the failed request.
- Logout clears tokens client-side and may call an API to invalidate the refresh token server-side.

## Troubleshooting

- 404 on known routes: Confirm the API is running at `https://localhost:5161` and that CORS allows `https://localhost:5174` during development.
- EF Core migrations not found: Ensure `LinhLong.Infrastructure` contains a `Migrations` folder and that you run `dotnet ef` with `-p` and `-s` pointing to the infrastructure and API projects respectively.
- SQL Server container issues: Use a strong SA password meeting complexity requirements.
- CORS issues: Add or adjust a CORS policy in `LinhLong.Api` startup to allow the frontend origin.
- HTTPS/dev certificates: Trust the .NET dev certificate with `dotnet dev-certs https --trust` or switch to HTTP for local-only testing.

## Notes

- Keep secrets (JWT keys, DB passwords) out of source control. Use environment variables or a secret manager.
- For a quick demo, SQLite is simplest since it requires no external DB.
- Swagger provides interactive documentation for the API endpoints and request/response shapes.

---

Prepared by: Dao Hoang Sao – October 2025
