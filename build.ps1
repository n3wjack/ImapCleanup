param([Parameter(Mandatory)]$version)

$buildFolder = "bin\Release\net10.0"

# --------------------------------------------------------------------------------------

Write-Host "`n*** Building ***`n" -ForegroundColor Cyan

pushd

cd ImapCleanup

Write-Host "Run regular build...." -ForegroundColor Cyan
dotnet publish

Write-Host "Run self-contained build..." -ForegroundColor Cyan
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true -p:TrimMode=partial

Write-Host "Create archive from regular build..." -ForegroundColor Cyan
pushd
cd (Join-Path $buildFolder "publish")
7z a  "ImapCleanup-$version.zip"  *.*

popd

Write-Host "Create archive for self contained build..." -ForegroundColor Cyan
pushd
cd (Join-Path $buildFolder "win-x64\publish")
7z a "ImapCleanup-$version-Windowsx64-self-contained.zip" ImapCleanup.exe

Write-Host "Smoke test self-container image" -ForegroundColor Cyan

# Run the exe, see if it shows the help info.
.\ImapCleanup.exe /?
.\ImapCleanup.exe --version

popd
popd

Write-Host "Create Docker image..." -ForegroundColor Cyan
docker build . -t n3wjack/imapcleanup:latest 
docker build . -t n3wjack/imapcleanup:$version 

Write-Host "Smoke test Docker image" -ForegroundColor Cyan

docker run n3wjack/imapcleanup --help
docker run n3wjack/imapcleanup --version

Write-Host "Moving archives to current directory..." -ForegroundColor Cyan
ls *.zip -r | % { move $_ . }

