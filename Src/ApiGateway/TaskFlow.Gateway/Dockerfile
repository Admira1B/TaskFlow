FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# === Project ===
COPY ["Src/ApiGateway/TaskFlow.Gateway/TaskFlow.Gateway.csproj", "Src/ApiGateway/TaskFlow.Gateway/"]

# === Shared projects ===
COPY ["Src/Shared/TaskFlow.Shared.Core/TaskFlow.Shared.Core.csproj", "Src/Shared/TaskFlow.Shared.Core/"]
COPY ["Src/Shared/TaskFlow.Shared.Consul/TaskFlow.Shared.Consul.csproj", "Src/Shared/TaskFlow.Shared.Consul/"]
COPY ["Src/Shared/TaskFlow.Shared.Logging/TaskFlow.Shared.Logging.csproj", "Src/Shared/TaskFlow.Shared.Logging/"]
COPY ["Src/Shared/TaskFlow.Shared.Middlewares/TaskFlow.Shared.Middlewares.csproj", "Src/Shared/TaskFlow.Shared.Middlewares/"]

# === Solution files ===
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]

RUN dotnet restore "Src/ApiGateway/TaskFlow.Gateway/TaskFlow.Gateway.csproj"
COPY . .

WORKDIR "/src/Src/ApiGateway/TaskFlow.Gateway"
RUN dotnet build "TaskFlow.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskFlow.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

COPY ["Src/ApiGateway/TaskFlow.Gateway/OcelotConfigurations/", "/app/OcelotConfigurations/"]
RUN touch /app/OcelotConfigurations/ocelot.json && \
    chmod 666 /app/OcelotConfigurations/ocelot.json && \
    chmod 755 /app/OcelotConfigurations/

HEALTHCHECK --interval=30s --timeout=3s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TaskFlow.Gateway.dll"]