#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app

ENV DOTNET_URLS=http://+:6000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrderApi.csproj", "."]
RUN dotnet restore "./ConfigurationLibrary.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ConfigurationLibrary.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigurationLibrary.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationLibrary.dll"]
