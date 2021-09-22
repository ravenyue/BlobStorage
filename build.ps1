$packages = ".\.packages"

if(Test-Path $packages) { Remove-Item $packages -Force -Recurse }

$libraries = Get-ChildItem .\src\BlobStorage*

foreach ($library in $libraries) {
    dotnet pack $library -c Release -o $packages --no-build
}