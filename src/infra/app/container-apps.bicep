// create a container app for a microservice (frontend or backend)

param name string
@description('Primary location for all resources')
param location string = resourceGroup().location
param tags object = {}

@description('Container app environment resource id')
param environmentResourceId string = ''

@secure()
@description('Application Insights (monitoring) connection string')
param applicationInsightsConnectionString string

@description('User assigned managed identity client id, the app uses this identity to access other resources')
param identityClientId string
@description('User assigned managed identity resource id')
param identityResourceId string = ''

@description('Container image')
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

var monitoringSettings = [
  {
    name: 'APPLICATIONINSIGHTS_AUTHENTICATION_STRING'
    value: 'ClientId=${identityClientId};Authorization=AAD'
  }
  {
    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
    value: applicationInsightsConnectionString
  }
]

module containerAppsApp 'br/public:avm/res/app/container-app:0.19.0' = {
  name: 'container-apps-${name}'
  scope: resourceGroup()
  params: {
    name: name
    environmentResourceId: environmentResourceId
    location: location
    tags: tags

    ingressTargetPort: 8080
    ingressExternal: true
    ingressTransport: 'auto'
    ingressAllowInsecure: false
    stickySessionsAffinity: 'sticky'
    scaleSettings: {
      minReplicas: 1
      maxReplicas: 2
      rules: [
        {
          name: 'http-scaling'
          http: {
            metadata: {
              concurrentRequests: '10'
            }
          }
        }
      ]
    }
    corsPolicy: {
      allowCredentials: true
      allowedOrigins: [
        '*'
      ]
    }

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
      ] : null
    }

    secrets: union(secrets, identitySecret, dockerHubSecrets)

    containers: [
      {
        image: imageApp
        name: '${name}-container'
        resources: {
          cpu: json('0.25') // cpu is int, the json parsing is to avoid the type warning for the fraction
          memory: '0.5Gi'
        }
        env: union(environmentVariables, identityVariables, monitoringSettings)
      }
    ]
  }
}

output fqdn string = containerAppsApp.outputs.fqdn
output name string = containerAppsApp.outputs.name
output resourceId string = containerAppsApp.outputs.resourceId
