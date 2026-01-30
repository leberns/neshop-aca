// Flexible Server for PostgreSQL

param name string
param location string = resourceGroup().location
param tags object = {}

param skuName string = 'Standard_B1ms'
param tier string = 'Burstable'
param storageSizeGB int = 32

@description('Entra admin object ID (for activeDirectoryAuth enabled)')
param entraAdministratorObjectId string

@description('Entra admin name (for activeDirectoryAuth enabled)')
param entraAdministratorName string

@description('Entra admin user type (for activeDirectoryAuth enabled)')
@allowed([
  'User'
  'Group'
  'ServicePrincipal'
])
param entraAdministratorType string

@description('An DB administrator username (for passwordAuth enabled)')
param administratorLogin string = ''

@description('An DB administrator password (for passwordAuth enabled)')
@secure()
param administratorPassword string = ''

@description('Managed identity (object) id, to be used by the applications to connect to the DB')
param identityObjectId string

@description('Managed identity resource id (it is a resource path /subscriptions/<subscription-id>/...)')
param identityResourceId string

@description('Managed identity name')
param identityName string

@description('Managed identity type')
param identityType string

@description('Names of the databases to be created on the server')
param databaseNames array = []

@description('PostgreSQL version (version 16 should support managed identities)')
param version string = '18'

var hasEntra = !empty(entraAdministratorObjectId) && !empty(entraAdministratorName) && !empty(entraAdministratorType) ? true : false

module postgresServer 'br/public:avm/res/db-for-postgre-sql/flexible-server:0.15.1' = {
  name: 'postgresServer'
  params: {
    name: name
    location: location
    tags: tags
    skuName: skuName
    tier: tier
    publicNetworkAccess: 'Enabled'
    authConfig: {
      activeDirectoryAuth: hasEntra ? 'Enabled' : fail('Entra administrator are required, provide object id, name (email) and type.')
      passwordAuth: !empty(administratorLogin) && !empty(administratorPassword) ? 'Enabled' : 'Disabled'
    }
    configurations: [
      {
        name: 'azure.extensions'
        value: 'azure_ai,vector'
      }
    ]
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorPassword
    administrators: [
      {
        objectId: entraAdministratorObjectId
        principalName: entraAdministratorName
        principalType: entraAdministratorType
      }
      {
        objectId: identityObjectId
        principalName: identityName
        principalType: identityType
      }
    ]
    storageSizeGB: storageSizeGB
    firewallRules: [
      {
        name: 'AllowAllIPs'
        startIpAddress: '0.0.0.0'
        endIpAddress: '255.255.255.255'
      }
      {
        name: 'AllowAllAzureInternal'
        startIpAddress: '0.0.0.0'
        endIpAddress: '0.0.0.0'
      }
    ]
    databases: [for name in databaseNames: {
      name: name
    }]
    geoRedundantBackup: 'Disabled'
    highAvailability: 'Disabled'
    availabilityZone: -1
    version: version
    managedIdentities: {
      systemAssigned: false
      userAssignedResourceIds: [
        '${identityResourceId}'
      ]
    }
  }
}

output fqdn string =  postgresServer.outputs.?fqdn ?? fail('PostgreSQL FQDN not available')
