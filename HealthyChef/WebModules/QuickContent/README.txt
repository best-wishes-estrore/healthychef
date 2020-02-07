Quick Content Module
====================

INSTRUCTIONS FOR USE
====================
Insert this server control into the page:

	<bss:QuickContent runat="server" ContentName="my-content-area-name" />

Set the ContentName attribute to a unique name that will be used to retrieve the content from the database.

Quick Content areas are only editable when a user is logged in with Administrator or Content Editor roles.
New content areas will be displayed with a prompt to "Double-click to add text..."

DATABASE
========
Run database install script /db/Install_QuickContent.sql

WEB.CONFIG UPDATES
==================
If upgrading an existing site, add the following tagPrefix to Web.config:
		<pages>
			<controls>
				<add tagPrefix="bss" tagName="QuickContent" src="~/WebModules/QuickContent/ContentArea.ascx" />
			</controls>
		</pages>

		
SUBSONIC GENERATION SCRIPT
==============================================================================
If you modify any QuickContent tables, views, or stored procs, you will need to run the Model/subsonic_gen.cmd script.
Edit subsonic_gen.cmd to set the correct database name on Line 5:

SET switches=/server devsql2 /db DATABASE_NAME_GOES_HERE 
