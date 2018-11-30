SET WIX_DIR="C:\Program Files (x86)\WiX Toolset v3.11\bin\"
SET SIGNTOOL_DIR="C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\"
SET OUT_FOLDER=Output
SET OUT_FILE_X86=WMBusinessTools.Setup-x32.msi
SET OUT_FILE_X64=WMBusinessTools.Setup-x64.msi
SET CertThumbprint="fcb671b0b83566d01aee0e142e9ba5a999208ee4"
SET TimeStampingServer="http://timestamp.comodoca.com/rfc3161"

Pushd .\..\WMBusinessTools.Setup.WiX

DEL %OUT_FOLDER%\*.wixobj
DEL %OUT_FOLDER%\*.wixpdb
DEL %OUT_FOLDER%\*.msi

SET PATH=%PATH%;%WIX_DIR%;%SIGNTOOL_DIR%
SET OUT_FILE_PATH=%OUT_FOLDER%\%OUT_FILE_X86%

candle.exe Components.wxs -dPlatform="x86" -out %OUT_FOLDER%\Components.wixobj
candle.exe Product.wxs -dPlatform="x86" -out %OUT_FOLDER%\Product.wixobj
light.exe %OUT_FOLDER%\Components.wixobj %OUT_FOLDER%\Product.wixobj -cultures:ru-ru -loc Translations\Lang.ru-ru.wxl -ext WixUIExtension -ext WixUtilExtension -ext WixNetFxExtension -out %OUT_FILE_PATH%

signtool.exe sign /sha1 %CertThumbprint% /t %TimeStampingServer% /d "WMBusinessTools" /du "https://www.webmoney-business-tools.com/" %OUT_FILE_PATH%

DEL %OUT_FOLDER%\*.wixobj
DEL %OUT_FOLDER%\*.wixpdb

SET OUT_FILE_PATH=%OUT_FOLDER%\%OUT_FILE_X64%

candle.exe Components.wxs -dPlatform="x64" -out %OUT_FOLDER%\Components.wixobj
candle.exe Product.wxs -dPlatform="x64" -out %OUT_FOLDER%\Product.wixobj
light.exe %OUT_FOLDER%\Components.wixobj %OUT_FOLDER%\Product.wixobj -cultures:ru-ru -loc Translations\Lang.ru-ru.wxl -ext WixUIExtension -ext WixUtilExtension -ext WixNetFxExtension -out %OUT_FILE_PATH%

signtool.exe sign /sha1 %CertThumbprint% /t %TimeStampingServer% /d "WMBusinessTools" /du "https://www.webmoney-business-tools.com/" %OUT_FILE_PATH%

popd