Menus module
===============================================================================

*******************************************************************************
*******************************************************************************
Note to module developer:
This sample README demonstrates how a module README should be formatted. 
Please modify this README to reflect the information appropriate to the module; 
delete the sample information that is not relevant to the module; and delete
this note.
*******************************************************************************
*******************************************************************************

This module assumes that it is located in:
~/WebModules/Menus/


DEPENDENCIES
===============================================================================
Foo.dll 3.0
Baz module 1.5


INSTALLATION AND CONFIGURATION
===============================================================================
Add a reference to each .dll in the module lib/ directory (there is no need to 
copy the .dll files to the project bin/ directory).

DataAccessLayer.xsd:
    Right-click -> select 'Properties'.
    Custom tool namespace: BayshoreSolutions.WebModules.ContentModule

Web.config:
    <appSettings>
        <!-- Location of Jim's BBQ recipes. -->
        <add key="bbqRecipes" value="~/WebModules/BbqRecipes/" />
    </appSettings>

Application database:
    Run db/Install.sql on the application database.


UPGRADE PROCEDURE
===============================================================================
If a project has a previous version of this module, each script in db/migrate/ 
must be executed, in order, on the application database, starting with the 
version immediately following the current installed module version.

Example:
    If the current installed module version is 2006.3.4.1, and db/migrate/ 
    has the following scripts:
        20060304.0.sql
        20060304.1.sql
        20060723.0.sql
        20070119.0.sql        
    To upgrade to version 20070119.0, you would execute 20060723.0.sql on the
    database, then 20070119.0.sql, because 2006.7.23.0 is the version 
    immediately following the current installed module version.

    