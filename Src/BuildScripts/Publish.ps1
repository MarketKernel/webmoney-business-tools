Add-Type -Assembly "System.IO.Compression.FileSystem";

Import-Module AWSPowerShell

$Region = "us-east-1"
$BucketName = "webmoney-business-tools"
$BIN_FOLDER = ".\..\WMBusinessTools\bin"
$MSI_FOLDER = ".\..\WMBusinessTools.Setup.WiX\Output"

function Publish-File([String]$key, [String]$file)
{
    Write-S3Object -Region $Region -BucketName $BucketName -Key $key -File $file -PublicReadOnly -Force
}

$version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$BIN_FOLDER\Production\WMBusinessTools.exe").FileVersion
$publicationFolder = "v" + ($version -replace "\.", "-")

# 1. Публикуем ZIP-архив
Publish-File "dist/$($publicationFolder)/WMBusinessTools-v$($version).zip" "$($BIN_FOLDER)\WMBusinessTools-v$($version).zip"

# 2. Публикуем файл манифеста
Publish-File "dist/Manifest.xml" ".\..\..\Tools\ManifestSigner\Signed\Manifest.xml"

# 3. Публикуем MSI
Publish-File "dist/$($publicationFolder)/WMBusinessTools.Setup-x32.msi" "$($MSI_FOLDER)\WMBusinessTools.Setup-x32.msi"
Publish-File "dist/$($publicationFolder)/WMBusinessTools.Setup-x64.msi" "$($MSI_FOLDER)\WMBusinessTools.Setup-x64.msi"

# 4. Invalidating objects
$callerReference = Get-Date -UFormat "%Y%m%d%H%M%S"
New-CFInvalidation -DistributionId "E1WLSVLBT9KYI8" -InvalidationBatch_CallerReference $callerReference -Paths_Item "/dist*" -Paths_Quantity 1

pause