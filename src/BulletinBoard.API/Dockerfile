FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BulletinBoard.API/BulletinBoard.API.csproj", "src/BulletinBoard.API/"]
COPY ["src/BulletinBoard.DataAccess/BulletinBoard.DataAccess.csproj", "src/BulletinBoard.DataAccess/"]
COPY ["src/BulletinBoard.Contracts/BulletinBoard.Contracts.csproj", "src/BulletinBoard.Contracts/"]
COPY ["src/BulletinBoard.Domain/BulletinBoard.Domain.csproj", "src/BulletinBoard.Domain/"]
COPY ["src/BulletinBoard.Infrastructure/BulletinBoard.Infrastructure.csproj", "src/BulletinBoard.Infrastructure/"]
COPY ["src/BulletinBoard.AppServices/BulletinBoard.AppServices.csproj", "src/BulletinBoard.AppServices/"]
COPY ["src/BulletinBoard.ComponentRegistrar/BulletinBoard.ComponentRegistrar.csproj", "src/BulletinBoard.ComponentRegistrar/"]
RUN dotnet restore "src/BulletinBoard.API/BulletinBoard.API.csproj"
COPY . .
WORKDIR "/src/src/BulletinBoard.API"
RUN dotnet build "BulletinBoard.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BulletinBoard.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.API.dll"]
