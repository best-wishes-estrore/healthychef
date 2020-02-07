
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
	AND	(StatusId = 1)
	AND	Culture = (
		SELECT	TOP 1 Culture
		FROM	Content
		WHERE 	ModuleId = @ModuleId
		AND	StatusId = 1
		ORDER BY (
			CASE Culture 
				WHEN @culture THEN 1 
				WHEN LEFT(@Culture, 2) THEN 2 
				WHEN '' THEN 3 
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


