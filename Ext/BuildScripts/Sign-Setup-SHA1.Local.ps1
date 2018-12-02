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
   <IPermission version="1" class="System.Security.Permissions.RegistryPermission, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Read="HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" Write="HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall" Create="HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"/>
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

Start-Process ".\mage2008sha1\mage.exe" -ArgumentList "-update `"$TargetPath\$ProjectName.exe.manifest`" -if `"App.ico`" -ch $CertThumbprint -ti `"$TimeStampingServer`"" -Wait -NoNewWindow
Start-Process ".\mage2008\mage.exe" -ArgumentList "-Sign `"$TargetPath\$ProjectName.exe.manifest`" -ch $CertThumbprint -ti `"$TimeStampingServer`"" -Wait -NoNewWindow

Start-Process ".\mage2008\mage.exe" -ArgumentList "-update `"$TargetPath\$ProjectName.application`" -ch $CertThumbprint -appManifest `"$TargetPath\$ProjectName.exe.manifest`" -ti `"$TimeStampingServer`"" -Wait -NoNewWindow
Start-Process ".\mage2008\mage.exe" -ArgumentList "-Sign `"$TargetPath\$ProjectName.application`" -ch $CertThumbprint -ti `"$TimeStampingServer`"" -Wait -NoNewWindow

Start-Process ".\mage2008\mage.exe" -ArgumentList "-update `"$PublishPath\$ProjectName.application`" -ch $CertThumbprint -appManifest `"$TargetPath\$ProjectName.exe.manifest`" -ti `"$TimeStampingServer`"" -Wait -NoNewWindow
Start-Process ".\mage2008\mage.exe" -ArgumentList "-Sign `"$PublishPath\$ProjectName.application`"  -ch $CertThumbprint -ti `"$TimeStampingServer`"" -Wait -NoNewWindow

Get-ChildItem -Path "$TargetPath\*"  -Recurse | Where-Object {!$_.PSIsContainer -and $_.Name -notlike "*.manifest" -and $_.Name -notlike "*.application"} | Rename-Item -NewName {$_.Name + ".deploy"}
pause