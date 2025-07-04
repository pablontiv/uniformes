FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UniformesSystem.API/UniformesSystem.API.csproj", "UniformesSystem.API/"]
COPY ["UniformesSystem.Database/UniformesSystem.Database.csproj", "UniformesSystem.Database/"]
RUN dotnet restore "UniformesSystem.API/UniformesSystem.API.csproj"
COPY . .
WORKDIR "/src/UniformesSystem.API"
RUN dotnet build "UniformesSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniformesSystem.API.csproj" -c Release -o /app/publish

FROM build as dbchecker
WORKDIR "/src/UniformesSystem.API"
RUN dotnet add package Microsoft.Extensions.Configuration.Json
RUN dotnet build -c Release -o /app/dbchecker

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=dbchecker /app/dbchecker/UniformesSystem.API.dll /app/dbchecker/
COPY --from=dbchecker /app/dbchecker/UniformesSystem.API.runtimeconfig.json /app/dbchecker/
COPY --from=dbchecker /app/dbchecker/UniformesSystem.API.deps.json /app/dbchecker/
COPY --from=dbchecker /app/dbchecker/UniformesSystem.Database.dll /app/dbchecker/
ENTRYPOINT ["dotnet", "UniformesSystem.API.dll"]
