$env:Path += ";C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.7.2 Tools;C:\Program Files (x86)\Windows Kits\10\bin\10.0.17134.0\x64\"
$AppPath = ".\..\..\Tools\ManifestSigner\ManifestSigner.exe"
$CertThumbprint = "fcb671b0b83566d01aee0e142e9ba5a999208ee4"
$TimeStampingServer = "http://timestamp.comodoca.com/rfc3161"
$VSKey = "VS_KEY_B16C51373071738B"

Start-Process "sn.exe" -ArgumentList "-Rc `"$AppPath`" `"$VSKey`"" -Wait -NoNewWindow

Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /t `"$TimeStampingServer`" `"$AppPath`"" -Wait -NoNewWindow
Start-Sleep -m 500
Start-Process "signtool.exe" -ArgumentList "sign /sha1 `"$CertThumbprint`" /as /fd sha256 /tr `"$TimeStampingServer`" /td sha256 `"$AppPath`"" -Wait -NoNewWindow
Start-Sleep -m 500