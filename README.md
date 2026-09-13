## RestERP — Restaurant Management System

Production-ready, layered restaurant ERP built on ASP.NET Core (.NET 9). The solution provides a clean separation of concerns across API, Web (MVC), Mobile, Application, Infrastructure, and Core domain layers.

### Highlights
- Clean, layered architecture (Core, Application, Infrastructure, API, Web, Mobile)
- EF Core with SQL Server, soft-delete filters, precise money fields
- ASP.NET Core Identity (roles: Admin, Employee, Customer)
- JWT authentication (API + Web cookie bridge + Mobile token storage), Swagger with security scheme
- Autofac-based DI, generic repository + unit of work
- Global logging and exception handling middleware
- Cross-platform mobile client (React Native / Expo) sharing the same REST API

## Architecture
- `RestERP.Core`: Domain entities, enums, base types
- `RestERP.Application`: Service abstractions/implementations and business logic
- `RestERP.Infrastructure`: EF DbContext, migrations, repositories, seed data
- `Services/RestERP.API`: REST API with Swagger, JWT, role policies
- `RestERP.Web`: MVC UI (Areas/Admin), HttpClient to API, auth integration
- `RestERP.Mobile`: React Native (Expo) client for customer and admin workflows over the same API

### Web Screenshots

<div align="center">
    <table>
        <tr>
            <td>
                <img width="1080" height="2400" alt="Screenshot_1789316097" src="https://github.com/user-attachments/assets/d0602d43-c71f-41b6-8cb0-9342343eca13" width="400"/>
            </td>
            <td>
                 <img width="1080" height="2400" alt="Screenshot_1789316585" src="https://github.com/user-attachments/assets/386d6d44-50cf-4f0d-8ff2-9518a9cb4da7" width="400"/>
            </td>
        </tr>
        <tr>
            <td>
               <img width="1080" height="2400" alt="Screenshot_1789316724" src="https://github.com/user-attachments/assets/00ceba68-b452-411d-8f0a-aad7faa628b2"  width="400"/>
            </td>
            <td>
                <img width="1080" height="2400" alt="Screenshot_1789316633" src="https://github.com/user-attachments/assets/7901b0cd-844f-41b0-a78f-6f298c8395ca" width="400"/>
            </td>
        </tr>
    </table>
</div>

### Data Access Patterns
- Generic Repository and Unit of Work are used to standardize data access and transaction boundaries while keeping domain logic clean.

## Tech Stack

### Backend & Web
- .NET 9, ASP.NET Core MVC, Entity Framework Core, SQL Server
- Autofac, ASP.NET Core Identity, JWT Bearer
- Swagger/OpenAPI, MemoryCache, Logging middleware

### Mobile
- React Native 0.86, Expo SDK 57
- React Navigation (stack + bottom tabs)
- TypeScript, AsyncStorage / SecureStore for auth tokens
- Consumes `RestERP.API` at `http://localhost:5050` (food images served via API)

## Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (local or remote)
- Node.js 20+ (for mobile)

### Run the API
```bash
dotnet run --project src/Services/RestERP.API
```
API runs at `http://localhost:5050`.

### Run the Web App
```bash
dotnet run --project src/RestERP.Web
```

### Run the Mobile App
```bash
cd src/RestERP.Mobile
npm install
npm start
```
Open with Expo Go or an emulator.

| Environment | API host |
|---|---|
| Android emulator | `10.0.2.2:5050` |
| iOS simulator / physical device | Same LAN IP as your machine (`src/RestERP.Mobile/src/config.ts`) |

Start the API with the `http` profile so the mobile client can reach it on the local network.

### Test Users (Development)

| Email | Password | Role |
|---|---|---|
| admin@resterp.com | Admin123! | Admin |
| employee@resterp.com | Employee123! | Employee |
| customer@test.com | Customer123! | Customer |

### Mobile Screens

**Customer:** Home, Menu (cart + checkout), Orders, Reservations, Login / Sign Up

**Admin / Employee:** Dashboard, Menu management, Orders, Tables, Users, Reservations, Reports

## Security
- JWT Bearer authentication with zero clock skew
- Role-based policies: `EmployeeOnly`, `CustomerOnly`
- Note: CORS is permissive in development (`AllowAll`). Harden for production.
- Seed passwords in code are for development. Rotate/remove in production.
- Mobile stores JWT tokens locally; use HTTPS and secure storage in production.

## CI/CD

The project uses Azure DevOps Pipelines for continuous integration and deployment.

### Pipeline Configuration

Two separate pipelines are configured for independent deployment:

1. **API Pipeline** (`azure-pipelines-api.yml`)
   - Builds and deploys the REST API project
   - Target: IIS Application Pool `RestERPAPI`
   - Deployment path: `C:\inetpub\publishrestapi`

2. **Web Pipeline** (`azure-pipelines-web.yml`)
   - Builds and deploys the MVC Web application
   - Target: IIS Application Pool `DefaultAppPool`
   - Deployment path: `C:\inetpub\publishrestweb`

### Pipeline Features

- **Trigger**: Automatically runs on pushes to `main` branch
- **Build Stage**:
  - Uses .NET 9 SDK
  - Restores dependencies and publishes projects in Release configuration
  - Creates build artifacts for deployment
- **Deploy Stage**:
  - Deploys to IIS using self-hosted agent pool (`HomePool`)
  - Implements zero-downtime deployment with `app_offline.htm`
  - Stops application pool, cleans deployment folder, copies new files, and restarts the pool

### Setup Requirements

- Self-hosted Azure DevOps agent pool configured with name `HomePool`
- IIS configured with the specified application pools and site paths
- Agent must have permissions to manage IIS (WebAdministration module)

## Notes
- Automatic migration on startup is convenient for local/dev; for production, prefer controlled migrations.
- Web uses cookie-stored JWT for API calls; ensure HTTPS and secure cookie flags in production.
- Mobile resolves the API base URL automatically for emulators and physical devices; update `src/RestERP.Mobile/src/config.ts` if your API port changes.
