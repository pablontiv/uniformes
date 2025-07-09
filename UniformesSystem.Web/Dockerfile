FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UniformesSystem.Web/UniformesSystem.Web.csproj", "UniformesSystem.Web/"]
COPY ["UniformesSystem.Database/UniformesSystem.Database.csproj", "UniformesSystem.Database/"]
COPY ["UniformesSystem.API/UniformesSystem.API.csproj", "UniformesSystem.API/"]
RUN dotnet restore "UniformesSystem.Web/UniformesSystem.Web.csproj"
COPY . .
WORKDIR "/src/UniformesSystem.Web"
RUN dotnet build "UniformesSystem.Web.csproj" -c Release -o /app/build

FROM build AS publish
# Use ExcludeConfigFilesFromPublishDir to prevent conflicts with the API project
RUN dotnet publish "UniformesSystem.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false -p:CopyOutputSymbolsToPublishDirectory=false -p:ExcludeConfigFilesFromBuildOutput=false

FROM base AS final
WORKDIR /app
# Copy the published app
COPY --from=publish /app/publish .
# Copy config files explicitly to ensure they're in the right location
COPY UniformesSystem.Web/appsettings*.json .
ENTRYPOINT ["dotnet", "UniformesSystem.Web.dll"]
