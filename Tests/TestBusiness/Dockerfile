FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /app

# restore
#COPY API/API.csproj ./api/
#RUN dotnet restore api/API.csproj
#COPY BusinessTest.csproj ./tests/
#COPY bin ./tests/bin/
#RUN dotnet restore tests/BusinessTest.csproj

COPY ./bin/Publish ./tests/bin/Publish
WORKDIR "./tests/bin/Publish"

# copy src
#COPY . .

# test
#RUN dotnet test tests/BusinessTest.csproj

RUN dotnet vstest BusinessTest.dll --logger:trx
