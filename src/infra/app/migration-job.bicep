// create a container app for EF Core migration jobs

param name string
@description('Primary location for all resources')
param location string = resourceGroup().location
param tags object = {}

@description('User assigned managed identity client id, the app uses this identity to access other resources')
param identityClientId string = ''
@description('User assigned managed identity resource id')
param identityResourceId string = ''

@description('Container app environment resource id')
param environmentResourceId string = ''

@description('Container image (same as your app image)')
param imageApp string

@description('Database connection string or components')
param secrets array = []

param environmentVariables array = []

@description('Docker Hub username')
param dockerHubUsername string = ''

@secure()
@description('Docker Hub personal access token')
param dockerHubToken string = ''

var dockerHubSecrets = !empty(dockerHubToken) ? [
  {
    name: 'docker-hub-password'
    value: dockerHubToken
  }
] : []

var identitySecret = !empty(identityClientId) ? [
  {
    name: 'user-assigned-managed-identity-client-id'
    value: identityClientId
  }
] : []

var identityVariables = !empty(identityClientId) ? [
  {
    name: 'AZURE_CLIENT_ID'
    secretRef: 'user-assigned-managed-identity-client-id'
  }
] : []

module migrationJob 'br/public:avm/res/app/job:0.5.1' = {
  name: 'migration-job-${name}'
  scope: resourceGroup()
  params: {
    name: name
    environmentResourceId: environmentResourceId
    location: location
    tags: tags

    // manual trigger - run via CLI or pipeline
    triggerType: 'Manual'
    manualTriggerConfig: {
    }

    // job execution settings
    replicaTimeout: 600  // 10 minutes timeout
    replicaRetryLimit: 0  // Don't retry failures

    registries: !empty(dockerHubUsername) ? [
      {
        server: 'index.docker.io'
        username: dockerHubUsername
        passwordSecretRef: 'docker-hub-password'
      }
    ] : null

    managedIdentities: {
      systemAssigned: false
      userAssignedResourceIds: !empty(identityResourceId) ? [
        identityResourceId
      ] : []
    }

    secrets: union(secrets, identitySecret, dockerHubSecrets)

    containers: [
      {
        image: imageApp
        name: '${name}-container'
        resources: {
          cpu: '1' // the job template supports cpu as string
          memory: '2Gi'
        }
        // Run './efbundle'
        command: [
          './efbundle'
        ]
        args: [
          '--connection'
          '$(CONNECTION_STRING)'
          '--verbose'
        ]
        env: union(environmentVariables, identityVariables)
      }
    ]
  }
}

output name string = migrationJob.outputs.name
output resourceId string = migrationJob.outputs.resourceId