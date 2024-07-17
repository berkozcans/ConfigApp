# 1. Aşama: Derleme
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production
# Proje dosyalarını kopyala ve restore et
COPY WebAppConfig/*.csproj WebAppConfig/
COPY ConfigurationLibrary/*.csproj ConfigurationLibrary/
RUN dotnet restore WebAppConfig/WebAppConfig.csproj

# Diğer dosyaları kopyala ve yayımla
COPY . .
WORKDIR /app/WebAppConfig
RUN dotnet publish -c Release -o /app/publish

# 2. Aşama: Çalıştırma
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "WebAppConfig.dll"]
