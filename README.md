# Filmify

Filmify is a modular .NET 9 solution for managing films, categories, tags, and curated boxes. It includes a clean architecture with separate Domain, Application, Infrastructure, API, Presentation/UI layers, and a dedicated Identity service.

### Solution structure
- `Filmify.Domain`: Core entities, value objects, and domain contracts
- `Filmify.Application`: Application services, DTOs, mapping, paging/sorting abstractions
- `Filmify.Infrastructure`: EF Core persistence, repositories, migrations, and DI installers
- `Filmify.Api`: ASP.NET Core minimal Web API with Swagger for Filmify
- `Filmify.Presentation`: ASP.NET Core MVC app (server-rendered)
- `Filmify.UI`: ASP.NET Core MVC app (client-facing UI with services/components)
- `IdentityService/*`: Identity API, Application, Domain, and Infrastructure for authentication

All projects target **.NET 9**.

## Prerequisites
- .NET SDK 9.x (`dotnet --info`)
- SQL Server (LocalDB, Developer, or container)
- Optionally: `dotnet-ef` tool for migrations
  - Install: `dotnet tool install -g dotnet-ef`

## Getting started
1) Restore and build
```bash
dotnet restore
dotnet build --configuration Debug
```

2) Configure connection strings
- Update `appsettings.Development.json` or `appsettings.json` in:
  - `Filmify/Filmify.Api/Filmify.Api/`
  - `Filmify/Filmify.Infrastructure/Filmify.Infrastructure/`
  - Identity service projects under `IdentityService/` (or `Filmify/IdentityService/` depending on your checkout)
- Look for `ConnectionStrings` sections and set your SQL Server connection.

3) Apply database migrations
- Filmify main database (Infrastructure)
```bash
# Update DB using API as startup (ensures correct host settings)
dotnet ef database update \
  --project Filmify/Filmify.Infrastructure/Filmify.Infrastructure/Filmify.Infrastructure.csproj \
  --startup-project Filmify/Filmify.Api/Filmify.Api/Filmify.Api.csproj
```

- Identity database (run the one matching your folder layout)
```bash
# If IdentityService is at repo root
dotnet ef database update \
  --project IdentityService/Filmify.Identity.Infrastructure/Filmify.Identity.Infrastructure.csproj \
  --startup-project IdentityService/Filmify.Identity.Api/Filmify.Identity.Api.csproj

# If IdentityService is nested under Filmify/
dotnet ef database update \
  --project Filmify/IdentityService/Filmify.Identity.Infrastructure/Filmify.Identity.Infrastructure.csproj \
  --startup-project Filmify/IdentityService/Filmify.Identity.Api/Filmify.Identity.Api.csproj
```

4) Run the services
```bash
# Filmify Web API (Swagger available at /swagger)
dotnet run --project Filmify/Filmify.Api/Filmify.Api/Filmify.Api.csproj

# Optional: Filmify Presentation MVC app
dotnet run --project Filmify/Filmify.Presentation/Filmify.Presentation/Filmify.Presentation.csproj

# Optional: Filmify UI MVC app
dotnet run --project Filmify/Filmify.UI/Filmify.UI/Filmify.UI.csproj

# Optional: Identity API
# Use the path that matches your checkout
dotnet run --project IdentityService/Filmify.Identity.Api/Filmify.Identity.Api.csproj
```

## API overview
When running `Filmify.Api`, navigate to `/swagger` for interactive docs. Controllers include:
- `FilmsController`
- `CategoryController`
- `TagController`
- `BoxesController`

## Migrations: common commands
Create a new migration in Infrastructure:
```bash
dotnet ef migrations add <Name> \
  --project Filmify/Filmify.Infrastructure/Filmify.Infrastructure/Filmify.Infrastructure.csproj \
  --startup-project Filmify/Filmify.Api/Filmify.Api/Filmify.Api.csproj
```

Update database:
```bash
dotnet ef database update \
  --project Filmify/Filmify.Infrastructure/Filmify.Infrastructure/Filmify.Infrastructure.csproj \
  --startup-project Filmify/Filmify.Api/Filmify.Api/Filmify.Api.csproj
```

Repeat similarly for Identity by switching to its `*.Infrastructure.csproj` and API startup project.

## Configuration
- Environment variables can override settings in `appsettings*.json`.
- Typical keys:
  - `ConnectionStrings__Default`
  - `ASPNETCORE_ENVIRONMENT` (e.g., Development)

## Troubleshooting
- EF Tools not found: install with `dotnet tool install -g dotnet-ef` and restart shell.
- Migrations errors: verify the `--project` points to `Filmify.Infrastructure` and `--startup-project` to `Filmify.Api` (or the Identity equivalents) and that connection strings are valid.
- SQL connectivity: confirm the SQL Server instance is reachable and the database user has permissions.
- Swagger not loading: ensure API is running and browse to `/swagger` on the API’s base URL.

## License
Licensed under the terms in `LICENSE.txt`.
