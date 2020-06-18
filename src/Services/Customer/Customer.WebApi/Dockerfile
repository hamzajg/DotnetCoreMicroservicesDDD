#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY src/Services/Customer/Customer.Api/Customer.WebApi.csproj src/Services/Customer/Customer.WebApi/
COPY src/Services/Customer/Customer.Application/Customer.Application.csproj src/Services/Customer/Customer.Application/
COPY src/Common/Domain.Common/Domain.Common.csproj src/Common/Domain.Common/
COPY src/Services/Customer/Customer.Domain/Customer.Domain.csproj src/Services/Customer/Customer.Domain/
RUN dotnet restore "src/Services/Customer/Customer.WebApi/Customer.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Services/Customer/Customer.WebApi"
RUN dotnet build "Customer.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customer.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.WebApi.dll"]
