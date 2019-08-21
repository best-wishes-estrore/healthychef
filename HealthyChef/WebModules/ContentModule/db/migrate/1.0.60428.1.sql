if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Content]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Content]
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

INSERT WebModules_SchemaVersions (Major, Minor, Revision, Build) VALUES (1,1,60429,1)
GO