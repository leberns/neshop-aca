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
# to execute locally referencing the infrastructure provisioned on Azure
cd ~/Dev/neshop-aca/src/ShopServer/Host/

# the connection string to Azure PostgreSQL: update Host and Username before updating this setting!
dotnet user-secrets set "ConnectionStrings:ShopDatabase" "Host=psql-mo7xupkdn22ec.postgres.database.azure.com;Database=neshopdb;Username=your-email@outlook.com;Ssl Mode=Require;"

# set the Azure OpenAI endpoint
dotnet user-secrets set "AiOptions:Endpoint" "https://cog-neshop1.openai.azure.com/"

```

```Sh
# set empty for testing
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet user-secrets set "ConnectionStrings:ShopDatabase" ""
```
