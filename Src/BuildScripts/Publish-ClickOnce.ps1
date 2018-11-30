function Publish-Folder([String]$keyPrefix, [String]$folder)
{
    Write-S3Object -Region $Region -BucketName $BucketName -KeyPrefix $keyPrefix -Folder $folder -Recurse -PublicReadOnly -Force
}

#Remove-S3Object -Region "us-east-1" -BucketName "cross-test" -KeyCollection (Get-S3Object -Region "us-east-1" -BucketName "cross-test" -KeyPrefix "tst1/Application Files" | select -ExpandProperty Key) -Force
# exe - удалить (не копировать!)
#Publish-Folder "tst1" ".\..\..\Ext\PartialTrustInstaller\publish"