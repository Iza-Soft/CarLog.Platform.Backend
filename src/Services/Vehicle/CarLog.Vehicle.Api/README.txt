use power shell for migration with commands 
********************************************** if have some problem with command "dotnet ef", see link: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli **********************************************
*      1. Test dotnet CLI with command dotnet ef
*      2. if there is problem whit dotnet CLI
*      3. Run command in Developer Command Prompt or Developer PowerShell: dotnet tool install --global dotnet-ef 
*      4. Install the latest Microsoft.EntityFrameworkCore.Design package.
*************************************************************************************************************************************************************************************************************************************************
!!! To create and apply migration need to be in CarLog.Vehicle.Api directory !!!
1.Create Migration: dotnet ef migrations add InitialCreate --context VehicleDbContext --project ../CarLog.Vehicle.Infrastructure --output-dir ../CarLog.Vehicle.Infrastructure/Persistence/Migrations
2.Apply Migration but localy: dotnet ef database update --context VehicleDbContext --project ..\CarLog.Vehicle.Infrastructure
3.Apply Migration with docker connection string: dotnet ef database update --context VehicleDbContext --project ..\CarLog.Vehicle.Infrastructure --connection "Server=sql-server,1433;Database=CarLogVehicleDb;User Id=sa;Password=CarLog123!;TrustServerCertificate=True;MultipleActiveResultSets=true"
 
see link: https://www.entityframeworktutorial.net/efcore/cli-commands-for-ef-core-migration.aspx
 
if have been used CLI for DB migration, that what you have to do is to add migration assembly (place that have been used for the migration)
Here, can use Lazy Loading Related Data, as adding UseLazyLoadingProxies(), but for this purpose have to install Microsoft.EntityFrameworkCore.Proxies package
in Package Manager: Install-Package Microsoft.EntityFrameworkCore.Proxies -Version 2.2.0
