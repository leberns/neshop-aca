@description('Location for AI resources. Change this location parameter if the deployment fails because region do not have the model')
param location string = resourceGroup().location
param name string
param tags object = {}

var chatModelName = 'gpt-4o-mini'
var textEmbeddingModelName = 'text-embedding-3-small'

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
        name: 'model-${textEmbeddingModelName}'
        sku: {
          name: 'Standard'
        }
        model: {
          format: 'OpenAI'
          name: textEmbeddingModelName
          version: '2025-06-01'
        }
      }
      {
        name: 'model-${chatModelName}'
        sku: {
          name: 'Standard'
        }
        model: {
          format: 'OpenAI'
          name: chatModelName
          version: '2025-06-01'
        }
      }
    ]
  }
}

output endpoint string = aiResource.outputs.endpoint
output resourceId string = aiResource.outputs.resourceId
