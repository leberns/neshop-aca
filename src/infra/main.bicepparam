using './main.bicep'

// note: the parameters are described in main.bicep

param environmentName = readEnvironmentVariable('AZURE_ENV_NAME', 'dev-neshop-aca')
param location = readEnvironmentVariable('AZURE_LOCATION', 'westeurope')

param dockerHubUsername = readEnvironmentVariable('DOCKERHUB_USERNAME')
param dockerHubToken = readEnvironmentVariable('DOCKERHUB_TOKEN')

param postgresEntraAdministratorName = readEnvironmentVariable('POSTGRESQL_ENTRA_ADMIN_NAME', '')
param postgresEntraAdministratorType = readEnvironmentVariable('POSTGRESQL_ENTRA_ADMIN_TYPE', 'User')
param postgresEntraAdministratorObjectId = readEnvironmentVariable('POSTGRESQL_ENTRA_ADMIN_OBJECT_ID', '')

param postgresAdministratorLogin = readEnvironmentVariable('POSTGRESQL_ADMIN_LOGIN', '')
param postgresAdministratorPassword = readEnvironmentVariable('POSTGRESQL_ADMIN_PASSWORD', '')

param postgresDatabaseName = readEnvironmentVariable('POSTGRESQL_DATABASE_NAME', 'neshopdb')

param nameServer = 'ca-neshop-server'
param imageServer = 'leberns/neshop:server-latest'

param nameWeb = 'ca-neshop-webl'
param imageWeb = 'leberns/neshop:webl-latest'

param aiResourceLocation = 'uaenorth'
