Add-Type -Assembly "System.IO.Compression.FileSystem";

$DEV_PKT = "236f20e4200c621f"
$PROD_PKT = "d1bcab3940c39a1a"
$BIN_FOLDER = ".\..\WMBusinessTools\bin"

cmd.exe /c ".\Build.Production.cmd"

(Get-Content $BIN_FOLDER\Production\SupportAssistant\Extensions.json) | ForEach-Object { $_ -replace $DEV_PKT, $PROD_PKT } | Set-Content $BIN_FOLDER\Production\SupportAssistant\Extensions.json
(Get-Content $BIN_FOLDER\Production\BasicExtensions\Extensions.json) | ForEach-Object { $_ -replace $DEV_PKT, $PROD_PKT } | Set-Content $BIN_FOLDER\Production\BasicExtensions\Extensions.json
(Get-Content $BIN_FOLDER\Production\WebMoney.Services\Extensions.json) | ForEach-Object { $_ -replace $DEV_PKT, $PROD_PKT } | Set-Content $BIN_FOLDER\Production\WebMoney.Services\Extensions.json

# App.config
[xml]$configDocument = Get-Content -Path "$BIN_FOLDER\Production\WMBusinessTools.exe.config"
$configDocument.SelectSingleNode("configuration/appSettings/add[@key='TranslationMode']").Attributes["value"].Value = "Off"
$configDocument.SelectSingleNode("configuration/appSettings/add[@key='OmitPrecompilation']").Attributes["value"].Value = "False"
$configDocument.Save("$BIN_FOLDER\Production\WMBusinessTools.exe.config")

# Log4Net.config
[xml]$log4NetDocument = Get-Content -Path "$BIN_FOLDER\Production\Log4Net.config"
$log4NetDocument.SelectSingleNode("configuration/log4net/root/priority").Attributes["value"].Value = "ERROR"
$udpAppenderElement = $log4NetDocument.SelectSingleNode("configuration/log4net/root/appender-ref[@ref='UdpAppender']")

if ($udpAppenderElement)
{
    $parent = $udpAppenderElement.ParentNode
    [void] $parent.RemoveChild($udpAppenderElement)
}

$log4NetDocument.Save("$BIN_FOLDER\Production\Log4Net.config")

$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$BIN_FOLDER\Production\WMBusinessTools.exe").FileVersion
$archiveFilePath = "$BIN_FOLDER\WMBusinessTools-v$version.zip";

[System.IO.Compression.ZipFile]::CreateFromDirectory("$BIN_FOLDER\Production", $archiveFilePath)

# Подготовка App.config для MSI
[xml]$configDocument = Get-Content -Path "$BIN_FOLDER\Production\WMBusinessTools.exe.config"
$configDocument.SelectSingleNode("configuration/appSettings/add[@key='OmitPrecompilation']").Attributes["value"].Value = "True"
$configDocument.Save("$BIN_FOLDER\Production\WMBusinessTools.exe.config")

# Подготовка манифеста
$archiveHash = (Get-FileHash -Path $archiveFilePath -Algorithm SHA256).Hash
$releaseDate = Get-Date -UFormat "%m/%d/%Y"
$publicationFolder = "v" + ($version -replace "\.", "-")

[xml]$manifestDocument = Get-Content -Path ".\..\..\..\WMBT-4.5\Tools\ManifestSigner\Manifest.xml"
$packageElement = $manifestDocument.SelectSingleNode("Manifest/Package[@OSMajorVersion='5']")
$packageElement.Attributes["Version"].Value = $version
$packageElement.Attributes["ReleaseDate"].Value = $releaseDate
$packageElement.Attributes["Digest"].Value = $archiveHash
$packageElement.InnerText = "http://www.webmoney-business-tools.com/dist/$publicationFolder/WMBusinessTools-v$version.zip"
$manifestDocument.Save(".\..\..\..\WMBT-4.5\Tools\ManifestSigner\Manifest.xml")

pause