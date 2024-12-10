@echo off
setlocal
set MC="D:\Work\OPC\UA-ModelCompiler\build\bin\Release\net8.0\Opc.Ua.ModelCompiler.exe"

echo Building ModelDesign
%MC% compile -version v105 -d2 ".\ModelDesign.xml" -cg ".\ModelDesign.csv" -o2 .\
echo Success!



