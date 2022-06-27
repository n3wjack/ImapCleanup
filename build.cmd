@echo off

echo.
echo **** Building self-contained winx64 exectable. ****
echo.

dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true

echo **** Executable can be found in \bin\Release\net6.0\win-x64\publish\ ****
