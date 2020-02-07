--********************************************************************************
-- UPGRADE 1.1.60424.1 Matt Murrell - Inserts default records into Content_Status table
--********************************************************************************

SET IDENTITY_INSERT dbo.Content_Status ON
INSERT INTO dbo.Content_Status (StatusId, StatusName) VALUES (1, 'Pending Content'); 
INSERT INTO dbo.Content_Status (StatusId, StatusName) VALUES (2, 'Active Content');
INSERT INTO dbo.Content_Status (StatusId, StatusName) VALUES (3, 'Past Content');

SET IDENTITY_INSERT dbo.Content_Status OFF

GO

INSERT WebModules_SchemaVersions (Major, Minor, Revision, Build) VALUES (1,1,60424,1)
GO