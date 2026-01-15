# Images creation on Docker Hub

Make the solution images available on Docker Hub to be deployed on Azure container apps.

## Prerequisites

- Docker Desktop, with buildx available.

- Docker Hub repository:
  - sign-in / sign-up on https://hub.docker.com/
  - create a repository, ex.: `neshop`

## Build and push the images

The images are built with the Dockerfiles and pushed to Docker Hub.

Make sure `leberns/neshop` is the <account>/<repository> to push the images to.

These commands update the images on Docker Hub with the latest changes, execute them as needed.

The multi-arch images are for:

- `linux/amd64`: Azure container app architecture
- `linux/arm64`: optional, local development environment (update it according to your local development environment)

Use `  --platform linux/amd64,linux/arm64 \`to build the multi-arch images.

```Sh
# for the first time, create an initial builder for multi-arch images

docker buildx create --name labs_builder --use
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
