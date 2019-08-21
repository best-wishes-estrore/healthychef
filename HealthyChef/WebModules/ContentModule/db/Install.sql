if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ContentModule_Versions_ContentModule_Status]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Content] DROP CONSTRAINT FK_ContentModule_Versions_ContentModule_Status
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Content]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content_Status]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Content_Status]
GO

CREATE TABLE [dbo].[Content] (
	[ContentVersionId] [int] IDENTITY (1, 1) NOT NULL ,
	[ModuleId] [int] NOT NULL ,
	[Culture] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[StatusId] [int] NOT NULL ,
	[Modified] [datetime] NOT NULL ,
	[Text] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Content_Status] (
	[StatusId] [int] IDENTITY (1, 1) NOT NULL ,
	[StatusName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Content] WITH NOCHECK ADD 
	CONSTRAINT [PK_ContentModule_Versions] PRIMARY KEY  CLUSTERED 
	(
		[ContentVersionId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Content_Status] WITH NOCHECK ADD 
	CONSTRAINT [PK_ContentModule_Status] PRIMARY KEY  CLUSTERED 
	(
		[StatusId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Content] ADD 
	CONSTRAINT [FK_ContentModule_Versions_ContentModule_Status] FOREIGN KEY 
	(
		[StatusId]
	) REFERENCES [dbo].[Content_Status] (
		[StatusId]
	)
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content_GetActiveContent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Content_GetActiveContent]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content_GetById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Content_GetById]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content_GetByModuleId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Content_GetByModuleId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content_GetByStatusId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Content_GetByStatusId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content_SetStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Content_SetStatus]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.Content_GetActiveContent
(
	@ModuleId int,
	@Culture varchar(10)
)
AS
	SELECT	*
	FROM	Content
	WHERE	(ModuleId = @ModuleId) 
	AND	(StatusId = 2)
	AND	Culture = (
		SELECT	TOP 1 Culture
		FROM	Content
		WHERE 	ModuleId = @ModuleId
		AND	StatusId = 2
		ORDER BY (
			CASE  
				WHEN Culture=@culture THEN 1 
				WHEN LEFT(Culture, 2)=LEFT(@Culture, 2) THEN 2 
				WHEN Culture='' THEN 3 
				ELSE 4 
			END	
		)
	)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.Content_GetById
(
	@ContentVersionId int
)
AS
	SET NOCOUNT ON;
SELECT     ContentVersionId, ModuleId, Culture, StatusId, Modified, Text
FROM         Content
WHERE     (ContentVersionId = @ContentVersionId)
ORDER BY Modified DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.Content_GetByModuleId
(
	@ModuleId int
)
AS
	SET NOCOUNT ON;
SELECT     ContentVersionId, ModuleId, Culture, StatusId, Modified, Text
FROM         Content
WHERE     (ModuleId = @ModuleId)
ORDER BY Modified DESC

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.Content_GetByStatusId
(
	@StatusId int,
	@ModuleId int,
	@Culture varchar(10)
)
AS
	SET NOCOUNT ON;
SELECT     ContentVersionId, ModuleId, Culture, StatusId, Modified, Text
FROM         Content
WHERE     (StatusId = @StatusId) and (ModuleId = @ModuleId) AND (Culture=@Culture)
ORDER BY Modified DESC
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.Content_SetStatus
(
	@StatusId int,
	@ContentVersionId int
)
AS
	SET NOCOUNT OFF;
UPDATE    Content
SET              StatusId = @StatusId
WHERE     (ContentVersionId = @ContentVersionId);
	 
SELECT ContentVersionId, ModuleId, Culture, StatusId, Modified, Text FROM Content WHERE (ContentVersionId = @ContentVersionId)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



SET IDENTITY_INSERT dbo.Content_Status ON
INSERT INTO dbo.Content_Status (StatusId, StatusName) VALUES (1, 'Pending Content'); 
INSERT INTO dbo.Content_Status (StatusId, StatusName) VALUES (2, 'Active Content');
INSERT INTO dbo.Content_Status (StatusId, StatusName) VALUES (3, 'Past Content');

SET IDENTITY_INSERT dbo.Content_Status OFF
GO
--INSERT WebModules_SchemaVersions (Major, Minor, Revision, Build) VALUES (1,1,60429,1)
INSERT WebModules_SchemaVersions ([AppName],Major, Minor, Revision, Build) VALUES ('ContentModule', 1,0,60907,1)
GO