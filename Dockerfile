FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["/API", "./API"]
COPY ["/SmartHouse.Domain.Core", "./SmartHouse.Domain.Core"]
COPY ["/SmartHouse.Domain.Interfaces", "./SmartHouse.Domain.Interfaces"]
COPY ["/SmartHouse.Infrastructure.Business", "./SmartHouse.Infrastructure.Business"]
COPY ["/SmartHouse.Infrastructure.Data", "./SmartHouse.Infrastructure.Data"]
COPY ["/SmartHouse.Services.Interfaces", "./SmartHouse.Services.Interfaces"]
COPY ["/Weather", "./Weather"]

RUN dotnet restore "API/SmartHouseAPI/SmartHouseAPI.csproj"
COPY . .
WORKDIR "API/SmartHouseAPI"
RUN dotnet build "SmartHouseAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartHouseAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartHouseAPI.dll"]