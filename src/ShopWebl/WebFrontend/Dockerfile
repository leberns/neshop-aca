FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShopClient/ShopClient.csproj", "ShopClient/"]
COPY ["WebContracts/WebContracts.csproj", "WebContracts/"]
COPY ["WebCore/WebCore.csproj", "WebCore/"]
COPY ["WebFrontend/WebFrontend.csproj", "WebFrontend/"]
RUN dotnet restore "./WebFrontend/WebFrontend.csproj"
COPY . .
WORKDIR "/src/WebFrontend"
RUN dotnet build "./WebFrontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebFrontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebFrontend.dll"]
