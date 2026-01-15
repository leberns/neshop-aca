# Connecting to the database on Azure

```Sh
export PGHOST=psql-wiok5xjx2aqja.postgres.database.azure.com
export PGPORT=5432
export PGUSER=<Azure account>
export PGDATABASE=neshop

# get access token
export PGPASSWORD="$(az account get-access-token --resource https://ossrdbms-aad.database.windows.net --query accessToken --output tsv)"

# connect to database
psql
```

## Test migration job manually

```Sh
# start the job
az containerapp job start --name ca-neshop-server-migrations --resource-group rg-dev-neshop-aca --output json
```

```Sh
# list all executions of the job
az containerapp job execution list --name ca-neshop-server-migrations --resource-group rg-dev-neshop-aca
```

```Sh
# show one specific execution
az containerapp job execution show \
 --job-execution-name ca-neshop-server-migrations-lb6w9da \
 --name ca-neshop-server-migrations \
 --resource-group rg-dev-neshop-aca
```

```Sh
# job logs while executing job
az containerapp job logs show \
   --name ca-neshop-server-migrations \
   --container ca-neshop-server-migrations-container
  -g rg-dev-neshop-aca

#{"TimeStamp":"2026-01-10T10:17:11.6300559+00:00","Log":"at Microsoft.EntityFrameworkCore.Migrations.Design.MigrationsBundle.Execute(String context, Assembly assembly, Assembly startupAssembly, String[] args)"}
#{"TimeStamp":"2026-01-10T10:17:11.6300608+00:00","Log":"password has been provided but the backend requires one (in cleartext)"}
#{"TimeStamp":"2026-01-10T10:17:11.6562489+00:00","Log":"Migration failed with exit code 1 ==="}
```
