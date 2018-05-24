## Commands

```bash
dotnet restore src/MyWeb/MyWeb.csproj
dotnet build src/MyWeb/MyWeb.csproj
dotnet run --project src/MyWeb/MyWeb.csproj "--console"

dotnet msbuild ubuntu/UbuntuApp/UbuntuApp.csproj /t:CreateZip /p:TargetFramework=netcoreapp2.1 /p:RuntimeIdentifier=ubuntu-x64/p:Configuration=Release

dotnet deb --framework netcoreapp2.1 -r ubuntu-x64
dotnet zip --framework netcoreapp2.0 -r linux-x64

 dotnet publish --framework netcoreapp2.0 -r linux-x64
```