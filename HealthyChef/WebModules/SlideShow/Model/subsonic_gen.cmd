@echo off
: Subsonic v2.1.0
: View all subsonic config options here:  http://www.subsonicproject.com/view/config-options.aspx

SET switches=/server devsql5 /db HealthyChef /generatedNamespace "BayshoreSolutions.WebModules.SlideShow" /fixPluralClassNames true /useSPs true /provider "WebModules" /generateRelatedTablesAsProperties true /templateDirectory "..\Templates"
echo.
echo Starting Subsonic...
@echo on
cd Generated
del *.cs
..\..\..\..\lib\sonic.exe generate %switches% /includeTableList "^SlideShow_" /includeProcedureList "^SlideShow_" /includeViewList "^SlideShow_"
@echo off
cd ..
pause