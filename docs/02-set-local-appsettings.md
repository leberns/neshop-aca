# Set local settings

## Set the connection string in User secrets for local debugging

The "ConnectionStrings:ShopDatabase" has to match an empty value in `appsettings.json`.

```Sh
# initialize the user secrets for the first time
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet user-secrets init
```

```Sh
# set the connection string to the LOCAL database
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet user-secrets set "ConnectionStrings:ShopDatabase" "Host=localhost;Database=neshopdb;Username=posadmin;Password=pospassw0rd"
# Password = something more complex than pospassw0rd

# depending on how the password was set in `dotnet user-secrets set`, afterwards clear the command history. Ex., for zsh:
history -p
```

```Sh
# set the connection string to Azure PostgreSQL after provisioning the infrastructure on Azure (update Host and Username!)
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet user-secrets set "ConnectionStrings:ShopDatabase" "Host=psql-wiok5xjx2aqja.postgres.database.azure.com;Database=neshopdb;Username=lebernsmuller@hotmail.com;Ssl Mode=Require;"
```

```Sh
# set empty for testing
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet user-secrets set "ConnectionStrings:ShopDatabase" ""
```
