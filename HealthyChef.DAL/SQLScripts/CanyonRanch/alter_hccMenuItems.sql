/*
   Wednesday, October 16, 20133:28:15 PM
   User: 
   Server: devsql6
   Database: healthychef
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.hccMenuItems ADD
	CanyonRanchRecipe bit NULL,
	CanyonRanchApproved bit NULL,
	VegetarianOptionAvailable bit NULL,
	VeganOptionAvailable bit NULL,
	GlutenFreeOptionAvailable bit NULL
GO
ALTER TABLE dbo.hccMenuItems SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.hccMenuItems', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.hccMenuItems', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.hccMenuItems', 'Object', 'CONTROL') as Contr_Per 