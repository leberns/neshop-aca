# Perform EF Core migrations

## Prerequisites

PostgreSQL installed and running on your machine / container engine, see `docker-compose.yml`.

## Create and execute the migration

```Sh
# for the first time only: install the dotnet-ef tool
cd ~/Dev/neshop-aca/ShopServer/

dotnet new tool-manifest
dotnet tool install dotnet-ef --local --version 10.0.1

# check if the `dotnet-ef` tool was installed
cat .config/dotnet-tools.json
```

```Sh
# build and create the migration
cd ~/Dev/neshop-aca/ShopServer/

dotnet build
dotnet ef migrations add Initial --project Database --startup-project Host
```

```Sh
# execute the migration to create or update the database referenced in app settings
cd ~/Dev/neshop-aca/ShopServer/

dotnet ef database update --project Database --startup-project Host
```

## Fix the error "Unable to create a 'DbContext' of type 'ShopDbContext'"

To fix this error when calling `dotnet ef migrations`,
pass the options with connection string to the constructor of ShopDbContext: `DbContextOptions<ShopDbContext> options,`

Error example:

```
Unable to create a 'DbContext' of type 'ShopDbContext'.
The exception 'No database provider has been configured for this DbContext.
A provider can be configured by overriding the 'DbContext.OnConfiguring' method
or by using 'AddDbContext' on the application service provider.
If 'AddDbContext' is used, then also ensure that your DbContext type accepts a DbContextOptions<TContext>
object in its constructor and passes it to the base constructor for DbContext.'
was thrown while attempting to create an instance.
For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
```

## Fix the error "Unable to create a 'DbContext' of type 'RuntimeType'"

To fix this error when calling `dotnet ef migrations`,
make sure to add the parameter `--startup-project Host` to the command-line during migrations and DB updating.

Error example:

```
Unable to create a 'DbContext' of type 'RuntimeType'.
The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[ShopDbContext]'
while attempting to activate 'ShopDbContext'.'
was thrown while attempting to create an instance.
For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
```
