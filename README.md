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

Customer and Product both expose the full Domain → Application → WebApi slice:
a command, a use case that publishes the domain event through an injectable
`PublisherServices`, a `ServiceCollectionExt.AddUseCases()` registration, and a
controller.

## HTTP API

| Service | Endpoint | Behaviour |
|---|---|---|
| Customer | `POST /Customer` | registers a customer, publishes `CustomerCreated`, returns `202 Accepted` |
| Product | `POST /Products` | creates a product, publishes `ProductCreated`, returns `202 Accepted` |

## Tech stack

- C# / .NET 10, .NET Standard 2.1 (shared libraries)
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

Install the .NET 10 SDK, then:

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

Reference/learning project. The structure is the lesson: each service is a
Domain → Application → WebApi slice, with Customer and Product wired
end-to-end and Identity still a scaffold.
