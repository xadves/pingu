# Usage

My preferred method of usage is to drop the .exe into my user folder ( run %userprofile% ) and then with command prompt or run:
```
> pingu google.com facebook.com 127.0.0.1
```

Every argument passed to the application will add it to it's list of hosts to ping.

# Build

Currently Compiling with this compile.bat script in the same directory as the source code. This requires you to have to the same version of .NET as I do, definitely check that before running the build script.

```
@echo off
echo Compiling with v4.0.30319...
C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe /t:exe /out:pingu.exe pingu.cs

pause
```