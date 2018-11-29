Add-Type -Assembly "System.IO.Compression.FileSystem";

$BIN_FOLDER = ".\..\WMBusinessTools\bin"

cmd.exe /c ".\Build.Production.cmd"

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

[xml]$manifestDocument = Get-Content -Path ".\..\..\Tools\ManifestSigner\Manifest.xml"
$packageElement = $manifestDocument.SelectSingleNode("Manifest/Package[@OSMajorVersion='6']")
$packageElement.Attributes["Version"].Value = $version
$packageElement.Attributes["ReleaseDate"].Value = $releaseDate
$packageElement.Attributes["Digest"].Value = $archiveHash
$packageElement.InnerText = "http://www.webmoney-business-tools.com/dist/$publicationFolder/WMBusinessTools-v$version.zip"
$manifestDocument.Save(".\..\..\Tools\ManifestSigner\Manifest.xml")

pause