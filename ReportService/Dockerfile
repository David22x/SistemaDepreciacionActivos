FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ReportService/ReportService.API/ReportService.API.csproj", "ReportService/ReportService.API/"]
COPY ["ReportService/ReportService.Domain/ReportService.Domain.csproj", "ReportService/ReportService.Domain/"]
COPY ["ReportService/ReportService.Infrastructure/ReportService.Infrastructure.csproj", "ReportService/ReportService.Infrastructure/"]
RUN dotnet restore "ReportService/ReportService.API/ReportService.API.csproj"
COPY . .
WORKDIR "/src/ReportService/ReportService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReportService.API.dll"]