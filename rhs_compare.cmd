@echo off

SETLOCAL ENABLEEXTENSIONS

if exist p:\ (goto pfound)
echo P: drive must be set
goto end

:pfound
p:\RHS_Compare\ArmaConfigParser.exe -path=p:\RHS_Compare -r
for /r p:\RHS_Compare %%A in (*) do (
	if "%%~nxA"=="config.cpp" (del /q %%~dpnxA) 
)

:end
pause
@exit /B 0