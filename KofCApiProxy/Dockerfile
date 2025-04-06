# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# docker compose specify local nuget package repo
# https://stackoverflow.com/questions/52770960/how-to-use-local-nuget-package-sources-for-dockerfile-dotnet-restore
# https://stackoverflow.com/questions/48821991/dockerfile-cant-see-local-file-or-private-nuget-server
# https://medium.com/@cwoolum/using-nuget-restore-securely-inside-of-docker-containers-with-caching-2c2f5453905d


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KofCApiProxy/KofCApiProxy.csproj", "KofCApiProxy/"]
RUN dotnet restore "./KofCApiProxy/KofCApiProxy.csproj"
COPY . .
WORKDIR "/src/KofCApiProxy"
RUN dotnet build "./KofCApiProxy.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./KofCApiProxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KofCApiProxy.dll"]