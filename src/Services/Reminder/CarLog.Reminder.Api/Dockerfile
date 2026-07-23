FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Reminder/CarLog.Reminder.Api/CarLog.Reminder.Api.csproj", "src/Services/Reminder/CarLog.Reminder.Api/"]
COPY ["src/Services/Reminder/CarLog.Reminder.Application/CarLog.Reminder.Application.csproj", "src/Services/Reminder/CarLog.Reminder.Application/"]
COPY ["src/Services/Reminder/CarLog.Reminder.Domain/CarLog.Reminder.Domain.csproj", "src/Services/Reminder/CarLog.Reminder.Domain/"]
COPY ["src/Services/Reminder/CarLog.Reminder.Infrastructure/CarLog.Reminder.Infrastructure.csproj", "src/Services/Reminder/CarLog.Reminder.Infrastructure/"]
RUN dotnet restore "./src/Services/Reminder/CarLog.Reminder.Api/CarLog.Reminder.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Reminder/CarLog.Reminder.Api"
RUN dotnet build "./CarLog.Reminder.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarLog.Reminder.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CarLog.Reminder.Api.dll"]