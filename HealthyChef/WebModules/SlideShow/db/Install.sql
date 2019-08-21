/****** Object:  Table [dbo].[SlideShow_Module]    Script Date: 09/02/2008 20:06:40 ******/
CREATE TABLE [dbo].[SlideShow_Module](
	[ModuleId] [int] NOT NULL,
	[Height] [smallint] NOT NULL CONSTRAINT [DF_SlideShow_Module_Height]  DEFAULT ((100)),
	[Width] [smallint] NOT NULL CONSTRAINT [DF_SlideShow_Module_Width]  DEFAULT ((100)),
	[ImageDisplayTime] [smallint] NOT NULL CONSTRAINT [DF_SlideShow_Module_ImageDisplayTime]  DEFAULT ((6)),
	[ImageDisplayOrder] [smallint] NOT NULL CONSTRAINT [DF_SlideShow_Module_ImageDisplayOrder]  DEFAULT ((0)),
	[ImageLooping] [bit] NOT NULL CONSTRAINT [DF_SlideShow_Module_ImageLooping]  DEFAULT ((1)),
	[ImageFadeTime] [smallint] NOT NULL CONSTRAINT [DF_SlideShow_Module_ImageFadeTime]  DEFAULT ((1)),
	[ImageXPosition] [int] NOT NULL CONSTRAINT [DF_SlideShow_Module_ImageXPosition]  DEFAULT ((0)),
	[ImageYPosition] [int] NOT NULL CONSTRAINT [DF_SlideShow_Module_ImageYPosition]  DEFAULT ((0)),
	[FlashFileName] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SlideShow_Module] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
))

go

/****** Object:  Table [dbo].[SlideShow_Image]    Script Date: 09/02/2008 20:04:56 ******/
CREATE TABLE [dbo].[SlideShow_Image](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[ImageFileName] [nvarchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SortOrder] [int] NOT NULL CONSTRAINT [DF_SlideShow_Image_SortOrder]  DEFAULT ((1)),
	[LinkUrl] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SlideShow_Image] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)) 

--
-- constraints
--

GO
ALTER TABLE [dbo].[SlideShow_Image]  WITH CHECK ADD  CONSTRAINT [FK_SlideShow_Image_SlideShow_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[SlideShow_Module] ([ModuleId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SlideShow_Image] CHECK CONSTRAINT [FK_SlideShow_Image_SlideShow_Module]

go

--
-- indexes
--

go

/****** Object:  Index [IX_SlideShow_Image]    Script Date: 12/11/2008 16:10:40 ******/
CREATE CLUSTERED INDEX [IX_SlideShow_Image] ON [dbo].[SlideShow_Image] 
(
	[ModuleId] ASC
)

go



--
-- procedures
--



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SlideShow_Image_RebuildSortOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SlideShow_Image_RebuildSortOrder]

go

CREATE procedure [dbo].[SlideShow_Image_RebuildSortOrder]
    @moduleId int
as

declare @temp table (
    TempId int identity primary key, --auto-incremented id used for new sort order.
    [Id] int --the actual primary key of the object.
)

begin transaction 

    insert into @temp ([Id])
        select [Id] 
            from SlideShow_Image 
            where ModuleId = @moduleId
            order by SortOrder asc

    if @@error <> 0 goto on_error

    --update all sort orders at once. this "rebuild from scratch" approach is 
    --robust: if the SortOrder was corrupted somehow, it recovers.
    update SlideShow_Image
        set SortOrder = t.TempId
        from @temp t
        where SlideShow_Image.[Id] = t.[Id]
    
    if @@error <> 0 goto on_error

commit transaction
    
return
    
on_error:
    rollback transaction
    return

go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SlideShow_Image_MovePosition]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SlideShow_Image_MovePosition]

go

--
--  moves a SlideShow image SortOrder up or down.
--
CREATE procedure [dbo].[SlideShow_Image_MovePosition]
    @id int,
    @direction bit -- 0: up; 1: down.
as

declare	@oldSortOrder int,
        @newSortOrder int,
        @movedId int,
        @moduleId int

begin transaction 

    select @moduleId = ModuleId,
        @oldSortOrder = SortOrder
        from SlideShow_Image 
        where [Id] = @id

    if @@error <> 0 goto on_error

    --sanity check: if more than one row has the same sort order, clean up.
    if 1 < (select count(1) 
            from SlideShow_Image
            where ModuleId = @moduleId and SortOrder = @oldSortOrder) 
    begin
        exec [SlideShow_Image_RebuildSortOrder] @moduleId

        if @@error <> 0 goto on_error
    end

    --up
    if(@direction = 0) begin
        set @newSortOrder = (select max(SortOrder) from SlideShow_Image 
                             where ModuleId = @moduleId and SortOrder < @oldSortOrder)

        if @@error <> 0 goto on_error
    end
    --down
    else begin
        set @newSortOrder = (select min(SortOrder) from SlideShow_Image 
                             where ModuleId = @moduleId and SortOrder > @oldSortOrder)

        if @@error <> 0 goto on_error
    end

    if @newSortOrder is not null
    begin
        set @movedId = (select [Id] from SlideShow_Image
                        where ModuleId = @moduleId and SortOrder = @newSortOrder)

        if @@error <> 0 goto on_error

        update SlideShow_Image
        set SortOrder = @newSortOrder
        where [Id] = @id

        if @@error <> 0 goto on_error

        update SlideShow_Image
        set SortOrder = @oldSortOrder
        where [Id] = @movedId

        if @@error <> 0 goto on_error
    end

    --rebuild the sort order
    exec [SlideShow_Image_RebuildSortOrder] @moduleId

    if @@error <> 0 goto on_error

commit transaction
    
return
    
on_error:
    rollback transaction
    return

go

/* add new js slide fields */

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
ALTER TABLE dbo.SlideShow_Module ADD
	NavType int NOT NULL CONSTRAINT DF_SlideShow_Module_NavType DEFAULT 0
GO
ALTER TABLE dbo.SlideShow_Module SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

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
ALTER TABLE dbo.SlideShow_Image ADD
	SlideTextContent nvarchar(MAX) NULL,
	SlideTextContentName nvarchar(500) NULL
GO
ALTER TABLE dbo.SlideShow_Image SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

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
ALTER TABLE dbo.SlideShow_Module ADD
	WrapType int NOT NULL CONSTRAINT DF_SlideShow_Module_WrapType DEFAULT 0
GO
ALTER TABLE dbo.SlideShow_Module SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
