cd ..\ace\addons
del *.pbo

py %0\..\..\ace\tools\build.py

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
del advanced_fatigue.pbo

cd ..\..\acex\addons
del *.pbo

py %0\..\..\acex\tools\build.py

del acex_headless.pbo
del acex_viewrestriction.pbo

pause