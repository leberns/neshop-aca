# Getting Started – Running locally connected to Azure resources

How to run and debug the application locally, but connecting to the resources provisioned on your Azure account.

## Prerequisites

- [Azure](https://azure.microsoft.com/) free or pay-as-you-go account
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

It is assumed the code is locally in the `~/Dev/neshop-aca` folder on Linux / MacOS. On Windows just replace `~/Dev` by a path like `D:\Dev`.

It is assumed the infrastructure has already been provisioned on Azure, check the other Getting Started guides for this.

### Trust HTTPS Dev Cert

```Sh
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet dev-certs https --trust
```

## Set the connection string in User secrets for local debugging

The "ConnectionStrings:ShopDatabase" has to match an empty value in `appsettings.json`.

```Sh
# initialize the user secrets for the first time
cd ~/Dev/neshop-aca/src/ShopServer/Host/

dotnet user-secrets init
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

## Execute the migration

```Sh
# execute the migration to create or update the database referenced in app settings
cd ~/Dev/neshop-aca/src/ShopServer/

dotnet ef database update --project Database --startup-project Host
```

## Run locally

Open the solution file: `NeShop.slnx`

Execute the projects in debug mode:

* Host.https
* WebFrontend.https

## Verify if the Host is running

Check with the HTTP client files if the endpoints are reachable:

* ~/Dev/neshop-aca/src/ShopServer/Host/ApiShop.http

## Verify if the WebFrontend is running

Open in the browser https://localhost:5041/
