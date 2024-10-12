FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BulletinBoard.EmailSender/BulletinBoard.EmailSender.csproj", "src/BulletinBoard.EmailSender/"]
COPY ["src/BulletinBoard.Contracts/BulletinBoard.Contracts.csproj", "src/BulletinBoard.Contracts/"]
RUN dotnet restore "src/BulletinBoard.EmailSender/BulletinBoard.EmailSender.csproj"
COPY . .
WORKDIR "/src/src/BulletinBoard.EmailSender"
RUN dotnet build "BulletinBoard.EmailSender.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BulletinBoard.EmailSender.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BulletinBoard.EmailSender.dll"]
