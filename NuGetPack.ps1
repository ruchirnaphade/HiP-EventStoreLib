$csproj = (ls HiP-EventStoreLib\*.csproj).FullName
Switch ("$env:Build_SourceBranchName")
{
    "master" { dotnet pack "$csproj" -o . }
    "develop" { dotnet pack "$csproj" -o . --version-suffix "develop" }
    default { exit }
}
$nupkg = (ls HiP-EventStoreLib\*.nupkg).FullName
dotnet nuget push "$nupkg" -k "$env:MyGetKey" -s "$env:NuGetFeed"
$LASTEXITCODE = 0