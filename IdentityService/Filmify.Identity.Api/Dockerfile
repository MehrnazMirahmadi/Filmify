# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityService/Filmify.Identity.Api/Filmify.Identity.Api.csproj", "IdentityService/Filmify.Identity.Api/"]
COPY ["IdentityService/Filmify.Identity.Application/Filmify.Identity.Application.csproj", "IdentityService/Filmify.Identity.Application/"]
COPY ["IdentityService/Filmify.Identity.Domain/Filmify.Identity.Domain.csproj", "IdentityService/Filmify.Identity.Domain/"]
COPY ["IdentityService/Filmify.Identity.Infrastructure/Filmify.Identity.Infrastructure.csproj", "IdentityService/Filmify.Identity.Infrastructure/"]
RUN dotnet restore "./IdentityService/Filmify.Identity.Api/Filmify.Identity.Api.csproj"
COPY . .
WORKDIR "/src/IdentityService/Filmify.Identity.Api"
RUN dotnet build "./Filmify.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Filmify.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Filmify.Identity.Api.dll"]