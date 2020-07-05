@echo off

echo.
echo **** Building self-contained winx64 exectable. ****
echo.

dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
