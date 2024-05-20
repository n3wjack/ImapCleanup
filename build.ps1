$version = "2.0.1"

Write-Host "`n*** Building self-contained winx64 exectable. ***`n" -ForegroundColor Cyan

pushd

cd ImapCleanup

Write-Host "Run regular build...." -ForegroundColor Cyan
dotnet publish

Write-Host "Run self-contained build..." -ForegroundColor Cyan
dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true -p:TrimMode=partial


Write-Host "Create archive regular build..." -ForegroundColor Cyan
pushd
cd bin\Release\net8.0\publish\
7z a  "ImapCleanup-$version.zip"  *.*

Write-Host "Smoke test" -ForegroundColor Cyan

# Run the exe, see if it shows the help info.
.\ImapCleanup.exe /?
.\ImapCleanup.exe --version

popd

Write-Host "Create archive for self contained build..." -ForegroundColor Cyan
pushd
cd bin\Release\net8.0\win-x64\publish
7z a "ImapCleanup-$version-Windowsx64-self-contained.zip" ImapCleanup.exe

Write-Host "Smoke test" -ForegroundColor Cyan

# Run the exe, see if it shows the help info.
.\ImapCleanup.exe /?
.\ImapCleanup.exe --version

popd

popd
Write-Host "Moving archives to current directory..." -ForegroundColor Cyan
ls *.zip -r | % { move $_ . }
