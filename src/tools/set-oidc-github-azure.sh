#!/bin/bash
# Configure the connection between GitHub and Azure for CI/CD so that GitHub Actions workflows can deploy to Azure using OpenID Connect (OIDC).

# Review these values, update accordingly:
GITHUB_REPO="neshop-aca" # replace with your GitHub repo
FC_SUBJECT="repo:leberns/neshop-aca:ref:refs/heads/main" # "subject", the federated credential has access just to this specific GitHub repo and branch!

echo "Setting up GitHub -> Azure OpenID Connect authentication (OIDC)"

IDENTITY_NAME="id-github-oidc-$GITHUB_REPO" # the identity name
APPLICATION_REGISTRATION_NAME="are-id-github-oidc-$GITHUB_REPO" # are = application registrations
FEDERATED_CREDENTIAL_NAME="fic-id-github-oidc-$GITHUB_REPO" # fic = federated identity credential
ROLE_1="Contributor" # with this role the GitHub OIDC identity can create resources
ROLE_2="Role Based Access Control Administrator" # with this role the GitHub OIDC identity can assign roles to other identities (RBAC for managed identities)
SCOPE="/subscriptions/$SUBSCRIPTION_ID"

SUBSCRIPTION_ID=$(az account show --query id -o tsv) # the Azure subscription where deployments shall be done, make sure `az` is in the right subscription
TENANT_ID=$(az account show --query tenantId -o tsv)

echo ""
echo "GITHUB_REPO: $GITHUB_REPO                                  <-- check repository name"
echo "FC_SUBJECT: $FC_SUBJECT  <-- double check repository and branch"
echo "SUBSCRIPTION_ID: $SUBSCRIPTION_ID    <-- is this the right subscription?"
echo "TENANT_ID: $TENANT_ID"
echo "IDENTITY_NAME: $IDENTITY_NAME"
echo "APPLICATION_REGISTRATION_NAME: $APPLICATION_REGISTRATION_NAME"
echo "FEDERATED_CREDENTIAL_NAME: $FEDERATED_CREDENTIAL_NAME"
read -p "Is this information correct? (Y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Nn]$ ]]; then
    echo "Exiting..."
    exit 1
fi

echo ""
echo "Checking application registration $APPLICATION_REGISTRATION_NAME"

CLIENT_ID=$(az ad app list --filter "displayName eq '$APPLICATION_REGISTRATION_NAME'" --query [].appId -o tsv)

if [[ -z "$CLIENT_ID" ]]
then
    echo "Registering application in Entra ID"
    CLIENT_ID=$(az ad app create --display-name ${APPLICATION_REGISTRATION_NAME} --query appId -o tsv)
fi

echo "Application (client) ID: $CLIENT_ID"

echo ""
echo "Checking service principal"

SP_OBJECT_ID=$(az ad sp list --filter "appId eq '$CLIENT_ID'" --query [].id -o tsv)

if [[ -z "$SP_OBJECT_ID" ]]
then
    echo "Creating service principal"
    SP_OBJECT_ID=$(az ad sp create --id $CLIENT_ID --query id -o tsv)
fi

echo "Service principal object ID: $SP_OBJECT_ID"

echo ""
echo "Checking role assignment for role '$ROLE_1'"

COUNT_ASSIGNED=$(az role assignment list --assignee  "$SP_OBJECT_ID" --subscription "$SUBSCRIPTION_ID" --query "[?roleDefinitionName=='$ROLE_1'] | length(@)" -o tsv)

if [[ "$COUNT_ASSIGNED" -eq 0 ]]; then
    echo "Assigning role '$ROLE_1' to service principal $SP_OBJECT_ID at scope $SCOPE"
    az role assignment create --role "$ROLE_1" --scope "$SCOPE" --assignee-object-id "$SP_OBJECT_ID" --assignee-principal-type ServicePrincipal
else
    echo "The role '$ROLE_1' has already been assigned to the service principal"
    echo "For subscriptions, check: Azure Portal > Subscriptions > (the subscription) > Access control (IAM) > Role assignments"
fi

echo ""
echo "Checking role assignment for role '$ROLE_2'"

COUNT_ASSIGNED=$(az role assignment list --assignee  "$SP_OBJECT_ID" --subscription "$SUBSCRIPTION_ID" --query "[?roleDefinitionName=='$ROLE_2'] | length(@)" -o tsv)

if [[ "$COUNT_ASSIGNED" -eq 0 ]]; then
    echo "Assigning role '$ROLE_2' to service principal $SP_OBJECT_ID at scope $SCOPE"
    az role assignment create --role "$ROLE_2" --scope "$SCOPE" --assignee-object-id "$SP_OBJECT_ID" --assignee-principal-type ServicePrincipal
else
    echo "The role '$ROLE_2' has already been assigned to the service principal"
    echo "For subscriptions, check: Azure Portal > Subscriptions > (the subscription) > Access control (IAM) > Role assignments"
fi

echo ""
echo "Checking federated credential"

FEDERATED_CREDENTIAL=$(az ad app federated-credential list --id $CLIENT_ID --query "[?name=='$FEDERATED_CREDENTIAL_NAME']" -o tsv)

PARAMETERS='{
    "name": "'$FEDERATED_CREDENTIAL_NAME'",
    "issuer": "https://token.actions.githubusercontent.com",
    "subject": "'$FC_SUBJECT'",
    "audiences": ["api://AzureADTokenExchange"]
  }'

echo "PARAMETERS: $PARAMETERS"

if [[ -z "$FEDERATED_CREDENTIAL" ]]
then
    echo "Creating federated credential $FEDERATED_CREDENTIAL_NAME"
    az ad app federated-credential create --id $CLIENT_ID --parameters "$PARAMETERS"
else
    echo "The federated credential already exists"
    echo "Check in Azure Portal > Entra ID > App registrations > ($APPLICATION_REGISTRATION_NAME)"
    echo " > Certificates & secrets > Federated credentials > ($FEDERATED_CREDENTIAL_NAME) > Actions"
fi

echo ""
echo "Add the following secrets to GitHub (<the repo> > Settings > Secrets and variables > Actions > New repository secrets):"
echo "AZURE_CLIENT_ID        = $CLIENT_ID"
echo "AZURE_TENANT_ID        = $TENANT_ID"
echo "AZURE_SUBSCRIPTION_ID  = $SUBSCRIPTION_ID"

echo ""
echo "Done"