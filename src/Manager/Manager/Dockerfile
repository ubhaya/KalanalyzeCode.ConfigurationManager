FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY "./Directory.Build.props" .
COPY ["src/Manager/IdentityServer/IdentityServer.csproj", "src/Manager/IdentityServer/"]
COPY ["src/Manager/Identity/Identity.csproj", "src/Manager/Identity/"]
RUN dotnet restore "src/Manager/IdentityServer/IdentityServer.csproj"
COPY . .
WORKDIR "/src/src/Manager/IdentityServer"
RUN dotnet build "IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.dll"]

ENV ASPNETCORE_URLS="https://+:5001;http://+:5000"
