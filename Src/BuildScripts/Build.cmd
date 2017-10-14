SET CONFIGURATION=Release

nuget restore ".\..\WMBusinessTools.sln"

SET PATH=%PATH%;"C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\"
MSBuild.exe ".\..\WMBusinessTools.sln" /p:Configuration=%CONFIGURATION%

XCOPY "..\..\Ext\Assemblies" "..\WMBusinessTools\bin\%CONFIGURATION%\" /S /Y
XCOPY "..\..\Ext\NativeBinaries" "..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\" /S /Y
XCOPY "Precompile.EF.cmd" "..\WMBusinessTools\bin\%CONFIGURATION%\" /S /Y
XCOPY "Precompile.Services.cmd" "..\WMBusinessTools\bin\%CONFIGURATION%\" /S /Y
XCOPY "..\Localization" "..\WMBusinessTools\bin\%CONFIGURATION%\Localization\" /S /Y

pause