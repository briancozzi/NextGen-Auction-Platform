#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/NextGen.BiddingPlatform.Migrator/NextGen.BiddingPlatform.Migrator.csproj", "src/NextGen.BiddingPlatform.Migrator/"]
COPY ["src/NextGen.BiddingPlatform.EntityFrameworkCore/NextGen.BiddingPlatform.EntityFrameworkCore.csproj", "src/NextGen.BiddingPlatform.EntityFrameworkCore/"]
COPY ["src/NextGen.BiddingPlatform.Core/NextGen.BiddingPlatform.Core.csproj", "src/NextGen.BiddingPlatform.Core/"]
COPY ["src/NextGen.BiddingPlatform.Core.Shared/NextGen.BiddingPlatform.Core.Shared.csproj", "src/NextGen.BiddingPlatform.Core.Shared/"]
RUN dotnet restore "src/NextGen.BiddingPlatform.Migrator/NextGen.BiddingPlatform.Migrator.csproj"
COPY . .
WORKDIR "/src/src/NextGen.BiddingPlatform.Migrator"
RUN dotnet build "NextGen.BiddingPlatform.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NextGen.BiddingPlatform.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NextGen.BiddingPlatform.Migrator.dll"]
