# DotnetCoreMicroservicesDDD

A .NET microservices reference project built around Domain-Driven Design.

## Services

| Service | Projects |
|---|---|
| **Customer** | `Customer.Domain`, `Customer.Application`, `Customer.WebApi` |
| **Product** | `Product.Domain`, `Product.Application`, `Product.Infrastructure`, `Product.WebApi` |
| **Identity** | `Identity.WebApi` |
| **Common** | `Domain.Common`, `Infrastructure.Common` |

Each service follows a layered structure: a framework-free domain, an
application layer for use cases, an infrastructure layer for persistence and
integrations, and a Web API host.

## Tech stack

- C# / .NET Core 3.0 and 3.1, .NET Standard 2.1
- ASP.NET Core Web API
- Docker Compose for running the services together
- xUnit-style tests under `tests/Tests`

## Project layout

```
src/
  Common/                  # shared domain/infrastructure building blocks
  Services/
    Customer/              # Customer.Domain, .Application, .WebApi
    Product/               # Product.Domain, .Application, .Infrastructure, .WebApi
    Identity/              # Identity.WebApi
tests/
  Tests/                   # Product.Domain, Product.App, Customer.App tests
docker-compose.yml
MicroservicesDDD.sln
```

## Building

Install the .NET Core 3.1 SDK, then:

```bash
dotnet restore MicroservicesDDD.sln
dotnet build MicroservicesDDD.sln
```

## Running with Docker

```bash
docker compose up --build
```

`docker-compose.yml` defines the Identity, Customer, and Product APIs. Each has
its own Dockerfile under its `WebApi` project.

## Tests

```bash
dotnet test MicroservicesDDD.sln
```

## Status

Reference/learning project. It targets .NET Core 3.x, which is out of support;
moving forward would mean retargeting the projects to a supported .NET version.
