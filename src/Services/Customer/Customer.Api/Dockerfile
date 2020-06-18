#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY src/Services/Customer/Customer.Api/Customer.Api.csproj src/Services/Customer/Customer.Api/
COPY src/Services/Customer/Customer.Application/Customer.Application.csproj src/Services/Customer/Customer.Application/
COPY src/Services.Common/Services.Common.csproj src/Services.Common/
COPY src/Services/Customer/Customer.Domain/Customer.Domain.csproj src/Services/Customer/Customer.Domain/
RUN dotnet restore "src/Services/Customer/Customer.Api/Customer.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Customer/Customer.Api"
RUN dotnet build "Customer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.Api.dll"]
