# Provisioning the infrastructure on Azure from a local machine

It is possible to deploy the application on Azure from a local machine instead of the GitHub Actions workflow.

## Prerequisites

- [Azure](https://azure.microsoft.com/) free or pay-as-you-go account
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

It is assumed the code is locally in the `~/Dev/neshop-aca` folder on Linux / MacOS. On Windows just replace `~/Dev` by a path like `D:\Dev`.

## Making sure container images are available

The container images of the application have to be available on Docker Hub before provisioning the application on Azure.

To make sure the images are available, there are two options:

1. Build the container images locally and push them to the container registry DockerHub, verify [How to build and push container images to Docker Hub](07-images-creation-docker-hub.md)

2. Use dummy images for the container apps:
  * remove the `imageServer` and `imageWeb` from the `main.bicepparam` file so that the default `containerapps-helloworld` image is used.

## Check / install bicep locally

```Sh
ls ~/.azure/bin/bicep # see if bicep is installed (Linux / MacOS)

az bicep install

az bicep version
# Output:
# Bicep CLI version 0.39.26 (1e90b06e40)
```

## Verify and update deployment parameters

The infrastructure sources are in the `infra/` folder.

The deployment parameters are defined in the `main.bicepparams` file. Verify if they are correct for your environment, location, etc.

The most commonly changeable parameters are read from environment variables, it makes them flexible to use in a CI/CD pipeline later.

The Bicep deployment runs at a subscription level so that Bicep can create a new resource group.

The resource group name is `rg-dev-neshop-aca`, but can be changed with the variable `AZURE_ENV_NAME`.

```Sh
export AZURE_ENV_NAME='dev-neshop-aca' # resource group name: rg-$AZURE_ENV_NAME, i.e.: rg-dev-neshop-aca
export AZURE_LOCATION='westeurope'

SUBSCRIPTION_ID=$(az account show --query id -o tsv)
echo $SUBSCRIPTION_ID # make sure this is the right subscription where the infrastructure will be deployed

# Application Insights connection string for neshop-aca - update according to the value in Azure Portal (after the provisioning)
export APPLICATIONINSIGHTS_CONNECTION_STRING="InstrumentationKey=00000000-0000-0000-0000-000000000000;IngestionEndpoint=https://westeurope-5....."

# login to dockerhub
export DOCKERHUB_USERNAME='<username>' # your Docker Hub account username
export DOCKERHUB_TOKEN
read -s DOCKERHUB_TOKEN # enter Docker Hub Personal Access Token (PAT) from https://hub.docker.com/settings/security

# The Entra ID user of the developer / administrator with access to the database without using a password.
export POSTGRESQL_ENTRA_ADMIN_NAME=$(az account show --query user.name -o tsv) # ex.: my-account-email@outlook.com on Entra ID
export POSTGRESQL_ENTRA_ADMIN_TYPE='User'
export POSTGRESQL_ENTRA_ADMIN_OBJECT_ID=$(az ad signed-in-user show --query id -o tsv) # the user object id on Entra ID

# The recommendation is to access the db through the Entra ID user configured above,
# but as fallback the credentials of a local postgresql admin can be provided here. It will be then provisioned with the database.
export POSTGRESQL_ADMIN_LOGIN='posadmin'
export POSTGRESQL_ADMIN_PASSWORD
read -s POSTGRESQL_ADMIN_PASSWORD # enter a password, something more complex than pospassw0rd
```

## Infrastructure deployment

```Sh
# deploy the infrastructure
cd ~/Dev/neshop-aca/src/infra

az deployment sub create \
  --name deploy-$AZURE_ENV_NAME \
  --subscription $SUBSCRIPTION_ID \
  --location $AZURE_LOCATION \
  --template-file main.bicep \
  --parameters main.bicepparam \
  --output table
```

```Sh
# list resource groups
az group list
```

```Sh
# delete the resource group to avoid incurring charges (after using it)
az group delete --name rg-dev-neshop-aca
```

```Sh
# Purge cognitive services, needed after deleting the resource group.
# Update location according to the region where the cognitive services were deployed.
az cognitiveservices account purge \
  --name cog-neshop1 \
  --resource-group rg-dev-neshop-aca \
  --location swedencentral
```

```Sh
az cognitiveservices account list --query "[].{name:name, resourceGroup:resourceGroup, endpoint:properties.endpoint}"
```
