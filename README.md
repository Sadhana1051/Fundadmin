# Fund Administration API

ASP.NET Core Web API for managing investment funds, investors, and transactions.

## Requirements Met

- **CRUD** for Funds and Investors
- **POST** to register Transactions
- **GET** transactions by investor: `GET /api/v1/transactions/investor/{investorId}`
- **GET** subscribed/redeemed totals per fund: `GET /api/v1/transactions/fund/{fundId}/totals`
- Repository Pattern, DTOs, Dependency Injection
- FluentValidation for input validation
- Global exception handling with RFC 7807 ProblemDetails
- Swagger/OpenAPI with XML comments
- JWT Authentication + HTTPS
- Unit tests (TransactionService, TransactionRepository)

## Bonus

- API versioning (v1)
- Health check (`/health`)
- Serilog structured logging

## Getting Started

```bash
dotnet restore
dotnet build
dotnet run --project FundAdministration.Api.csproj
```

Open https://localhost:5001/swagger

### Obtain a JWT Token (for testing)

```http
POST https://localhost:5001/api/v1/auth/token
```

No body required. Use the returned `token` in the `Authorization: Bearer <token>` header for protected endpoints.

### Run Tests

```bash
dotnet test
```
