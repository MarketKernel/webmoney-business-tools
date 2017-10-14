nuget restore "./../WMBusinessTools.sln"
SET PATH="C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\"
MSBuild.exe "./../WMBusinessTools.sln" /p:Configuration=Release
pause