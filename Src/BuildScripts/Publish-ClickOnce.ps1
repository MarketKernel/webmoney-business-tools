Import-Module AWSPowerShell

$Region = "us-east-1"
$BucketName = "webmoney-business-tools"
$PUB_DIR = ".\..\..\Ext\PartialTrustInstaller\publish"

function Publish-Folder([String]$keyPrefix, [String]$folder)
{
    Write-S3Object -Region $Region -BucketName $BucketName -KeyPrefix $keyPrefix -Folder $folder -Recurse -PublicReadOnly -Force
}

Remove-Item -Path "$($PUB_DIR)\setup.exe" -Confirm:$false -Force

Remove-S3Object -Region $Region -BucketName $BucketName -KeyCollection (Get-S3Object -Region $Region -BucketName $BucketName -KeyPrefix "dist/clickonce/Application Files" | select -ExpandProperty Key) -Force
Publish-Folder "dist/clickonce" $PUB_DIR

# Invalidating objects
$callerReference = Get-Date -UFormat "%Y%m%d%H%M%S"
New-CFInvalidation -DistributionId "E1WLSVLBT9KYI8" -InvalidationBatch_CallerReference $callerReference -Paths_Item "/dist*" -Paths_Quantity 1

pause