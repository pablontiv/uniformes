FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update && apt-get install -y wget

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UniformesSystem.API/UniformesSystem.API.csproj", "UniformesSystem.API/"]
COPY ["UniformesSystem.Database/UniformesSystem.Database.csproj", "UniformesSystem.Database/"]
RUN dotnet restore "UniformesSystem.API/UniformesSystem.API.csproj"
COPY . .
WORKDIR "/src/UniformesSystem.API"
RUN dotnet build "UniformesSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
# Use ExcludeConfigFilesFromPublishDir to prevent conflicts with the Web project
RUN dotnet publish "UniformesSystem.API.csproj" -c Release -o /app/publish /p:UseAppHost=false -p:CopyOutputSymbolsToPublishDirectory=false -p:ExcludeConfigFilesFromBuildOutput=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy config files explicitly to ensure they're in the right location
COPY UniformesSystem.API/appsettings*.json .
ENTRYPOINT ["dotnet", "UniformesSystem.API.dll"]
