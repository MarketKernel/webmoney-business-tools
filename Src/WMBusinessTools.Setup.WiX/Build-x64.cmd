SET WIX_DIR="C:\Program Files (x86)\WiX Toolset v3.11\bin\"
SET OUT_DIR=Output
SET OUT_FILE=WMBusinessTools.Setup-x64.msi

DEL %OUT_DIR%\*.wixobj
DEL %OUT_DIR%\*.wixpdb
DEL %OUT_DIR%\*.msi

SET PATH=%PATH%;%WIX_DIR%
SET OUT_FILE_PATH=%OUT_DIR%\%OUT_FILE%

candle.exe Components.wxs -dPlatform="x64" -out %OUT_DIR%\Components.wixobj
candle.exe Product.wxs -dPlatform="x64" -out %OUT_DIR%\Product.wixobj
light.exe %OUT_DIR%\Components.wixobj %OUT_DIR%\Product.wixobj -cultures:ru-ru -loc Translations\Lang.ru-ru.wxl -ext WixUIExtension -ext WixUtilExtension -ext WixNetFxExtension -out %OUT_FILE_PATH%
pause