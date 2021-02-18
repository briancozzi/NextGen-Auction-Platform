#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/NextGen.BiddingPlatform.Web.Public/NextGen.BiddingPlatform.Web.Public.csproj", "src/NextGen.BiddingPlatform.Web.Public/"]
COPY ["src/NextGen.BiddingPlatform.Web.Core/NextGen.BiddingPlatform.Web.Core.csproj", "src/NextGen.BiddingPlatform.Web.Core/"]
COPY ["src/NextGen.BiddingPlatform.Application/NextGen.BiddingPlatform.Application.csproj", "src/NextGen.BiddingPlatform.Application/"]
COPY ["src/NextGen.BiddingPlatform.Application.Shared/NextGen.BiddingPlatform.Application.Shared.csproj", "src/NextGen.BiddingPlatform.Application.Shared/"]
COPY ["src/NextGen.BiddingPlatform.Core.Shared/NextGen.BiddingPlatform.Core.Shared.csproj", "src/NextGen.BiddingPlatform.Core.Shared/"]
COPY ["src/NextGen.BiddingPlatform.Core/NextGen.BiddingPlatform.Core.csproj", "src/NextGen.BiddingPlatform.Core/"]
COPY ["src/NextGen.BiddingPlatform.EntityFrameworkCore/NextGen.BiddingPlatform.EntityFrameworkCore.csproj", "src/NextGen.BiddingPlatform.EntityFrameworkCore/"]
COPY ["src/NextGen.BiddingPlatform.GraphQL/NextGen.BiddingPlatform.GraphQL.csproj", "src/NextGen.BiddingPlatform.GraphQL/"]
RUN dotnet restore "src/NextGen.BiddingPlatform.Web.Public/NextGen.BiddingPlatform.Web.Public.csproj"
COPY . .
WORKDIR "/src/src/NextGen.BiddingPlatform.Web.Public"
RUN dotnet build "NextGen.BiddingPlatform.Web.Public.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NextGen.BiddingPlatform.Web.Public.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NextGen.BiddingPlatform.Web.Public.dll"]
