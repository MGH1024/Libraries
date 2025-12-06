# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# ---------------- BASE ----------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ---------------- BUILD ----------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore
COPY ["Services/Library/Library.Endpoint.Api/Library.Endpoint.Api.csproj", "Services/Library/Library.Endpoint.Api/"]
COPY ["Services/Library/Library.Application/Library.Application.csproj", "Services/Library/Library.Application/"]
COPY ["Services/Library/Library.Domain/Library.Domain.csproj", "Services/Library/Library.Domain/"]
COPY ["Services/Library/Library.Infrastructure/Library.Infrastructure.csproj", "Services/Library/Library.Infrastructure/"]
RUN dotnet restore "./Services/Library/Library.Endpoint.Api/Library.Endpoint.Api.csproj"

# Copy all project files
COPY . .

WORKDIR "/src/Services/Library/Library.Endpoint.Api"
RUN dotnet build "./Library.Endpoint.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------------- PUBLISH ----------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Library.Endpoint.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------------- FINAL ----------------
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Copy HealthCheck folder from API project into /app inside container
COPY Services/Library/Library.Endpoint.Api/HealthCheck /app/HealthCheck

ENTRYPOINT ["dotnet", "Library.Endpoint.Api.dll"]
