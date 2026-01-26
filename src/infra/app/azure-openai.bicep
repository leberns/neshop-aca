@description('Location for AI resources. Change this location parameter if the deployment fails because region do not have the models')
param location string = resourceGroup().location
param name string
param tags object = {}

module aiResource 'br/public:avm/res/cognitive-services/account:0.14.1' = {
  name: 'ai-resource-${name}'
  scope: resourceGroup()
  params: {
    name: name
    location: location
    tags: tags

    sku:'S0'
    kind: 'OpenAI'
    apiProperties: {
      enableManagedIdentity: true
      scaleType: 'Standard'
    }
    publicNetworkAccess: 'Enabled'
    deployments: [
      {
        name: 'embedding-model'
        sku: {
          name: 'Standard'
        }
        model: {
          name: 'text-embedding-3-small'
          format: 'OpenAI'
          version: '2025-06-01'
        }
      }
      {
        name: 'chat-model'
        sku: {
          name: 'Standard'
        }
        model: {
          name: 'gpt-4o-mini'
          format: 'OpenAI'
          version: '2025-06-01'
        }
      }
    ]
  }
}

output endpoint string = aiResource.outputs.endpoint
output resourceId string = aiResource.outputs.resourceId
