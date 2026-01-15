# Getting Started - Deploying to Azure

How to deploy from your GitHub repository to your Azure account.

## Prerequisites

- [Azure](https://azure.microsoft.com/) free or pay-as-you-go account
- [GitHub](https://github.com/) account and repository called "neshop-aca"
- [DockerHub](https://hub.docker.com/) account and repository called "neshop"
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

## Prepare your GitHub repository

Clone / download the repository and copy the content to your repository "neshop-aca" on GitHub.

It is assumed your repository will be under an `~/Dev` folder on Linux / MacOS.

On Windows just replace `~/Dev` by a path like `D:\Dev` in the scripts.

```Sh
# get the code with "git clone" and copy to your repository at ~/Dev
git clone https://github.com/leberns/neshop-aca.git
```

Make sure to push what is in `~/Dev/neshop-aca` to your repository on GitHub.

## Configure the identity on Azure for the deployments

The GitHub Actions need an identity on Azure so that it can deploy create or update resources on Azure (databases, container apps, etc).

The identity is authorized at subscription level, so that it can create resource groups and also assign roles to the managed identities.

This script creates the identity on Azure.

Please review it's values before executing it.

```Sh
cd ~/Dev/neshop-aca/src/tools/

# make the script executable, if needed
chmod +x ./set-oidc-github-azure.sh

# review the variables `GITHUB_REPO` and `FC_SUBJECT` and execute
./set-oidc-github-azure.sh
```

The script outputs a few values, add them as secrets to the GitHub Repo, for example:

```
Add the following secrets to GitHub (<the repo> > Settings > Secrets and variables > Actions > New repository secrets):
AZURE_CLIENT_ID        = <guid>
AZURE_TENANT_ID        = <guid>
AZURE_SUBSCRIPTION_ID  = <guid>
```

Do not copy/paste the values with spaces.

With this done, the GitHub Actions can now deploy from your neshop-aca repository, main branch, on your Azure subscription.

## Prepare access to the container images on Docker Hub

So that the container images on Docker Hub can be updated and used in deployment workflows from GitHub Actions.

Make sure to have:

- your Docker Hub `<dockerhub_username>`
- your GitHub `<github_username>` and repository `<github_repository>`.

Prepare a personal access token (PAT) in Docker Hub with **Read/Write** access:

- https://app.docker.com/ > Settings > Personal access tokens > Generate new token

  Description: Push pull images from GitHub neshop-aca

  Access permissions: Read & Write

  Copy the personal access token, it will not be available later.

In the GitHub repository, set the secrets:

<the repo> > Settings > Secrets and variables > Actions > New repository secrets

```
DOCKERHUB_USERNAME: <dockerhub_username>
DOCKERHUB_TOKEN: <the PAT> (ex.: dckr_pat_123...)
```

## Deploying to Azure
