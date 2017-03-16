dotnet publish -c Release
copy bin\Release\netcoreapp1.1\Nether.Web.xml bin\Release\netcoreapp1.1\publish

docker build -t netherweb .