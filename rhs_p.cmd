@echo off

SETLOCAL ENABLEEXTENSIONS

pause

if exist p:\ (goto pfound)
echo P: drive must be set
goto end

:pfound

for /F "Tokens=2* skip=2" %%A In ('REG QUERY "HKLM\SOFTWARE\Wow6432Node\Bohemia Interactive Studio\ArmA 3" /v "MAIN" 2^>nul') do (set _ARMA3PATH=%%B)
if defined _ARMA3PATH goto found_A3
 
for /F "Tokens=2* skip=2" %%A In ('REG QUERY "HKLM\SOFTWARE\Bohemia Interactive Studio\ArmA 3" /v "MAIN" 2^>nul') do (set _ARMA3PATH=%%B)
if defined _ARMA3PATH goto found_A3
 
for /F "Tokens=2* skip=2" %%A In ('REG QUERY "HKLM\SOFTWARE\Wow6432Node\Bohemia Interactive\ArmA 3" /v "MAIN" 2^>nul') do (set _ARMA3PATH=%%B)
if defined _ARMA3PATH goto found_A3
 
for /F "Tokens=2* skip=2" %%A In ('REG QUERY "HKLM\SOFTWARE\Bohemia Interactive\ArmA 3" /v "MAIN" 2^>nul') do (set _ARMA3PATH=%%B)
if defined _ARMA3PATH goto found_A3
 
rem no regkeys are found so use steams generic folder if present

for /F "Tokens=2* skip=2" %%A In ('REG QUERY "HKLM\SOFTWARE\Wow6432Node\Valve\Steam" /v "InstallPath" 2^>nul') do (set _ARMA3PATH=%%B\steamapps\common\Arma 3)
if defined _ARMA3PATH goto found_A3
for /F "Tokens=2* skip=2" %%A In ('REG QUERY "HKLM\SOFTWARE\Valve\Steam" /v "InstallPath" 2^>nul') do (set _ARMA3PATH=%%B\steamapps\common\Arma 3)
if defined _ARMA3PATH goto found_A3

:found_A3

if exist p:\rhsafrf (rmdir /s/q p:\rhsafrf)
if exist p:\rhsusf (rmdir /s/q p:\rhsusf)

if not exist "%_ARMA3PATH%\@RHSAFRF\addons" goto noafrf
extractpbo -p "%_ARMA3PATH%\@RHSAFRF\addons" p:\

if not exist "%_ARMA3PATH%\@RHSUSAF\addons" goto nousaf
extractpbo -p "%_ARMA3PATH%\@RHSUSAF\addons" p:\

:noafrf
echo @RHSAFRF not found

:nousaf
echo @RHSUSAF not found

:end
@exit /B 0