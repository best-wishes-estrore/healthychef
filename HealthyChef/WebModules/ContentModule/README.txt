MODULE INSTALLATION

Fix the .xsd:
    Right-click on DataAccessLayer.xsd.
    Select 'Properties'.
    Enter "BayshoreSolutions.WebModules.ContentModule" into the "Custom tool
    Namespace" property.

Install the database objects:
    Run db/Install.sql on the application database.


DEPENDENCIES

    WebModules:
        TextEditor
