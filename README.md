## RestERP — Restaurant Management System

Production-ready, layered restaurant ERP built on ASP.NET Core (.NET 9). The solution provides a clean separation of concerns across API, Web (MVC), Application, Infrastructure, and Core domain layers.

### Highlights
- Clean, layered architecture (Core, Application, Infrastructure, API, Web)
- EF Core with SQL Server, soft-delete filters, precise money fields
- ASP.NET Core Identity (roles: Admin, Employee, Customer)
- JWT authentication (API + Web cookie bridge), Swagger with security scheme
- Autofac-based DI, generic repository + unit of work
- Global logging and exception handling middleware

## Architecture
- `RestERP.Core`: Domain entities, enums, base types
- `RestERP.Application`: Service abstractions/implementations and business logic
- `RestERP.Infrastructure`: EF DbContext, migrations, repositories, seed data
- `Services/RestERP.API`: REST API with Swagger, JWT, role policies
- `RestERP.Web`: MVC UI (Areas/Admin), HttpClient to API, auth integration

<div align="center">
    <table>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/14fcac76-30d6-4b66-a919-d73a881c8e60" alt="mainpage" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/4a868afa-651f-49c2-9358-d9650db4b7b2" alt="management" width="400"/>
            </td>
        </tr>
        <tr>
            <td>
                <img src="https://github.com/user-attachments/assets/74c4cadd-49ca-43e6-8060-00a5ba1af2b2" alt="menu" width="400"/>
            </td>
            <td>
                <img src="https://github.com/user-attachments/assets/8db0fe5f-c4d0-41a5-abc3-c5aa52bf1d85" alt="login" width="400"/>
            </td>
        </tr>
    </table>
</div>

### Data Access Patterns
- Generic Repository and Unit of Work are used to standardize data access and transaction boundaries while keeping domain logic clean.

## Tech Stack
- .NET 9, ASP.NET Core MVC, Entity Framework Core, SQL Server
- Autofac, ASP.NET Core Identity, JWT Bearer
- Swagger/OpenAPI, MemoryCache, Logging middleware

## Security
- JWT Bearer authentication with zero clock skew
- Role-based policies: `EmployeeOnly`, `CustomerOnly`
- Note: CORS is permissive in development (`AllowAll`). Harden for production.
- Seed passwords in code are for development. Rotate/remove in production.

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

