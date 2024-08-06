dotnet run --project src/Manager/IdentityServer/IdentityServer.csproj &
dotnet run --project src/Manager/Api/Api.csproj &
dotnet run --project src/Manager/Ui/Ui.csproj &

dotnet test KalanalyzeCode.ConfigurationManager.sln --no-build -c Release

wait

echo "All projects have been run."