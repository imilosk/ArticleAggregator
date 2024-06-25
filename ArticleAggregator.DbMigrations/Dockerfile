FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ArticleAggregator.DbMigrations/ArticleAggregator.DbMigrations.csproj", "ArticleAggregator.DbMigrations/"]
RUN dotnet restore "ArticleAggregator.DbMigrations/ArticleAggregator.DbMigrations.csproj"
COPY . .
WORKDIR "/src/ArticleAggregator.DbMigrations"
RUN dotnet build "ArticleAggregator.DbMigrations.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ArticleAggregator.DbMigrations.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArticleAggregator.DbMigrations.dll"]
