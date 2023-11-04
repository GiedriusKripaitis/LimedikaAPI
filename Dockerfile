FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /code

COPY NuGet.config NuGet.config
COPY src src

RUN dotnet restore --configfile NuGet.config src/Limedika.Api.Host/Limedika.Api.Host.csproj

COPY src src

RUN dotnet clean src/Limedika.Api.Host/Limedika.Api.Host.csproj -c Release \
&& dotnet build src/Limedika.Api.Host/Limedika.Api.Host.csproj -c Release --no-restore \
&& dotnet publish src/Limedika.Api.Host/Limedika.Api.Host.csproj -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Development
# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

ENTRYPOINT ["dotnet", "Limedika.Api.Host.dll"]
