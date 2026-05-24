# Peng — Core Base System

## Project Overview

Peng is a Modular Monolith base system built with .NET 10 + Vue 3. It provides authentication, authorization, and a clean architectural foundation for spinning up new projects quickly.

## Tech Stack

| Layer | Technology |
|---|---|
| Backend framework | .NET 10 (ASP.NET Core, Minimal APIs) |
| Architecture | Modular Monolith |
| ORM | EF Core 10 + Npgsql (PostgreSQL) |
| CQRS / Mediator | MediatR 14 |
| Validation | FluentValidation 12 |
| Auth | JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) |
| Mapping | Mapster 10 |
| Logging | Serilog (Console + File sinks) |
| API Docs | Scalar (OpenAPI) |
| Password hashing | BCrypt.Net-Next |
| Frontend | Vue 3 + TypeScript + Vite 8 |
| State | Pinia |
| HTTP client | Axios |
| CSS | TailwindCSS v4 |
| Containerization | Docker + Docker Compose |
| CI/CD | GitHub Actions |

## Project Structure

```
Peng/
├── src/
│   ├── SharedKernel/Peng.SharedKernel/      # Base classes, interfaces, Result pattern
│   ├── Modules/
│   │   └── Identity/
│   │       ├── Peng.Modules.Identity.Domain/          # Entities, repository interfaces
│   │       ├── Peng.Modules.Identity.Application/     # Commands, queries, validators, DTOs
│   │       └── Peng.Modules.Identity.Infrastructure/  # EF Core, JWT, BCrypt, DI wiring
│   └── Bootstrapper/Peng.API/               # Entry point, middleware, endpoint mapping
├── tests/
│   ├── Peng.Tests.Unit/                     # Unit tests (no DB)
│   └── Peng.Tests.Integration/              # Integration tests (Testcontainers)
├── frontend/                                # Vue 3 app
├── docker-compose.yml
└── .github/workflows/ci.yml
```

## Key Patterns

### Adding a New Module

1. Create three projects: `Peng.Modules.YourModule.Domain`, `.Application`, `.Infrastructure`
2. Add project references following the same chain as Identity
3. Implement `IModuleInstaller` in Infrastructure — registers all DI services
4. Implement `IModuleDescriptor` in Application — self-documents the module
5. Register in `ServiceCollectionExtensions.AddModules()`
6. Add endpoint groups implementing `IEndpointGroup` in the API project

### Command / Query pattern

Every feature follows CQRS via MediatR:
- Commands: `ICommand<TResponse>` → `ICommandHandler<TCommand, TResponse>`
- Queries: `IQuery<TResponse>` → `IQueryHandler<TQuery, TResponse>`
- Validation: `AbstractValidator<TCommand>` — auto-runs via `ValidationBehavior` pipeline

### Result pattern

All handlers return `Result<T>` — never throw for business errors:
```csharp
// Return success
return Result.Success(new MyDto(...));

// Return failure
return Result.Failure<MyDto>(Error.NotFound("User"));
return Result.Failure<MyDto>(Error.Conflict("Email"));
```

### Permissions

Permissions are string constants defined per module:
```csharp
// Peng.Modules.YourModule.Application/YourModulePermissions.cs
public static class YourModulePermissions
{
    public const string ItemsRead = "yourmodule:items:read";
    public const string ItemsWrite = "yourmodule:items:write";
}
```

Check permissions in endpoints:
```csharp
.RequireAuthorization(policy => policy.RequireClaim("permission", YourModulePermissions.ItemsRead));
```

Check in Vue:
```typescript
const { can } = usePermission()
if (can('yourmodule:items:read')) { ... }
```

### Module Documentation

Every module must implement `IModuleDescriptor` to appear in `/api/docs/modules`. This auto-generates business rule documentation readable by all team members.

## Common Commands

### Backend

```bash
# Run API (requires PostgreSQL running)
dotnet run --project src/Bootstrapper/Peng.API

# Add a new EF Core migration (from solution root)
dotnet ef migrations add <Name> \
  --project src/Modules/Identity/Peng.Modules.Identity.Infrastructure \
  --startup-project src/Bootstrapper/Peng.API \
  --output-dir Persistence/Migrations

# Run tests
dotnet test

# Build
dotnet build
```

### Frontend

```bash
cd frontend
npm run dev          # Dev server at http://localhost:5173
npm run build        # Production build
npm run type-check   # TypeScript check
npm run lint         # ESLint
```

### Docker (local development)

```bash
# First time setup
cp .env.example .env   # Edit .env with your secrets

# Start all services (API + Frontend + PostgreSQL)
docker compose up -d

# Rebuild after code changes
docker compose up -d --build

# View API logs
docker compose logs -f api
```

## Environment Variables

| Variable | Description |
|---|---|
| `ConnectionStrings__Default` | PostgreSQL connection string |
| `Jwt__SecretKey` | JWT signing key (min 32 chars) |
| `Jwt__Issuer` | Token issuer (default: `peng-api`) |
| `Jwt__Audience` | Token audience (default: `peng-client`) |
| `AllowedOrigins__0` | CORS allowed origin |

## API Endpoints

| Method | Path | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | No | Register new user |
| POST | `/api/auth/login` | No | Login, get JWT |
| GET | `/api/users/me` | Yes | Get current user |
| GET | `/api/users` | `identity:users:read` | List users (paginated) |
| GET | `/api/docs/modules` | No | Module documentation |
| GET | `/scalar/v1` | No | Interactive API docs (dev only) |

## Default Roles

| Role | Description |
|---|---|
| Admin | Full access to all permissions |
| Member | Default role for new registrations (no permissions by default) |

## Adding a New Feature (Checklist)

- [ ] Define entities in `.Domain`
- [ ] Define repository interfaces in `.Domain`
- [ ] Add permission constants in `.Application/YourModulePermissions.cs`
- [ ] Write Command/Query + Handler + Validator in `.Application`
- [ ] Update `IModuleDescriptor` with the new feature's business rules
- [ ] Implement repositories in `.Infrastructure`
- [ ] Add EF Core configuration in `.Infrastructure/Persistence/Configurations`
- [ ] Create migration
- [ ] Add endpoint group in `Peng.API/Endpoints`
- [ ] Add route in frontend `src/router/index.ts`
- [ ] Add API method in `src/api/`
- [ ] Add view/component in `src/views/` or `src/modules/`
- [ ] Write unit tests for command/query handlers
- [ ] Write integration tests for the endpoints
