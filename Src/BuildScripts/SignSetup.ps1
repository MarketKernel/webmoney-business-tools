$env:Path += ";C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools;C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\"
$AppPath = ".\..\WMBusinessTools.Setup\Production\WMBusinessTools.Setup.msi"
$CertThumbprint = "fcb671b0b83566d01aee0e142e9ba5a999208ee4"
$TimeStampingServer = "http://timestamp.comodoca.com/rfc3161"

Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /t `"$TimeStampingServer`" /d `"WMBusinessTools`" /du `"https://www.webmoney-business-tools.com/`" `"$AppPath`"" -Wait -NoNewWindow
Start-Sleep -m 500