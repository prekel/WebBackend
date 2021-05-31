FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MyStore.WebApi/MyStore.WebApi.csproj", "MyStore.WebApi/"]
RUN dotnet restore "MyStore.WebApi/MyStore.WebApi.csproj"
COPY . .
WORKDIR "/src/MyStore.WebApi"
RUN dotnet build "MyStore.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyStore.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStore.WebApi.dll"]
