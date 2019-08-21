FormBuilder module
===============================================================================

MODULE DEPENDENCIES
===============================================================================
BayshoreSolutions.Common 2008.9.26.10+
BayshoreSolutions.WebModules 2008.8.7.4+ OR 2007.8.1.7+
SubSonic 2.1.0.0


INSTALLATION AND CONFIGURATION
===============================================================================
Application database:
    Run db/Install.sql on the application database.

Web.config:
    <appSettings>
        <add key="FormBuilder_UploadDirectory" value="~/userfiles/formbuilder_submissions/" />
    </appSettings>



MODULE UPGRADE
===============================================================================
If a project has a previous version of this module, each script in db/migrate/ 
must be executed, in order, on the application database, starting with the 
version immediately following the current installed module version.

	04_29_2010.sql
    
    