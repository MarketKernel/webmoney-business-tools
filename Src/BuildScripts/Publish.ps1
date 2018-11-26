Import-Module AWSPowerShell

$Region = "us-east-1"
#$BucketName = "webmoney-business-tools"
$BucketName = "cross-test"

function Publish-File([String]$key, [String]$file)
{
    Write-S3Object -Region $Region -BucketName $BucketName -Key $key -File $file -PublicReadOnly -Force

    #$user = New-Object -TypeName Amazon.S3.Model.S3Grantee
    #$user.URI = "http://acs.amazonaws.com/groups/global/AllUsers"
 
    #$grantRead = New-Object -TypeName Amazon.S3.Model.S3Grant
    #$grantRead.Grantee = $user
    #$grantRead.Permission = [Amazon.S3.S3Permission]::READ

    #$acl = Get-S3Acl -Region $Region -BucketName $BucketName
    #$acl.Grants += @($grantRead)

    #Set-S3Acl -Region $Region -BucketName $BucketName -Key $key -Grant $acl.Grants -OwnerId $acl.Owner.Id
}

function Publish-Folder([String]$keyPrefix, [String]$folder)
{
    Write-S3Object -Region $Region -BucketName $BucketName -KeyPrefix $keyPrefix -Folder $folder -Recurse -PublicReadOnly -Force

    #$user = New-Object -TypeName Amazon.S3.Model.S3Grantee
    #$user.URI = "http://acs.amazonaws.com/groups/global/AllUsers"
 
    #$grantRead = New-Object -TypeName Amazon.S3.Model.S3Grant
    #$grantRead.Grantee = $user
    #$grantRead.Permission = [Amazon.S3.S3Permission]::READ

    #$acl = Get-S3Acl -Region $Region -BucketName $BucketName
    #$acl.Grants += @($grantRead)

    #Set-S3Acl -Region $Region -BucketName $BucketName -Key $key -Grant $acl.Grants -OwnerId $acl.Owner.Id
}

Remove-S3Object -Region "us-east-1" -BucketName "cross-test" -KeyCollection (Get-S3Object -Region "us-east-1" -BucketName "cross-test" -KeyPrefix "tst1/Application Files" | select -ExpandProperty Key) -Force
# exe - удалить (не копировать!)
Publish-Folder "tst1" ".\..\..\Ext\PartialTrustInstaller\publish"

# 1. Публикуем ZIP-архивы
#Publish-File $BucketName "pslfld/WMBusinessTools-v3.1.1.0.zip" ".\..\WMBusinessTools\bin\WMBusinessTools-v3.1.1.0.zip"

# 2. Публикуем файл манифеста
#Publish-File $BucketName "dist/Manifest.xml" ".\..\..\Tools\ManifestSigner\Signed\Manifest.xml"

# 3. Публикуем MSI

# 4. Invalidating objects
#$callerReference = Get-Date -UFormat "%Y%m%d%H%M%S"
#New-CFInvalidation -DistributionId "E1WLSVLBT9KYI8" -InvalidationBatch_CallerReference $callerReference -Paths_Item "/dist*" -Paths_Quantity 1
pause