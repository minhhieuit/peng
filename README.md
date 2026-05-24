# Peng — Core Base System

> Hệ thống nền tảng (Modular Monolith) dùng để khởi tạo nhanh các dự án mới với authentication, authorization và kiến trúc sạch có sẵn.

## Tech Stack

| | Technology |
|---|---|
| **Backend** | .NET 10 · ASP.NET Core Minimal APIs |
| **Architecture** | Modular Monolith |
| **Database** | PostgreSQL 17 · EF Core 10 |
| **CQRS** | MediatR 14 |
| **Validation** | FluentValidation 12 |
| **Auth** | JWT Bearer · BCrypt |
| **Logging** | Serilog (Console + File) |
| **API Docs** | Scalar (OpenAPI) |
| **Frontend** | Vue 3 · TypeScript · Vite 8 |
| **State** | Pinia |
| **CSS** | TailwindCSS v4 |
| **CI/CD** | GitHub Actions |
| **Container** | Docker · Docker Compose |

---

## Tính năng

- **Đăng ký / Đăng nhập** — JWT Bearer, BCrypt password hashing
- **Phân quyền Hybrid RBAC** — Role-based + resource-level permission check
- **Module tự đăng ký** — mỗi module implement `IModuleInstaller` và `IModuleDescriptor`
- **Auto documentation** — endpoint `/api/docs/modules` liệt kê toàn bộ business rules của từng module
- **Result pattern** — không throw exception cho business errors, xử lý lỗi nhất quán
- **Auto migrate + seed** — database tự migrate và seed roles/permissions khi khởi động
- **Structured logging** — Serilog với log ra console và file theo ngày

---

## Cấu trúc dự án

```
Peng/
├── src/
│   ├── SharedKernel/
│   │   └── Peng.SharedKernel/          # Entity, Result<T>, CQRS interfaces, ValidationBehavior
│   ├── Modules/
│   │   └── Identity/
│   │       ├── Domain/                 # Entities, repository interfaces, domain events
│   │       ├── Application/            # Commands, queries, validators, DTOs, permissions
│   │       └── Infrastructure/         # EF Core, JWT, BCrypt, DI registration
│   └── Bootstrapper/
│       └── Peng.API/                   # Entry point, middleware, endpoint mapping, Scalar docs
├── tests/
│   ├── Peng.Tests.Unit/                # Unit tests (không cần DB)
│   └── Peng.Tests.Integration/         # Integration tests (Testcontainers)
├── frontend/                           # Vue 3 app
│   └── src/
│       ├── api/                        # Axios API clients
│       ├── stores/                     # Pinia stores
│       ├── composables/                # usePermission()
│       ├── layouts/                    # AppLayout với permission-aware nav
│       ├── router/                     # Route guards (auth, guest, permission)
│       └── views/                      # Login, Register, Dashboard, Users, 404
├── .github/workflows/ci.yml            # CI: build, test, lint, type-check
├── docker-compose.yml
└── CLAUDE.md                           # Hướng dẫn cho AI assistant
```

---

## Bắt đầu nhanh

### Yêu cầu

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org)
- [PostgreSQL 17](https://www.postgresql.org/) hoặc [Docker](https://www.docker.com/)

### Chạy với Docker (khuyên dùng)

```bash
# 1. Clone repo
git clone git@github.com:minhhieuit/peng.git
cd peng

# 2. Cấu hình môi trường
cp .env.example .env
# Sửa .env: điền POSTGRES_PASSWORD và JWT_SECRET_KEY

# 3. Khởi động toàn bộ hệ thống
docker compose up -d

# API:      http://localhost:8080
# Frontend: http://localhost:80
# Docs:     http://localhost:8080/scalar/v1
```

### Chạy local (development)

```bash
# Terminal 1 — Backend
dotnet run --project src/Bootstrapper/Peng.API
# API: http://localhost:5159
# Docs: http://localhost:5159/scalar/v1

# Terminal 2 — Frontend
cd frontend
npm install
npm run dev
# UI: http://localhost:5173
```

> Database sẽ tự động migrate và seed dữ liệu (roles, permissions) khi API khởi động lần đầu.

---

## API Endpoints

| Method | Path | Auth | Mô tả |
|---|---|---|---|
| `POST` | `/api/auth/register` | — | Đăng ký tài khoản mới |
| `POST` | `/api/auth/login` | — | Đăng nhập, nhận JWT |
| `GET` | `/api/users/me` | Bearer | Thông tin user hiện tại |
| `GET` | `/api/users` | `identity:users:read` | Danh sách users (phân trang) |
| `GET` | `/api/docs/modules` | — | Tài liệu modules + business rules |
| `GET` | `/scalar/v1` | — | Interactive API docs (dev only) |

---

## Phân quyền

Hệ thống dùng **Hybrid RBAC**: Role-based ở cấp module + resource-level check.

```
User ──► Roles ──► Permissions
                    identity:users:read
                    identity:users:write
                    identity:roles:read
                    ...
```

**Roles mặc định:**

| Role | Mô tả |
|---|---|
| `Admin` | Toàn quyền, có tất cả permissions |
| `Member` | Role mặc định khi đăng ký, chưa có permission |

**Kiểm tra permission trong API:**
```csharp
.RequireAuthorization(policy => policy.RequireClaim("permission", IdentityPermissions.UsersRead));
```

**Kiểm tra permission trong Vue:**
```typescript
const { can } = usePermission()
if (can('identity:users:read')) { /* ... */ }
```

---

## Thêm module mới

1. Tạo 3 projects: `Peng.Modules.YourModule.Domain`, `.Application`, `.Infrastructure`
2. Implement `IModuleInstaller` → đăng ký DI services
3. Implement `IModuleDescriptor` → tự document business rules
4. Đăng ký trong `ServiceCollectionExtensions.AddModules()`
5. Thêm endpoint groups implement `IEndpointGroup`
6. Tạo EF migration, thêm view/route ở frontend

Xem checklist đầy đủ trong [`CLAUDE.md`](./CLAUDE.md).

---

## CI/CD

GitHub Actions tự động chạy khi push lên `main` / `develop`:

```
push → Build .NET → Unit tests → Integration tests
     → Type-check → ESLint → Frontend build
     → Docker build (chỉ trên main)
```

---

## Biến môi trường

| Biến | Mô tả |
|---|---|
| `ConnectionStrings__Default` | PostgreSQL connection string |
| `Jwt__SecretKey` | JWT signing key (tối thiểu 32 ký tự) |
| `Jwt__Issuer` | Token issuer (mặc định: `peng-api`) |
| `Jwt__Audience` | Token audience (mặc định: `peng-client`) |
| `AllowedOrigins__0` | CORS allowed origin |

---

## License

MIT
