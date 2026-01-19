param appInsightsName string
param managedIdentityPrincipalId string // Principal ID for the Managed Identity
param userIdentityPrincipalId string = '' // Principal ID for the User Identity

// Define Role Definition IDs internally
// https://learn.microsoft.com/en-us/azure/role-based-access-control/built-in-roles
var monitoringRoleDefinitionId = '3913510d-42f4-4e42-8a64-420c390055eb' // Monitoring Metrics Publisher

resource monitoring 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightsName
}

// Role assignment for Application Insights - Managed Identity
resource appInsightsRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(monitoring.id, managedIdentityPrincipalId, monitoringRoleDefinitionId) // Use managed identity ID
  scope: monitoring
  properties: {
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', monitoringRoleDefinitionId)
    principalId: managedIdentityPrincipalId // Use managed identity ID
    principalType: 'ServicePrincipal' // Managed Identity is a Service Principal
  }
}

// Role assignment for Application Insights - User Identity
resource appInsightsRoleAssignment_User 'Microsoft.Authorization/roleAssignments@2022-04-01' = if (!empty(userIdentityPrincipalId)) {
  name: guid(monitoring.id, userIdentityPrincipalId, monitoringRoleDefinitionId)
  scope: monitoring
  properties: {
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', monitoringRoleDefinitionId)
    principalId: userIdentityPrincipalId // Use user identity ID
    principalType: 'User' // User Identity is a User Principal
  }
}
