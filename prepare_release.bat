
:: Prep folders

cd ..\release

mkdir @a3cs
mkdir @a3cs_server
cd @a3cs_server
mkdir addons

:: Copy ACE

cd ..\..\
xcopy /s "ace\release\@ace" release\@a3cs

cd release\@a3cs
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

del *.bisign

:: Copy A3CS

cd ..\..\..\
xcopy /s "a3cs\release\@a3cs" release\@a3cs

cd release\@a3cs
RD /q /s keys

cd addons

xcopy /s a3cs_server.pbo ..\..\@a3cs_server\addons
del a3cs_server.pbo
del *.bisign

pause