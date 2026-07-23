FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Expense/CarLog.Expense.Api/CarLog.Expense.Api.csproj", "src/Services/Expense/CarLog.Expense.Api/"]
COPY ["src/Services/Expense/CarLog.Expense.Application/CarLog.Expense.Application.csproj", "src/Services/Expense/CarLog.Expense.Application/"]
COPY ["src/Services/Expense/CarLog.Expense.Domain/CarLog.Expense.Domain.csproj", "src/Services/Expense/CarLog.Expense.Domain/"]
COPY ["src/Services/Expense/CarLog.Expense.Infrastructure/CarLog.Expense.Infrastructure.csproj", "src/Services/Expense/CarLog.Expense.Infrastructure/"]
RUN dotnet restore "./src/Services/Expense/CarLog.Expense.Api/CarLog.Expense.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Expense/CarLog.Expense.Api"
RUN dotnet build "./CarLog.Expense.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CarLog.Expense.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "CarLog.Expense.Api.dll"]