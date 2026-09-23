FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["AssetService.API/AssetService.API.csproj", "AssetService.API/"]
COPY ["AssetService.Application/AssetService.Application.csproj", "AssetService.Application/"]
COPY ["AssetService.Domain/AssetService.Domain.csproj", "AssetService.Domain/"]
COPY ["AssetService.Infrastructure/AssetService.Infrastructure.csproj", "AssetService.Infrastructure/"]
RUN dotnet restore "AssetService.API/AssetService.API.csproj"
COPY . .
WORKDIR /src/AssetService.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AssetService.API.dll"]
