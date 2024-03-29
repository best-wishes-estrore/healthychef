/*
   Friday, November 01, 20132:25:16 PM
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
ALTER TABLE dbo.hccCarts ADD
	TaxRate decimal(18, 0) NULL
GO
ALTER TABLE dbo.hccCarts SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.hccCarts', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.hccCarts', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.hccCarts', 'Object', 'CONTROL') as Contr_Per 