/*
   Friday, October 18, 20139:23:57 AM
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
ALTER TABLE dbo.hccUserProfiles ADD
	CanyonRanchCustomer bit NULL
GO
ALTER TABLE dbo.hccUserProfiles SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.hccUserProfiles', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.hccUserProfiles', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.hccUserProfiles', 'Object', 'CONTROL') as Contr_Per 