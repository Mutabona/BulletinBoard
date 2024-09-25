FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BulletinBoard.DbMigrator/BulletinBoard.DbMigrator.csproj", "src/BulletinBoard.DbMigrator/"]
COPY ["src/BulletinBoard.Contracts/BulletinBoard.Contracts.csproj", "src/BulletinBoard.Contracts/"]
COPY ["src/BulletinBoard.AppServices/BulletinBoard.AppServices.csproj", "src/BulletinBoard.AppServices/"]
COPY ["src/BulletinBoard.Domain/BulletinBoard.Domain.csproj", "src/BulletinBoard.Domain/"]
COPY ["src/BulletinBoard.DataAccess/BulletinBoard.DataAccess.csproj", "src/BulletinBoard.DataAccess/"]
COPY ["src/BulletinBoard.Infrastructure/BulletinBoard.Infrastructure.csproj", "src/BulletinBoard.Infrastructure/"]
RUN dotnet restore "src/BulletinBoard.DbMigrator/BulletinBoard.DbMigrator.csproj"
COPY . .
WORKDIR "/src/src/BulletinBoard.DbMigrator"
RUN dotnet build "BulletinBoard.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BulletinBoard.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.DbMigrator.dll"]
