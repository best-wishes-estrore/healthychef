IF OBJECT_ID('dbo.hccCartALCMenuItem', 'U') IS NOT NULL
BEGIN
  DROP TABLE [dbo].[hccCartALCMenuItem]; 
END
GO

CREATE TABLE [dbo].[hccCartALCMenuItem](
  [ID] [int] IDENTITY(1,1) NOT NULL,
  [CartItemID] [int] NOT NULL,
  [ParentCartItemID] [int] NOT NULL,
  [Ordinal] [int] NOT NULL,
  CONSTRAINT [PK_hccCartALCMenuItem] PRIMARY KEY CLUSTERED 
  (
    [ID] ASC
  ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[hccCartALCMenuItem]  WITH NOCHECK ADD  CONSTRAINT [FK_hccCartALCMenuItem_hccCartItems_CartItemID] FOREIGN KEY([CartItemID])
  REFERENCES [dbo].[hccCartItems] ([CartItemID])
  ON DELETE CASCADE
GO