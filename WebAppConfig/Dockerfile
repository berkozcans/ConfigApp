# .NET SDK imajını kullan
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WebAppConfig/WebAppConfig.csproj", "WebAppConfig/"]
#COPY ["ConfigurationLibrary/ConfigurationLibrary.csproj", "ConfigurationLibrary/"]
RUN dotnet restore "WebAppConfig/WebAppConfig.csproj"
COPY . .
WORKDIR "/src/WebAppConfig"
RUN dotnet build "WebAppConfig.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAppConfig.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAppConfig.dll"]
