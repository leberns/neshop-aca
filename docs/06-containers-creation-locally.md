# Containers creation for local testing

It is possible to execute the application locally on a container engine before deploying it to the cloud.

## Prerequisites

- [Docker Desktop](https://docs.docker.com/desktop/) or [Podman](https://podman-desktop.io/) or another container engine

The instructions below are for Docker Desktop.

## Create images on local docker

Add `Dockerfile` to the projects (reference https://github.com/azure-samples/dab-azure-sql-quickstart).

Check if they build and images are created in the local docker.

```Sh
cd ~/Dev/neshop-aca/ShopServer/

docker build -t neshop/shopserver -f ./Host/Dockerfile .
```

```Sh
cd ~/Dev/neshop-aca/ShopWebl/

docker build -t neshop/shopwebl -f ./WebFrontend/Dockerfile .
```

## Prepare secrets for the local database

A container for a local PostgreSQL is going to be created, too, see `docker-compose.yml` in `docker/` folder.

Therefore, make sure to have an `.env` file on the same folder as `docker-compose.yml` with a content based on `_env.template.txt`.

## Create and run containers locally from those images

The application configurations are defined in the `docker-compose.yml` file, ex.: database connection strings.

```Sh
cd ~/Dev/neshop-aca/docker/

docker compose up

# test: http://localhost:5052/products
# test: http://localhost:5042
```

Note:
The ports used by the containers are specified in `docker-compose.yml`, environment variable `ASPNETCORE_URLS`.
These ports are different from the ones used on debugging as stated in `launchSettings.json`, ShopServer Host and ShopWebl WebFrontend.
This allows containers and code running in debugging to execute at the same time.
