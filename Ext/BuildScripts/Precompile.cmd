@echo off

pushd "%~dp0"

SET NETFX=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319

IF NOT EXIST %NETFX% SET NETFX=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319

SET NGEN=%NETFX%\ngen.exe

%NGEN% install EntityFramework.dll

popd