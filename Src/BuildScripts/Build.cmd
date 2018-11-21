IF NOT DEFINED CONFIGURATION (
SET CONFIGURATION=Release
)

nuget restore ".\..\WMBusinessTools[.Net4.0].sln"

SET MSBUILD_DIR="C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin"

IF NOT EXIST %MSBUILD_DIR% SET MSBUILD_DIR="C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin"

SET PATH=%PATH%;%MSBUILD_DIR%

MSBuild.exe ".\..\WMBusinessTools[.Net4.0].sln" /p:Configuration=%CONFIGURATION%

XCOPY "..\..\Ext\NativeBinaries" "..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\" /S /Y
XCOPY "..\Localization" "..\WMBusinessTools\bin\%CONFIGURATION%\Localization\" /S /Y

REM SupportAssistant
move /y .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\ru\* .\..\WMBusinessTools\bin\%CONFIGURATION%\ru

move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.Azure.KeyVault.Core.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.Data.Edm.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.Data.OData.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.Data.Services.Client.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.Practices.ServiceLocation.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.Practices.Unity.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Microsoft.WindowsAzure.Storage.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\SupportAssistant.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\System.Spatial.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\

rmdir .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\ru
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\de
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\es
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\fr
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\it
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\ja
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\ko
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\zh-Hans
rmdir /s /q .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\zh-Hant
del .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\*.dll
del .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\*.pdb
del .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\*.xml
del .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\*.config

REM WebMoney.Services
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\amd64 .\..\WMBusinessTools\bin\%CONFIGURATION%\
move /y .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\ru\* .\..\WMBusinessTools\bin\%CONFIGURATION%\ru
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\TrustedCertificates .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\x86 .\..\WMBusinessTools\bin\%CONFIGURATION%\

move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\AutoMapper.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\ClosedXML.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\DocumentFormat.OpenXml.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\EntityFramework.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\EntityFramework.SqlServer.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\EntityFramework.SqlServerCompact.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\System.Data.SqlServerCe.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\System.IO.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\System.Runtime.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\System.Threading.Tasks.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\WebMoney.Cryptography.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\WebMoney.Services.Contracts.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\WebMoney.Services.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\WebMoney.XmlInterfaces.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\

rmdir .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\ru
del .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\*.dll
del .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\*.pdb
del .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\*.xml
del .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\*.config

REM BasicExtensions
move /y .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\ru\* .\..\WMBusinessTools\bin\%CONFIGURATION%\ru

move .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\WMBusinessTools.Extensions.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\WMBusinessTools.Extensions.dll.config .\..\WMBusinessTools\bin\%CONFIGURATION%\
move .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\Xml2WinForms.dll .\..\WMBusinessTools\bin\%CONFIGURATION%\

rmdir .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\ru
del .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\*.dll
del .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\*.pdb
del .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\*.xml
del .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\*.config

REM WMBusinessTools
del .\..\WMBusinessTools\bin\%CONFIGURATION%\*.pdb
del .\..\WMBusinessTools\bin\%CONFIGURATION%\*.xml