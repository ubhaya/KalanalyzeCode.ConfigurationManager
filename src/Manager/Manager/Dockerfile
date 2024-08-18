FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY "./Directory.Build.props" .
COPY "./Directory.Packages.props" .
COPY ["src/Manager/Manager/Manager.csproj", "src/Manager/Manager/"]
COPY ["src/Manager/Application/Application.csproj", "src/Manager/Application/"]
COPY ["src/Manager/Entity/Entity.csproj", "src/Manager/Entity/"]

RUN dotnet restore "src/Manager/Entity/Entity.csproj"
RUN dotnet restore "src/Manager/Application/Application.csproj"
RUN dotnet restore "src/Manager/Manager/Manager.csproj"
COPY . .
WORKDIR "/src/src/Manager/Manager"
RUN dotnet build "Manager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Manager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KalanalyzeCode.ConfigurationManager.Ui.dll"]

ENV ASPNETCORE_URLS="https://+:7206;http://+:5225"
ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false
