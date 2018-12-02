pushd "%~dp0"
powershell -file SignManifest.ps1 -sourceFilePath "Manifest.xml" -targetFilePath ".\Signed\Manifest.xml" -thumbprint "fcb671b0b83566d01aee0e142e9ba5a999208ee4"
popd