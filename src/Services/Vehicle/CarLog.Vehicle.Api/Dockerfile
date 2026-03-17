# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Vehicle/CarLog.Vehicle.Api/CarLog.Vehicle.Api.csproj", "src/Services/Vehicle/CarLog.Vehicle.Api/"]
COPY ["src/Services/Vehicle/CarLog.Vehicle.Application/CarLog.Vehicle.Application.csproj", "src/Services/Vehicle/CarLog.Vehicle.Application/"]
COPY ["src/Services/Vehicle/CarLog.Vehicle.Domain/CarLog.Vehicle.Domain.csproj", "src/Services/Vehicle/CarLog.Vehicle.Domain/"]
COPY ["src/Services/Vehicle/CarLog.Vehicle.Infrastructure/CarLog.Vehicle.Infrastructure.csproj", "src/Services/Vehicle/CarLog.Vehicle.Infrastructure/"]
RUN dotnet restore "./src/Services/Vehicle/CarLog.Vehicle.Api/CarLog.Vehicle.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Vehicle/CarLog.Vehicle.Api"
RUN dotnet build "./CarLog.Vehicle.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarLog.Vehicle.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Environment variable (опционално)
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CarLog.Vehicle.Api.dll"]