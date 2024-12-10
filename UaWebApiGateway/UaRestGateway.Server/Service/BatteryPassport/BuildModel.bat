@ECHO off
SETLOCAL

rem set MODELCOMPILER=docker run -v %CD%:/data --rm ghcr.io/opcfoundation/ua-modelcompiler:latest 
set MODELCOMPILER=D:\Work\OPC\UA-ModelCompiler\build\bin\Release\net8.0\Opc.Ua.ModelCompiler.exe
set MODEL=BatteryPassport
set VERSION=v105
set EXCLUDE=Draft
set INPUT=.\
set CSVINPUT=.
set OUTPUT=.
set USEALLOWSUBTYPES=-useAllowSubtypes

ECHO Building Model %MODEL%
ECHO %MODELCOMPILER% compile -version %VERSION% -exclude %EXCLUDE% -d2 "%INPUT%\%MODEL%.xml,Opc.Ua.BatteryPassport,BatteryPassport" -cg "%CSVINPUT%\%MODEL%.csv" -o2 "%OUTPUT%" %USEALLOWSUBTYPES%
%MODELCOMPILER% compile -version %VERSION% -exclude %EXCLUDE% -d2 "%INPUT%\%MODEL%.xml" -cg "%CSVINPUT%\%MODEL%.csv" -o2 "%OUTPUT%" %USEALLOWSUBTYPES%
IF %ERRORLEVEL% NEQ 0 ( ECHO Failed %MODEL% & EXIT /B 3 )

