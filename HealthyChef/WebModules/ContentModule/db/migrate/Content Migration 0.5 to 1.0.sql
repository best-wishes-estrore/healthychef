INSERT INTO Content
	(ModuleId, Culture, StatusId, Modified, Text)
SELECT 	ModuleId, 
	CASE 
		WHEN LEN(Culture)=0 THEN 'en-US'
		ELSE Culture
	END AS Culture,
	2,
	GETDATE(),
	COALESCE(ContentText,'')
FROM 	WebModulesSystem_Content A 
INNER JOIN WebModules_ModuleVersions B ON A.ContentId = B.VersionId


SELECT * FROM Content 