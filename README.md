# NeShop

This project aims to deploy an application with all three tiers (frontend / backend / database) from GitHub to Azure.

The demo application is an online e-commerce shop: NeShop, but it could be your application, if you like to replace the code.

## Points of interest

- **On Budget**: no expensive databases or resources are provisioned on Azure, good for students or individuals
- **Deployment fully automated** with GitHub Actions: frontend, backend, database
- Resources provisioned with **infrastructure as code**: **Bicep**
- **No passwords needed**: the application uses managed identities to access the database
- Database: **Azure Database for PostgreSQL flexible server**
- Database migrations with **efbundle** from container app job
- Backend: ASP.NET Core WebAPI
- Frontend: Web/BlazorServer
- [Backend API consumed as an OpenAPI specification by the Frontend](./docs/04-rest-client-generation-in-web-project.md)
- Azure Container Apps (= aca)
- Container registry: DockerHub
- [Images and test content created with AI](./docs/05-static-assets-generation.md)

## The project

Example of the application running on Azure:
![Products screen-shot](./docs/media/neshop-aca-products.png)

GitHub Actions workflow deploying to Azure, an execution example:
![GitHub Actions workflow screen-shot](./docs/media/github-workflow-execution.png)

## Next steps

Pick an interest and follow along.

- [Getting Started - Deploying to Azure with GitHub CI/CD](./docs/01-getting-started-deploying-to-azure.md)
- [Getting Started - Provisioning the infrastructure on Azure from a local machine](./docs/02-getting-started-provisioning-infrastructure.md)
- [Getting Started - Running locally connected to Azure resources](./docs/03-getting-started-running-locally-to-azure.md)
- [Generating the REST Client to consume API in the Web project](./docs/04-rest-client-generation-in-web-project.md)
- [Static Assets - How images were generated with AI](./docs/05-static-assets-generation.md)
