FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["ReportService.API/ReportService.API.csproj", "ReportService.API/"]
COPY ["ReportService.Domain/ReportService.Domain.csproj", "ReportService.Domain/"]
COPY ["ReportService.Application/ReportService.Application.csproj", "ReportService.Application/"]
COPY ["ReportService.Infrastructure/ReportService.Infrastructure.csproj", "ReportService.Infrastructure/"]
RUN dotnet restore "ReportService.API/ReportService.API.csproj"
COPY . .
WORKDIR "/src/ReportService.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReportService.API.dll"]
