targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('The environment and application names, ex.: dev-neshop-aca. It is also used for the resource group naming: rg-<env-name>.')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
@allowed([
  'australiaeast'
  'australiasoutheast'
  'brazilsouth'
  'canadacentral'
  'centralindia'
  'centralus'
  'eastasia'
  'eastus'
  'eastus2'
  'eastus2euap'
  'francecentral'
  'germanywestcentral'
  'italynorth'
  'japaneast'
  'koreacentral'
  'northcentralus'
  'northeurope'
  'norwayeast'
  'southafricanorth'
  'southcentralus'
  'southeastasia'
  'southindia'
  'spaincentral'
  'swedencentral'
  'uaenorth'
  'uksouth'
  'ukwest'
  'westcentralus'
  'westeurope'
  'westus'
  'westus2'
  'westus3'
])
param location string

@description('Name of backend server')
param nameServer string

@description('Tag of backend server image to be provisioned on Docker Hub')
param imageServer string

@description('Name of web frontend')
param nameWeb string

@description('Tag of web frontend image to be provisioned on Docker Hub')
param imageWeb string

@description('Docker Hub username')
param dockerHubUsername string = ''

@secure()
@description('Docker Hub personal access token')
param dockerHubToken string = ''

@description('The Entra ID user principal name (email) of the developer / administrator with access to the database.')
param postgresEntraAdministratorName string
@description('The Entra ID user principal type of the developer / administrator with access to the database.')
param postgresEntraAdministratorType string
@description('The Entra ID user principal id (object id) of the developer / administrator with access to the database server.')
param postgresEntraAdministratorObjectId string

@description('The DB administrator username to be provisioned on the server (for passwordAuth authentication on PostgreSQL)')
param postgresAdministratorLogin string
@description('The DB administrator password (for passwordAuth authentication on PostgreSQL)')
@secure()
param postgresAdministratorPassword string

@description('Database name')
param postgresDatabaseName string

var abbrs = loadJsonContent('./utils/abbreviations.json')
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var postgresServerName = '${abbrs.dBforPostgreSQLServers}${resourceToken}'

var tags = { env: environmentName }

resource rg 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: '${abbrs.resourcesResourceGroups}${environmentName}'
  location: location
  tags: tags
}

// User assigned managed identity to be used by the app to reach other resources like database
module managedIdentity 'br/public:avm/res/managed-identity/user-assigned-identity:0.4.3' = {
  name: 'user-assigned-identity'
  scope: rg
  params: {
    location: location
    tags: tags
    name: '${abbrs.managedIdentityUserAssignedIdentities}neshopmid-${resourceToken}'
  }
}

module postgresServer 'app/postgresql.bicep' = {
  name: 'postgresql'
  scope: rg
  params: {
    name: postgresServerName
    location: location
    tags: tags
    databaseNames: [postgresDatabaseName]
    entraAdministratorName: postgresEntraAdministratorName
    entraAdministratorObjectId: postgresEntraAdministratorObjectId
    entraAdministratorType: postgresEntraAdministratorType
    administratorLogin: postgresAdministratorLogin
    administratorPassword: postgresAdministratorPassword
    identityObjectId: managedIdentity.outputs.principalId
    identityResourceId: managedIdentity.outputs.resourceId
    identityName: managedIdentity.outputs.name
    identityType: 'ServicePrincipal'
  }
}

module logAnalytics 'br/public:avm/res/operational-insights/workspace:0.14.2' = {
  name: 'log-analytics'
  scope: rg
  params: {
    name: '${abbrs.operationalInsightsWorkspaces}${resourceToken}'
    location: location
    tags: tags
  }
}

module monitoring 'br/public:avm/res/insights/component:0.7.1' = {
  name: '${uniqueString(deployment().name, location)}-appinsights'
  scope: rg
  params: {
    name: '${abbrs.insightsComponents}${resourceToken}'
    location: location
    tags: tags
    workspaceResourceId: logAnalytics.outputs.resourceId
    disableLocalAuth: true
  }
}

module aiResource './app/azure-openai.bicep' = {
  name: 'ai-resource'
  scope: rg
  params: {
    name: '${abbrs.cognitiveServicesAccounts}${resourceToken}'
    location: location
    tags: tags
  }
}

module containerAppsEnvironment 'br/public:avm/res/app/managed-environment:0.8.0' = {
  name: 'container-apps-env'
  scope: rg
  params: {
    name: '${abbrs.appManagedEnvironments}${resourceToken}'
    location: location
    tags: tags
    logAnalyticsWorkspaceResourceId: logAnalytics.outputs.resourceId
    zoneRedundant: false
  }
}

module migrationJob './app/migration-job.bicep' = {
  name: 'migration-job'
  scope: rg
  params: {
    name: '${nameServer}-migrations'
    imageApp: imageServer
    tags: tags
    environmentResourceId: containerAppsEnvironment.outputs.resourceId
    identityClientId: managedIdentity.outputs.clientId
    identityResourceId: managedIdentity.outputs.resourceId
    dockerHubUsername: dockerHubUsername
    dockerHubToken: dockerHubToken
    secrets: [
      {
        name: 'database-connection-string' // for migrations it is required password authentication to PostgreSQL
        value: 'Host=${postgresServer.outputs.fqdn};Port=5432;Database=${postgresDatabaseName};Username=${postgresAdministratorLogin};Password=${postgresAdministratorPassword};SSL Mode=Require;Trust Server Certificate=true;'
      }
    ]
    environmentVariables: [
      {
        name: 'CONNECTION_STRING'
        secretRef: 'database-connection-string'
      }
      {
        name: 'ASPNETCORE_ENVIRONMENT'
        value: 'Production'
      }
    ]
  }
}

module containerAppsServerApp 'app/container-apps.bicep' = {
  name: 'container-apps-server'
  scope: rg  
  params: {
    name: nameServer
    imageApp: imageServer
    tags: tags
    environmentResourceId: containerAppsEnvironment.outputs.resourceId
    identityClientId: managedIdentity.outputs.clientId
    identityResourceId: managedIdentity.outputs.resourceId
    dockerHubUsername: dockerHubUsername
    dockerHubToken: dockerHubToken
    secrets: [
      {
        name: 'database-connection-string'
        value: 'Host=${postgresServer.outputs.fqdn};Port=5432;Database=${postgresDatabaseName};Username=${managedIdentity.outputs.name};SSL Mode=Require;Trust Server Certificate=true;'
      }
    ]
    environmentVariables: [
      {
        name: 'ConnectionStrings__ShopDatabase'
        secretRef: 'database-connection-string'
      }
      {
        name: 'ManagedIdentityOptions__ManagedIdentityClientId'
        value: managedIdentity.outputs.clientId
      }
      {
        name: 'AiOptions__Endpoint'
        value: aiResource.outputs.endpoint
      }
      {
        name: 'ASPNETCORE_HTTP_PORTS'
        value: '80;8080'
      }
    ]
  }
}

module containerAppsWebApp 'app/container-apps.bicep' = {
  name: 'container-apps-web'
  scope: rg  
  params: {
    name: nameWeb
    imageApp: imageWeb
    tags: tags
    environmentResourceId: containerAppsEnvironment.outputs.resourceId
    dockerHubUsername: dockerHubUsername
    dockerHubToken: dockerHubToken
    secrets: [
      {
        name: 'shop-client-base-url'
        value: 'http://${containerAppsServerApp.outputs.fqdn}/'
      }
    ]
    environmentVariables: [
      {
        name: 'ShopClient__BaseUrl'
        secretRef: 'shop-client-base-url'
      }
      {
        name: 'ASPNETCORE_HTTP_PORTS'
        value: '80;8080'
      }
    ]
  }
}

module rbac 'app/rbac.bicep' = {
  name: 'rbac-assignments'
  scope: rg
  params: {
    appInsightsName: monitoring.outputs.name
    aiName: aiResource.name
    managedIdentityPrincipalId: managedIdentity.outputs.principalId
    userIdentityPrincipalId: postgresEntraAdministratorObjectId
  }
}

output NESHOP_RESOURCE_GROUP_NAME string = rg.name
output NESHOP_CONTAINER_APPS_SERVER_FQDN string = containerAppsServerApp.outputs.fqdn
output NESHOP_CONTAINER_APPS_WEBL_FQDN string = containerAppsWebApp.outputs.fqdn
output NESHOP_DATABASE_FQDN string = postgresServer.outputs.fqdn
output NESHOP_MIGRATION_JOB_NAME string = migrationJob.outputs.name
