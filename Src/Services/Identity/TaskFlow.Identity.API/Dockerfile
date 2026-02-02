FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Identity Service Project files
COPY ["Src/Services/Identity/TaskFlow.Identity.Contracts/TaskFlow.Identity.Contracts.csproj", "Src/Services/Identity/TaskFlow.Identity.Contracts/"]
COPY ["Src/Services/Identity/TaskFlow.Identity.API/TaskFlow.Identity.API.csproj", "Src/Services/Identity/TaskFlow.Identity.API/"]
COPY ["Src/Services/Identity/TaskFlow.Identity.Application/TaskFlow.Identity.Application.csproj", "Src/Services/Identity/TaskFlow.Identity.Application/"]
COPY ["Src/Services/Identity/TaskFlow.Identity.Domain/TaskFlow.Identity.Domain.csproj", "Src/Services/Identity/TaskFlow.Identity.Domain/"]
COPY ["Src/Services/Identity/TaskFlow.Identity.Infrastructure.Messaging/TaskFlow.Identity.Infrastructure.Messaging.csproj", "Src/Services/Identity/TaskFlow.Identity.Infrastructure.Messaging/"]
COPY ["Src/Services/Identity/TaskFlow.Identity.Infrastructure.SqlServer/TaskFlow.Identity.Infrastructure.SqlServer.csproj", "Src/Services/Identity/TaskFlow.Identity.Infrastructure.SqlServer/"]
COPY ["Src/Shared/TaskFlow.Shared.Core/TaskFlow.Shared.Core.csproj", "Src/Shared/TaskFlow.Shared.Core/"]
COPY ["Src/Shared/TaskFlow.Shared.Logging/TaskFlow.Shared.Logging.csproj", "Src/Shared/TaskFlow.Shared.Logging/"]
COPY ["Src/Shared/TaskFlow.Shared.Messaging/TaskFlow.Shared.Messaging.csproj", "Src/Shared/TaskFlow.Shared.Messaging/"]
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]

RUN dotnet restore "Src/Services/Identity/TaskFlow.Identity.API/TaskFlow.Identity.API.csproj"
COPY . .

WORKDIR "/src/Src/Services/Identity/TaskFlow.Identity.API"
RUN dotnet build "TaskFlow.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskFlow.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

HEALTHCHECK --interval=30s --timeout=3s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "TaskFlow.Identity.API.dll"]
