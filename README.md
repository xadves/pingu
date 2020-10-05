
Currently Compiling with this compile.bat script

`@echo off
echo Compiling with v4.0.30319...
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /t:exe /out:%1.exe .\src\%1.cs

pause`