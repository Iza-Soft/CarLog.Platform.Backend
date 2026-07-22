FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Maintenance/CarLog.Maintenance.Api/CarLog.Maintenance.Api.csproj", "src/Services/Maintenance/CarLog.Maintenance.Api/"]
COPY ["src/Services/Maintenance/CarLog.Maintenance.Application/CarLog.Maintenance.Application.csproj", "src/Services/Maintenance/CarLog.Maintenance.Application/"]
COPY ["src/Services/Maintenance/CarLog.Maintenance.Domain/CarLog.Maintenance.Domain.csproj", "src/Services/Maintenance/CarLog.Maintenance.Domain/"]
COPY ["src/Services/Maintenance/CarLog.Maintenance.Infrastructure/CarLog.Maintenance.Infrastructure.csproj", "src/Services/Maintenance/CarLog.Maintenance.Infrastructure/"]
RUN dotnet restore "./src/Services/Maintenance/CarLog.Maintenance.Api/CarLog.Maintenance.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Maintenance/CarLog.Maintenance.Api"
RUN dotnet build "./CarLog.Maintenance.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarLog.Maintenance.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CarLog.Maintenance.Api.dll"]