[CmdletBinding()]
param(
    [Parameter(Position=0,Mandatory=1)][string]$url,
    [Parameter(Position=1,Mandatory=1)][string]$key
)

$scriptName = $MyInvocation.MyCommand.Name
$packages = ".\.packages"


if ([string]::IsNullOrEmpty($key)) {
    Write-Host "${scriptName}: key is empty or not set. Skipped pushing package(s)."
} else {
    Get-ChildItem $packages | ForEach-Object {
        Write-Host "$($scriptName): Pushing $($_.Name)"
        $file = "$($packages)\$($_.Name)"
        dotnet nuget push $file --source $url $key
        if ($lastexitcode -ne 0) {
            throw ("Exec: " + $errorMessage)
        }
    }
}