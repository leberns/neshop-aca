# How to build and push container images to Docker Hub

The container images used for the deployment of NeShop container apps on Azure are in the container registry Docker Hub.

The GitHub Actions Workflow builds these container images automatically.
However, in case it is desired to provision the NeShop application from the local machine on Azure,
check here how to build and push the images from the local machine to Docker Hub.

## Prerequisites

- Docker Desktop, with buildx available.

- Docker Hub repository:
  - sign-in / sign-up on https://hub.docker.com/
  - create a repository, ex.: `neshop`

## Build and push the images

The images are built with the Dockerfiles and pushed to Docker Hub.

These commands update the images on Docker Hub with the latest changes, execute them as needed.

Update `lebers/neshop` to the <account>/<repository> on the scripts below to push the images to your Docker Hub repository.

```Sh
# for the first time, create an initial builder for multi-arch images

docker buildx create --name neshop_builder --use
```

```Sh
# build and push the image for the server
cd ~/Dev/neshop-aca/src/ShopServer/

docker buildx build \
  -t leberns/neshop:server-latest \
  -f ./Host/Dockerfile . \
  --platform linux/amd64 \
  --push
```

```Sh
# build and push the image for the web frontend
cd ~/Dev/neshop-aca/src/ShopWebl/

docker buildx build \
  -t leberns/neshop:webl-latest \
  -f ./WebFrontend/Dockerfile . \
  --platform linux/amd64 \
  --push
```

The container images are available on your repository in Docker Hub: https://hub.docker.com/

### Multi-arch images

- `linux/amd64`: for Azure container app architecture
- `linux/arm64`: optional, for local development environment (update it according to your local development environment)

Usege: `  --platform linux/amd64,linux/arm64 \` to build the multi-arch images, if you like to do so.

## Updating the container apps on Azure

If the container apps have already been deployed to Azure at least once, it is possible to update them. 

```Sh
# update the server container app on Azure
az containerapp update \
  --name ca-neshop-server \
  --resource-group rg-dev-neshop-aca \
  --image leberns/neshop:server-latest
```

```Sh
# check container status
az containerapp revision list --name ca-neshop-server --resource-group rg-dev-neshop-aca  --output table
```

```Sh
# stop container revision
az containerapp revision deactivate --name ca-neshop-server --resource-group rg-dev-neshop-aca --revision ca-neshop-server--gi1562l
```

```Sh
# stop container revision
az containerapp revision activate --name ca-neshop-server --resource-group rg-dev-neshop-aca --revision ca-neshop-server--gi1562l
```

```Sh
# update the frontend container app on Azure
az containerapp update \
  --name ca-neshop-webl \
  --resource-group rg-dev-neshop-aca \
  --image leberns/neshop:webl-latest
```
