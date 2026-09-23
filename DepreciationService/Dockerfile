FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["DepreciationService.API/DepreciationService.API.csproj", "DepreciationService.API/"]
COPY ["DepreciationService.Application/DepreciationService.Application.csproj", "DepreciationService.Application/"]
COPY ["DepreciationService.Domain/DepreciationService.Domain.csproj", "DepreciationService.Domain/"]
COPY ["DepreciationService.Infrastructure/DepreciationService.Infrastructure.csproj", "DepreciationService.Infrastructure/"]
RUN dotnet restore "DepreciationService.API/DepreciationService.API.csproj"
COPY . .
WORKDIR /src/DepreciationService.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DepreciationService.API.dll"]
