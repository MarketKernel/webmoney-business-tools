echo off

echo ========== START ==========


rd /s /q .\..\SupportAssistant\bin
rd /s /q .\..\SupportAssistant\obj

rd /s /q .\..\WebMoney.Services\bin
rd /s /q .\..\WebMoney.Services\obj

rd /s /q .\..\WebMoney.Services.Contracts\bin
rd /s /q .\..\WebMoney.Services.Contracts\obj

rd /s /q .\..\WMBusinessTools\bin
rd /s /q .\..\WMBusinessTools\obj

rd /s /q .\..\WMBusinessTools.Extensions\bin
rd /s /q .\..\WMBusinessTools.Extensions\obj

rd /s /q .\..\WMBusinessTools.Extensions.Contracts\bin
rd /s /q .\..\WMBusinessTools.Extensions.Contracts\obj


echo ========== DONE ==========
pause