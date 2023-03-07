$package="Vertical.Scanner"

dotnet tool uninstall --global $package
Remove-Item -Recurse $env:USERPROFILE/.nuget/packages/$package
dotnet pack -c release -o ./.package src/$package.csproj
dotnet nuget push ./.package/$package.1.0.0.nupkg --source $env:USERPROFILE/.nuget/packages
dotnet tool install --global --add-source $env:USERPROFILE/.nuget/packages $package