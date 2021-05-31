FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MyStore.Web/MyStore.Web.fsproj", "MyStore.Web/"]
RUN dotnet restore "MyStore.Web/MyStore.Web.fsproj"
COPY . .
WORKDIR "/src/MyStore.Web"
RUN dotnet build "MyStore.Web.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyStore.Web.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStore.Web.dll"]
