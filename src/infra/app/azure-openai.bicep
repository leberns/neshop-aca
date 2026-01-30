@description('Location for AI resources. Change this location parameter if the deployment fails because region do not have the models')
param location string
param name string
param tags object = {}

module aiResource 'br/public:avm/res/cognitive-services/account:0.14.1' = {
  name: 'ai-resource-${name}'
  scope: resourceGroup()
  params: {
    name: name
    location: location
    tags: tags

    sku: 'S0'
    kind: 'OpenAI'
    disableLocalAuth: false
    apiProperties: {
      enableManagedIdentity: true
      scaleType: 'Standard'
      customSubDomainName: name
    }
    publicNetworkAccess: 'Enabled'
    deployments: [
      {
        name: 'embedding-model'
        sku: {
          name: 'GlobalStandard'
          capacity: 1
        }
        model: {
          name: 'text-embedding-3-small'
          format: 'OpenAI'
          version: '1'
        }
      }
      {
        name: 'chat-model'
        sku: {
          name: 'GlobalStandard'
          capacity: 1
        }
        model: {
          name: 'gpt-4o-mini'
          format: 'OpenAI'
          version: '2024-07-18'
        }
      }
    ]
  }
}

output name string = aiResource.outputs.name
output endpoint string = aiResource.outputs.endpoint
output resourceId string = aiResource.outputs.resourceId
