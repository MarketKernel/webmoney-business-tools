SET CONFIGURATION=Production
SET DEV_PKT=236f20e4200c621f
SET PROD_PKT=d1bcab3940c39a1a

CALL .\Build.cmd

powershell -Command "(Get-Content .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Extensions.json) | ForEach-Object { $_ -replace '%DEV_PKT%', '%PROD_PKT%' } | Set-Content .\..\WMBusinessTools\bin\%CONFIGURATION%\SupportAssistant\Extensions.json"
powershell -Command "(Get-Content .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\Extensions.json) | ForEach-Object { $_ -replace '%DEV_PKT%', '%PROD_PKT%' } | Set-Content .\..\WMBusinessTools\bin\%CONFIGURATION%\BasicExtensions\Extensions.json"
powershell -Command "(Get-Content .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\Extensions.json) | ForEach-Object { $_ -replace '%DEV_PKT%', '%PROD_PKT%' } | Set-Content .\..\WMBusinessTools\bin\%CONFIGURATION%\WebMoney.Services\Extensions.json"