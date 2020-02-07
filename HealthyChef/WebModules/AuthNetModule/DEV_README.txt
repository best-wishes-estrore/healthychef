[This file should be deleted once the module is released.]

MODULE DEVELOPMENT GUIDELINES
===============================================================================
The most important feature of a module is self-containment: the 
productivity advantage of a modular architecture depends on the "pluggability"
of a module: one should be able to install a module into a project by dropping 
the module folder into the project's WebModules/ directory and following the 
README.txt.

The second most important feature of a module is consistency: modules should
adhere to a predictable pattern. 

All standard (i.e., not project-specific) modules should live in the 
'BayshoreSolutions.WebModules.Cms' namespace. Modules should *not* live in the
'BayshoreSolutions.WebModules' namespace, which is for the WebModules core.

To access the WebModules connection string from code, use:
BayshoreSolutions.WebModules.Global.ConnectionString

Following is a list of files and folders common to every module.

Files:

    README.txt
        Concise description of module functionality, assumptions, and gotchas.
        Explicitly specifies the following:
            - dependencies, including versions
            - installation and configuration
            - procedure to upgrade from an old version of the module
        Please use the stub README.txt provided with the module template for
        inspiration.
        
    CHANGES.txt
        Indicates the current module version, and previous module versions,
        with a history of significant changes per version. Please use the stub 
        provided with the module template for inspiration.
        
    Web.config
        ASP.NET framework settings specific to the module.
    
    WebApp.config
        WebModules framework settings specific to the module.
    
    Display.ascx
        This is the public view of the module.
    
    Edit.aspx
        This form controls the per-instance settings for the module.
    
    Default.aspx
        If HasSettings="true" in the module WebApp.config, this form controls 
        system-wide settings for the module.
        
    db/Install.sql
        Creates all database objects (views, tables, stored procedures, 
        user-defined functions, etc.) and data required by the module. The 
        script must *not* use fully qualified identifiers.

Folders:

    lib/
        Contains all NON-standard dependencies required by the module.        
            Standard dependencies include, but are not limited to: 
                - the .NET framework
                - libraries included with the WebModules CMS project template
                - files expected to be on all developer machines
        
        If the module depends on other WebModules CMS modules, do *not*
        include the required module(s); instead, specify the required module(s)
        and version(s) in the README.txt.
    
    public/
        Public-readable folder containing client files (CSS, javascript, etc.) 
        required by the module view. 

    public/include/
        The CMS automatically adds .css files in this directory to any page 
        that contains the module. This feature should be used with caution
        (e.g., CSS classes should be named carefully to avoid conflicts with 
        the site's main theme).
        [Future support for other file types (such as javascript) is planned.]
        
    db/
        Contains SQL scripts required by the module.
        
    db/migrate/
        Contains database change scripts named with the format YYYYMMDD.XX.sql,
        where the name indicates the version being migrated to.
        
