$identityServer = Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project src/Manager/IdentityServer/IdentityServer.csproj" -PassThru
$webApi = Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project src/Manager/Api/Api.csproj" -PassThru
$webUi = Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run --project src/Manager/Ui/Ui.csproj" -PassThru

Wait-Process -InputObject $identityServer, $webApi, $webUi

Write-Output "All projects have been run."