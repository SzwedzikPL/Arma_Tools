@echo off

SETLOCAL ENABLEEXTENSIONS

if exist p:\ (goto pfound)
echo P: drive must be set
goto end

:end
pause
@exit /B 0

:cpfile
echo %2
if exist p:\RHS_Compare\%1 (rmdir /s/q p:\RHS_Compare\%1 )
mkdir p:\RHS_Compare\%1
copy %2 p:\RHS_Compare\%1\config.cpp 1>NUL
goto eof

:finddir
chdir /d "%1"
for %%F in (.) do (call :cpfile %%~nxF %2)
goto eof

:pfound
for /r p:\rhsafrf\addons %%A in (*) do (
	if "%%~nxA"=="config.cpp" (call :finddir %%~dpA %%~dpnxA)	
)
for /r p:\rhsusf\addons %%A in (*) do (
	if "%%~nxA"=="config.cpp" (call :finddir %%~dpA %%~dpnxA)	
)
goto end

:eof