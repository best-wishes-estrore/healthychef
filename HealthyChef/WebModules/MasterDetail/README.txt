MasterDetail module
===============================================================================

DEPENDENCIES
===============================================================================
BayshoreSolutions.WebModules 2008.8.7.5 
RssToolkit.dll (included in the lib/ folder of this project)
SubSonic 2.1.0.0 (included in the lib/ folder of this project)

DATABASE
========
Run database install script /db/Install_MasterDetail.sql

If upgrading an older version of the MasterDetail module, run the /db/migrate/Upgrade_MasterDetail_06082010.sql


SUBSONIC GENERATION SCRIPT
==============================================================================
If you modify any MasterDetail tables, views, or stored procs, you will need to run the Model/subsonic_gen.cmd script.
Edit subsonic_gen.cmd to set the correct database name on Line 5:

SET switches=/server devsql2 /db DATABASE_NAME_GOES_HERE 


LEGACY WEBMODULES
===============================================================================
Projects using an old version of WebModules CMS need to add the following files:
    /Templates/WebModules/Module.Master
    /WebModules/Admin/ModuleAdminPage.cs


