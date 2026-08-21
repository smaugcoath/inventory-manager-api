#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/InventoryManager.WebApi.Host/InventoryManager.WebApi.Host.csproj", "src/InventoryManager.WebApi.Host/"]
COPY ["src/InventoryManager.WebApi.Mvc/InventoryManager.WebApi.Mvc.csproj", "src/InventoryManager.WebApi.Mvc/"]
COPY ["src/InventoryManager.WebApi.Business/InventoryManager.WebApi.Business.csproj", "src/InventoryManager.WebApi.Business/"]
COPY ["src/InventoryManager.WebApi.Data/InventoryManager.WebApi.Data.csproj", "src/InventoryManager.WebApi.Data/"]
COPY ["src/InventoryManager.WebApi.Infrastructure/InventoryManager.WebApi.Infrastructure.csproj", "src/InventoryManager.WebApi.Infrastructure/"]
RUN dotnet restore "src/InventoryManager.WebApi.Host/InventoryManager.WebApi.Host.csproj"
COPY . .
WORKDIR "/src/src/InventoryManager.WebApi.Host"
RUN dotnet build "InventoryManager.WebApi.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InventoryManager.WebApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventoryManager.WebApi.Host.dll"]