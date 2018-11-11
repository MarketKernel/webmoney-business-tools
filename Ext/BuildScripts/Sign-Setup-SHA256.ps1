$env:Path += ";C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools;C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\"
$PublishPath = ".\..\PartialTrustInstaller\publish"
$AppFilesPath = "$PublishPath\Application Files"
$ProjectName = "WMBusinessToolsSetup"
$TargetPath = Convert-Path "$AppFilesPath\${ProjectName}_*"
$CertThumbprint = "fcb671b0b83566d01aee0e142e9ba5a999208ee4"
$TimeStampingServer = "http://timestamp.comodoca.com/rfc3161"
$VSKey = "VS_KEY_B16C51373071738B"

Get-ChildItem "$TargetPath\*.deploy" -Recurse | Rename-Item -NewName { $_.Name -replace '\.deploy','' } 

# Строгое имя
Start-Process "sn.exe" -ArgumentList "-Rc `"$TargetPath\$ProjectName.exe`" `"$VSKey`"" -Wait -NoNewWindow
Start-Process "sn.exe" -ArgumentList "-Rc `"$TargetPath\ru\$ProjectName.resources.dll`" `"$VSKey`"" -Wait -NoNewWindow

# Подпись сертификатом
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /t `"$TimeStampingServer`" `"$PublishPath\Setup.exe`"" -Wait -NoNewWindow
Start-Sleep -m 500
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /as /fd sha256 /tr `"$TimeStampingServer`" /td sha256 `"$PublishPath\Setup.exe`"" -Wait -NoNewWindow
Start-Sleep -m 500
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /t `"$TimeStampingServer`" `"$TargetPath\$ProjectName.exe`"" -Wait -NoNewWindow
Start-Sleep -m 500
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /as /fd sha256 /tr `"$TimeStampingServer`" /td sha256 `"$TargetPath\$ProjectName.exe`"" -Wait -NoNewWindow
Start-Sleep -m 500
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /t `"$TimeStampingServer`" `"$TargetPath\ru\$ProjectName.resources.dll`"" -Wait -NoNewWindow
Start-Sleep -m 500
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /as /fd sha256 /tr `"$TimeStampingServer`" /td sha256 `"$TargetPath\ru\$ProjectName.resources.dll`"" -Wait -NoNewWindow
Start-Sleep -m 500

# Добавляем необходимые разрешения в манифест
[xml]$xmlDocument = Get-Content -Path "$TargetPath\$ProjectName.exe.manifest"
$ns = New-Object System.Xml.XmlNamespaceManager($xmlDocument.NameTable)
$ns.AddNamespace("v2","urn:schemas-microsoft-com:asm.v2")
$ns.AddNamespace("asmv1","urn:schemas-microsoft-com:asm.v1")
$permissionSetElement = $xmlDocument.SelectSingleNode("asmv1:assembly/v2:trustInfo/v2:security/v2:applicationRequestMinimum/v2:PermissionSet", $ns)
$parent = $permissionSetElement.ParentNode

[void] $parent.RemoveChild($permissionSetElement)

$newPermissionSetElement = [Xml] @'
<PermissionSet class="System.Security.PermissionSet" version="1" ID="Custom" SameSite="site" xmlns="urn:schemas-microsoft-com:asm.v2">
   <IPermission version="1" class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Unrestricted="true"/>
   <IPermission version="1" class="System.Security.Permissions.FileIOPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Read="*AllLocalFiles*" PathDiscovery="*AllLocalFiles*" Write="*AllLocalFiles*" />
   <IPermission version="1" class="System.Security.Permissions.SecurityPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Flags="UnmanagedCode, Execution, ControlThread" />
   <IPermission version="1" class="System.Security.Permissions.UIPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Window="AllWindows" />
   <IPermission version="1" class="System.Security.Permissions.MediaPermission, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Audio="SafeAudio" Image="SafeImage" Video="SafeVideo" />
   <IPermission version="1" class="System.Net.DnsPermission, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Unrestricted="true" />
   <IPermission version="1" class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Unrestricted="true" />
   <IPermission version="1" class="System.Net.WebPermission, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
      <ConnectAccess>
         <URI uri="http://www\.webmoney-business-tools\.com/.*" />
      </ConnectAccess>
   </IPermission>
</PermissionSet>
'@

[void] $parent.AppendChild($xmlDocument.ImportNode($newPermissionSetElement.PermissionSet, $true))

$xmlDocument.Save("$TargetPath\$ProjectName.exe.manifest")

# Подпись манифеста
Start-Process ".\mage2008sha256\mage.exe" -ArgumentList "-update `"$TargetPath\$ProjectName.exe.manifest`" -if `"App.ico`" -ch $CertThumbprint -ti `"$TimeStampingServer`"" -Wait -NoNewWindow
Start-Process "mage.exe" -ArgumentList "-update `"$TargetPath\$ProjectName.application`" -Algorithm sha256RSA -IncludeProviderURL true -ProviderURL `"https://www.webmoney-business-tools.com/dist/clickonce/$ProjectName.application`" -ch $CertThumbprint -appManifest `"$TargetPath\$ProjectName.exe.manifest`" -ti `"$TimeStampingServer`"" -Wait -NoNewWindow
Start-Process "mage.exe" -ArgumentList "-update `"$PublishPath\$ProjectName.application`" -Algorithm sha256RSA -IncludeProviderURL true -ProviderURL `"https://www.webmoney-business-tools.com/dist/clickonce/$ProjectName.application`" -ch $CertThumbprint -appManifest `"$TargetPath\$ProjectName.exe.manifest`" -ti `"$TimeStampingServer`"" -Wait -NoNewWindow

Get-ChildItem -Path "$TargetPath\*"  -Recurse | Where-Object {!$_.PSIsContainer -and $_.Name -notlike "*.manifest" -and $_.Name -notlike "*.application"} | Rename-Item -NewName {$_.Name + ".deploy"}
pause
# SIG # Begin signature block
# MIIN/QYJKoZIhvcNAQcCoIIN7jCCDeoCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUCdghiisbH0FVV21Dusc03SGC
# 2oKgggs0MIIFTDCCBDSgAwIBAgIQK91Ppx49jqSsOyLu+VfgjDANBgkqhkiG9w0B
# AQsFADB9MQswCQYDVQQGEwJHQjEbMBkGA1UECBMSR3JlYXRlciBNYW5jaGVzdGVy
# MRAwDgYDVQQHEwdTYWxmb3JkMRowGAYDVQQKExFDT01PRE8gQ0EgTGltaXRlZDEj
# MCEGA1UEAxMaQ09NT0RPIFJTQSBDb2RlIFNpZ25pbmcgQ0EwHhcNMTgwNzEzMDAw
# MDAwWhcNMTkwNzEzMjM1OTU5WjCBlDELMAkGA1UEBhMCVUExDjAMBgNVBBEMBTc1
# MDUwMRAwDgYDVQQIDAdLaGVyc29uMRYwFAYDVQQHDA1PbGVrc2FuZHJpdmthMRkw
# FwYDVQQJDBA4LCBLbmlhemV2YSBTdHIuMRcwFQYDVQQKDA5NYXJpYSBTZWtlcmlu
# YTEXMBUGA1UEAwwOTWFyaWEgU2VrZXJpbmEwggEiMA0GCSqGSIb3DQEBAQUAA4IB
# DwAwggEKAoIBAQC3i1uoxCWVr7UF0xZaXotZTytT0MG/jBRXA+GjMS5hDPhlxemY
# NuDwoi5mpMtcSMFRQz8P+lASStM3WdI+UjpcjSQB34BPujHuHAyGzrnTUhrC2Z3/
# vwIcVGn5RmpmJAv+LtxDsyzwVhLEibdWaweF0DYXOR+Ntbgmh3nnOdW/+Fb4LV7Y
# HPQ/NcQ2ATHRa8iYf3Kj4aPrGrI+eT1mTUizEfpRq5oVR3jkMiXPre5cnpDKgkmQ
# T7fIT9fs2Iy/MBKE1EZfoqqb32kqsJB2xdNyAMudMG4gPbhfeXr3LzY2zJ6JaBXE
# ++eWWVWDpAAFU94Bpfqsnc+lcxHYGRnZKkw5AgMBAAGjggGuMIIBqjAfBgNVHSME
# GDAWgBQpkWD/ik366/mmarjP+eZLvUnOEjAdBgNVHQ4EFgQUj7CK8K2Gn+E72fP2
# p8WUWHzrFDowDgYDVR0PAQH/BAQDAgeAMAwGA1UdEwEB/wQCMAAwEwYDVR0lBAww
# CgYIKwYBBQUHAwMwEQYJYIZIAYb4QgEBBAQDAgQQMEYGA1UdIAQ/MD0wOwYMKwYB
# BAGyMQECAQMCMCswKQYIKwYBBQUHAgEWHWh0dHBzOi8vc2VjdXJlLmNvbW9kby5u
# ZXQvQ1BTMEMGA1UdHwQ8MDowOKA2oDSGMmh0dHA6Ly9jcmwuY29tb2RvY2EuY29t
# L0NPTU9ET1JTQUNvZGVTaWduaW5nQ0EuY3JsMHQGCCsGAQUFBwEBBGgwZjA+Bggr
# BgEFBQcwAoYyaHR0cDovL2NydC5jb21vZG9jYS5jb20vQ09NT0RPUlNBQ29kZVNp
# Z25pbmdDQS5jcnQwJAYIKwYBBQUHMAGGGGh0dHA6Ly9vY3NwLmNvbW9kb2NhLmNv
# bTAfBgNVHREEGDAWgRRzdXBwb3J0QHdtc2lnbmVyLmNvbTANBgkqhkiG9w0BAQsF
# AAOCAQEAbeikNuXfNp1KvuwiYeobcJfDom0T9EnJVtlQaE7wSjia8c9V5My+Mla0
# w/EVw4LVyDPldoupOE9QGZ6Ph2luCsiKMJC9j5bnp7J3di3/hmIBk0xQNuuTQZ9m
# BvXWbsZxC6uKhkx0Fo4afaba81f+13IIYHQ4W1V6iaLrDIUVvl7T0plgdzET1R4h
# VOpj4DPCSV/IV4i5vM4LKoOWpRzTDTY4JQjwgYJpjTHVYZIETw6PNw7dHptvyc/i
# Z7o3sSQvKn6bXA+6Z5LYpwJ0+HdbJqWMULHkZxkDS+/4PudrzrQcCbolWfl1i+75
# H0gBtDgmPmXSUtIMfOriz112tpWjUzCCBeAwggPIoAMCAQICEC58h8wOk0pS/pT9
# HLfNNK8wDQYJKoZIhvcNAQEMBQAwgYUxCzAJBgNVBAYTAkdCMRswGQYDVQQIExJH
# cmVhdGVyIE1hbmNoZXN0ZXIxEDAOBgNVBAcTB1NhbGZvcmQxGjAYBgNVBAoTEUNP
# TU9ETyBDQSBMaW1pdGVkMSswKQYDVQQDEyJDT01PRE8gUlNBIENlcnRpZmljYXRp
# b24gQXV0aG9yaXR5MB4XDTEzMDUwOTAwMDAwMFoXDTI4MDUwODIzNTk1OVowfTEL
# MAkGA1UEBhMCR0IxGzAZBgNVBAgTEkdyZWF0ZXIgTWFuY2hlc3RlcjEQMA4GA1UE
# BxMHU2FsZm9yZDEaMBgGA1UEChMRQ09NT0RPIENBIExpbWl0ZWQxIzAhBgNVBAMT
# GkNPTU9ETyBSU0EgQ29kZSBTaWduaW5nIENBMIIBIjANBgkqhkiG9w0BAQEFAAOC
# AQ8AMIIBCgKCAQEAppiQY3eRNH+K0d3pZzER68we/TEds7liVz+TvFvjnx4kMhEn
# a7xRkafPnp4ls1+BqBgPHR4gMA77YXuGCbPj/aJonRwsnb9y4+R1oOU1I47Jiu4a
# DGTH2EKhe7VSA0s6sI4jS0tj4CKUN3vVeZAKFBhRLOb+wRLwHD9hYQqMotz2wzCq
# zSgYdUjBeVoIzbuMVYz31HaQOjNGUHOYXPSFSmsPgN1e1r39qS/AJfX5eNeNXxDC
# RFU8kDwxRstwrgepCuOvwQFvkBoj4l8428YIXUezg0HwLgA3FLkSqnmSUs2HD3vY
# YimkfjC9G7WMcrRI8uPoIfleTGJ5iwIGn3/VCwIDAQABo4IBUTCCAU0wHwYDVR0j
# BBgwFoAUu69+Aj36pvE8hI6t7jiY7NkyMtQwHQYDVR0OBBYEFCmRYP+KTfrr+aZq
# uM/55ku9Sc4SMA4GA1UdDwEB/wQEAwIBhjASBgNVHRMBAf8ECDAGAQH/AgEAMBMG
# A1UdJQQMMAoGCCsGAQUFBwMDMBEGA1UdIAQKMAgwBgYEVR0gADBMBgNVHR8ERTBD
# MEGgP6A9hjtodHRwOi8vY3JsLmNvbW9kb2NhLmNvbS9DT01PRE9SU0FDZXJ0aWZp
# Y2F0aW9uQXV0aG9yaXR5LmNybDBxBggrBgEFBQcBAQRlMGMwOwYIKwYBBQUHMAKG
# L2h0dHA6Ly9jcnQuY29tb2RvY2EuY29tL0NPTU9ET1JTQUFkZFRydXN0Q0EuY3J0
# MCQGCCsGAQUFBzABhhhodHRwOi8vb2NzcC5jb21vZG9jYS5jb20wDQYJKoZIhvcN
# AQEMBQADggIBAAI/AjnD7vjKO4neDG1NsfFOkk+vwjgsBMzFYxGrCWOvq6LXAj/M
# bxnDPdYaCJT/JdipiKcrEBrgm7EHIhpRHDrU4ekJv+YkdK8eexYxbiPvVFEtUgLi
# dQgFTPG3UeFRAMaH9mzuEER2V2rx31hrIapJ1Hw3Tr3/tnVUQBg2V2cRzU8C5P7z
# 2vx1F9vst/dlCSNJH0NXg+p+IHdhyE3yu2VNqPeFRQevemknZZApQIvfezpROYyo
# H3B5rW1CIKLPDGwDjEzNcweU51qOOgS6oqF8H8tjOhWn1BUbp1JHMqn0v2RH0aof
# U04yMHPCb7d4gp1c/0a7ayIdiAv4G6o0pvyM9d1/ZYyMMVcx0DbsR6HPy4uo7xwY
# WMUGd8pLm1GvTAhKeo/io1Lijo7MJuSy2OU4wqjtxoGcNWupWGFKCpe0S0K2VZ2+
# medwbVn4bSoMfxlgXwyaiGwwrFIJkBYb/yud29AgyonqKH4yjhnfe0gzHtdl+K7J
# +IMUk3Z9ZNCOzr41ff9yMU2fnr0ebC+ojwwGUPuMJ7N2yfTm18M04oyHIYZh/r9V
# dOEhdwMKaGy75Mmp5s9ZJet87EUOeWZo6CLNuO+YhU2WETwJitB/vCgoE/tqylSN
# klzNwmWYBp7OSFvUtTeTRkF8B93P+kPvumdh/31J4LswfVyA4+YWOUunMYICMzCC
# Ai8CAQEwgZEwfTELMAkGA1UEBhMCR0IxGzAZBgNVBAgTEkdyZWF0ZXIgTWFuY2hl
# c3RlcjEQMA4GA1UEBxMHU2FsZm9yZDEaMBgGA1UEChMRQ09NT0RPIENBIExpbWl0
# ZWQxIzAhBgNVBAMTGkNPTU9ETyBSU0EgQ29kZSBTaWduaW5nIENBAhAr3U+nHj2O
# pKw7Iu75V+CMMAkGBSsOAwIaBQCgeDAYBgorBgEEAYI3AgEMMQowCKACgAChAoAA
# MBkGCSqGSIb3DQEJAzEMBgorBgEEAYI3AgEEMBwGCisGAQQBgjcCAQsxDjAMBgor
# BgEEAYI3AgEVMCMGCSqGSIb3DQEJBDEWBBTVqLHlqwJv3g1UTsZJkDiZyXz8+DAN
# BgkqhkiG9w0BAQEFAASCAQCm7+hnX6Ezr/lwlaTya2G16vspqAT8eHtUSaOChFMr
# 8MnByBiIjqnrzW0FddwNr1S6Z4rXzaHA4HQi1UDiJ4sZapv5nvzdpyPOgj3zaYYl
# z8SZsFshQmO4RmKMWleG8kKgnQkFsB9hk2MzOe0pS46zTl7pYZFTctcrWTceN8Dk
# uYnVW9rM3UL7tl+meEmzaRSbx2pZctXXrrWIR51VNncZEU+EzgEKrYXhmPVXXxqc
# ybbVaKuCpkqUKtte5tHb5cvormtd2r8Lxx2idPGN9KuKt/ZbeQes347BqNi+QIda
# ECgqpe7iukM/zX9XDeQuEudY+JkzGteCWGoEENKOoQrZ
# SIG # End signature block
