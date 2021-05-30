FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MyStore.MvcFSharp/MyStore.MvcFSharp.fsproj", "MyStore.MvcFSharp/"]
RUN dotnet restore "MyStore.MvcFSharp/MyStore.MvcFSharp.fsproj"
COPY . .
WORKDIR "/src/MyStore.MvcFSharp"
RUN dotnet build "MyStore.MvcFSharp.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyStore.MvcFSharp.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyStore.MvcFSharp.dll"]
