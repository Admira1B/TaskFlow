FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# === Project === 
COPY ["Src/Services/Tasks/TaskFlow.Tasks.API/TaskFlow.Tasks.API.csproj", "Src/Services/Tasks/TaskFlow.Tasks.API/"]

# === Direct dependencies === 
COPY ["Src/Services/Tasks/TaskFlow.Tasks.Application/TaskFlow.Tasks.Application.csproj", "Src/Services/Tasks/TaskFlow.Tasks.Application/"]
COPY ["Src/Services/Tasks/TaskFlow.Tasks.Infrastructure.Messaging/TaskFlow.Tasks.Infrastructure.Messaging.csproj", "Src/Services/Tasks/TaskFlow.Tasks.Infrastructure.Messaging/"]

# === Transitive dependencies === 
COPY ["Src/Services/Tasks/TaskFlow.Tasks.Domain/TaskFlow.Tasks.Domain.csproj", "Src/Services/Tasks/TaskFlow.Tasks.Domain/"]
COPY ["Src/Services/Tasks/TaskFlow.Tasks.Contracts/TaskFlow.Tasks.Contracts.csproj", "Src/Services/Tasks/TaskFlow.Tasks.Contracts/"]
COPY ["Src/Services/Tasks/TaskFlow.Tasks.Infrastructure.SqlServer/TaskFlow.Tasks.Infrastructure.SqlServer.csproj", "Src/Services/Tasks/TaskFlow.Tasks.Infrastructure.SqlServer/"]

# === External services contracts === 
COPY ["Src/Services/Identity/TaskFlow.Identity.Contracts/TaskFlow.Identity.Contracts.csproj", "Src/Services/Identity/TaskFlow.Identity.Contracts/"]

# === Shared projects === 
COPY ["Src/Shared/TaskFlow.Shared.Core/TaskFlow.Shared.Core.csproj", "Src/Shared/TaskFlow.Shared.Core/"]
COPY ["Src/Shared/TaskFlow.Shared.Consul/TaskFlow.Shared.Consul.csproj", "Src/Shared/TaskFlow.Shared.Consul/"]
COPY ["Src/Shared/TaskFlow.Shared.Logging/TaskFlow.Shared.Logging.csproj", "Src/Shared/TaskFlow.Shared.Logging/"]
COPY ["Src/Shared/TaskFlow.Shared.Messaging/TaskFlow.Shared.Messaging.csproj", "Src/Shared/TaskFlow.Shared.Messaging/"]
COPY ["Src/Shared/TaskFlow.Shared.ApiClients/TaskFlow.Shared.ApiClients.csproj", "Src/Shared/TaskFlow.Shared.ApiClients/"]
COPY ["Src/Shared/TaskFlow.Shared.Middlewares/TaskFlow.Shared.Middlewares.csproj", "Src/Shared/TaskFlow.Shared.Middlewares/"]

# === Solution files === 
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]

RUN dotnet restore "Src/Services/Tasks/TaskFlow.Tasks.API/TaskFlow.Tasks.API.csproj"
COPY . .

WORKDIR "/src/Src/Services/Tasks/TaskFlow.Tasks.API"
RUN dotnet build "TaskFlow.Tasks.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskFlow.Tasks.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TaskFlow.Tasks.API.dll"]
