FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Gateway/Gateway/Gateway.csproj", "Gateway/Gateway/"]
RUN dotnet restore "Gateway/Gateway/Gateway.csproj"
COPY . .
WORKDIR "/src/Gateway/Gateway"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gateway.dll"]