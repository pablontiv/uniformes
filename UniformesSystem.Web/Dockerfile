FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UniformesSystem.Web/UniformesSystem.Web.csproj", "UniformesSystem.Web/"]
COPY ["UniformesSystem.API/UniformesSystem.API.csproj", "UniformesSystem.API/"]
COPY ["UniformesSystem.Database/UniformesSystem.Database.csproj", "UniformesSystem.Database/"]
RUN dotnet restore "UniformesSystem.Web/UniformesSystem.Web.csproj"
COPY . .
WORKDIR "/src/UniformesSystem.Web"
RUN dotnet build "UniformesSystem.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniformesSystem.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniformesSystem.Web.dll"]
