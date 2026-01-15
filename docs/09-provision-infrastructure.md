# Provision infrastructure from a local machine

The deployment is done through Bicep, the infrastructure sources are in the `infra/` folder.

## Prerequisites

- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)

## Check / install bicep locally

```Sh
ls ~/.azure/bin/bicep # see if bicep is installed

az bicep install

az bicep version
# Output:
# Bicep CLI version 0.39.26 (1e90b06e40)
```

## Verify and update deployment parameters

The parameters are defined in the `main.bicepparams` file. Verify if they are correct for your environment, location, etc.

The most commonly changeable parameters are read from environment variables, it makes them flexible to use in a CI/CD pipeline later.

The Bicep deployment runs at a subscription level so that Bicep can create a new resource group.
The resource group name as per the variables is `rg-dev-neshop-aca`, but can be changed with the variable `AZURE_ENV_NAME`.

```Sh
export AZURE_ENV_NAME='rg-dev-neshop-aca' # resource group name: rg-$AZURE_ENV_NAME, i.e.: rg-rg-dev-neshop-aca
export AZURE_LOCATION='westeurope'

SUBSCRIPTION_ID=$(az account show --query id -o tsv)
echo $SUBSCRIPTION_ID # make sure this is the right subscription where the infrastructure will be deployed

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

## Deployment

```Sh
# deploy the infrastructure
cd ~/Dev/neshop-aca/src/infra

az deployment sub create --name deploy-$AZURE_ENV_NAME --subscription $SUBSCRIPTION_ID --location $AZURE_LOCATION --template-file main.bicep --parameters main.bicepparam
```

```Sh
# list resource groups
az group list
```

```Sh
# delete the resource group to avoid incurring charges (after using it)
az group delete --name rg-dev-neshop-aca
```
