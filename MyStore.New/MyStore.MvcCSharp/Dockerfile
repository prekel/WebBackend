FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MyStore.MvcCSharp/MyStore.MvcCSharp.csproj", "MyStore.MvcCSharp/"]
RUN dotnet restore "MyStore.MvcCSharp/MyStore.MvcCSharp.csproj"
COPY . .
WORKDIR "/src/MyStore.MvcCSharp"
RUN dotnet build "MyStore.MvcCSharp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyStore.MvcCSharp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStore.MvcCSharp.dll"]
