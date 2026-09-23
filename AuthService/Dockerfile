# ETAPA 1: Construcción (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar los archivos .csproj de las 4 capas
COPY ["AuthService.API/AuthService.API.csproj", "AuthService.API/"]
COPY ["AuthService.Application/AuthService.Application.csproj", "AuthService.Application/"]
COPY ["AuthService.Domain/AuthService.Domain.csproj", "AuthService.Domain/"]
COPY ["AuthService.Infrastructure/AuthService.Infrastructure.csproj", "AuthService.Infrastructure/"]

# Restaurar dependencias
RUN dotnet restore "AuthService.API/AuthService.API.csproj"

# Copiar el resto del código
COPY . .
WORKDIR "/src/AuthService.API"

# Publicar en modo Release
RUN dotnet publish "AuthService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ETAPA 2: Ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Configurar el puerto interno del contenedor
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "AuthService.API.dll"]