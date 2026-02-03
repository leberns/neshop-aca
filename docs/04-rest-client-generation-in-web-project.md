# Generating the REST Client to consume API in the Web project

## Prerequisites

[NSwag commandline tool](https://github.com/RicoSuter/NSwag/wiki/CommandLine) is installed and available in the path:

```Sh
dotnet tool install --global NSwag.ConsoleCore
```

```Sh
nswag version
```

## Updating the ShopClient Project

Create / update the dotnet OpenApi client to be used for REST calls from the Web project to the ShopServer backend.

Execute the ShopServer project, open the Swagger UI at http://localhost:5050/swagger/index.html and replace the `openapi-v1.json` in the `Spec/` folder.

```Sh
# generate the client (to be done every time there is a change in openapi-v1.json)
cd ~/Dev/neshop-aca/src/ShopWebl/ShopClient/

nswag run ./Configuration/nswag.json
```

## Initial client setup

```Sh
# create the client configuration file "nswag.json" (just for the first time)
cd ~/Dev/neshop-aca/src/ShopWebl/ShopClient/

mkdir Configuration
cd Configuration

nswag new
```

Update in `Configuration/nswag.json`

```Json
{
  "runtime": "Net100",
  "documentGenerator": {
    "url": "../Spec/openapi-v1.json"
  },
  "codeGenerators": {
    "openApiToCSharpClient": {
      "className": "ShopClient",
      "generateClientInterfaces": true,
      "exceptionClass": "ShopApiException",
      "useBaseUrl": false,
      "namespace": "ShopWebl.ShopClient",
      "output": "../Generated/ShopClient.cs"
    }
  }
}
```

Why is "useBaseUrl": false?
To allow the base url to be settable per environment while the ShopClient is a strongly typed client.
The strongly typed client can be injected directly in services without HttpClientFactory.
