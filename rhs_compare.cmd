@echo off

SETLOCAL ENABLEEXTENSIONS

if exist p:\ (goto pfound)
echo P: drive must be set
goto end

:pfound
p:\rhs_compare\ArmaConfigParser.exe -path=p:\rhs_compare -r
for /r p:\rhs_compare %%A in (*) do (
	if "%%~nxA"=="config.cpp" (del /q %%~dpnxA) 
)

:end
pause
@exit /B 0