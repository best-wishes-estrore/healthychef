@echo off
: Subsonic v2.1.0
: View all subsonic config options here:  http://www.subsonicproject.com/view/config-options.aspx

SET switches=/server devsql2 /db $safeprojectname$ /generatedNamespace "BayshoreSolutions.WebModules.MasterDetail" /fixPluralClassNames true /useSPs true /provider "WebModules" /generateRelatedTablesAsProperties true /templateDirectory "..\Templates"
echo.
echo Starting Subsonic...
@echo on
cd Generated
del *.cs
..\..\..\..\lib\sonic.exe generate %switches% /includeTableList "^MasterDetail_" /includeProcedureList "^MasterDetail_" /includeViewList "^MasterDetail_"
@echo off
cd ..
pause