
:: Prep folders

cd ..\release

RD /q /s @A3CS
RD /q /s @A3CS_Server

mkdir @A3CS
mkdir @A3CS_Server
cd @A3CS_Server
mkdir addons

:: Copy ACE

cd ..\..\
xcopy /s "ace\release\@ace" "release\@A3CS"

cd release\@A3CS
RD /q /s keys
RD /q /s optionals
del AUTHORS.txt
del LICENSE
del logo_ace3_ca.paa
del meta.cpp
del mod.cpp
del README.md
del README_DE.md
del README_PL.md

:: Clear ACE pbos

cd addons
del ace_advanced_ballistics.pbo
del ace_gforces.pbo
del ace_hearing.pbo
del ace_nametags.pbo
del ace_weather.pbo
del ace_winddeflection.pbo
del ace_atragmx.pbo
del ace_kestrel4500.pbo
del ace_dagr.pbo
del ace_rangecard.pbo
del ace_scopes.pbo
del ace_frag.pbo
del ace_sitting.pbo

del *.bisign

:: Copy ACEX

cd ..\..\..\
xcopy /s "acex\release\@acex\addons" "release\@A3CS\addons"

cd release\@A3CS\addons

del acex_headless.pbo
del acex_viewrestriction.pbo

del *.bisign

:: Copy A3CS

cd ..\..\..\
xcopy /s "a3cs\release\@a3cs" "release\@A3CS"

cd release\@A3CS
RD /q /s keys

cd ..\..\a3cs

xcopy /s version.a3c "..\release\@A3CS"

:: Prepare a3cs_server

cd ..\release\@A3CS\addons

xcopy /s a3cs_server.pbo "..\..\@A3CS_Server\addons"
del a3cs_server.pbo
del *.bisign

pause