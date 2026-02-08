FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Host/Host.csproj", "Host/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Database/Database.csproj", "Database/"]
COPY ["AiClient/AiClient.csproj", "AiClient/"]
RUN dotnet restore "./Host/Host.csproj"
COPY . .
WORKDIR "/src/Host"
RUN dotnet build "./Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build AS efbundle
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/Database
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef
RUN dotnet tool restore
RUN dotnet ef migrations bundle --project ./Database.csproj --startup-project ../Host/Host.csproj --self-contained -o /app/efbundle --verbose

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=efbundle /app/efbundle ./efbundle
ENTRYPOINT ["dotnet", "Host.dll"]
