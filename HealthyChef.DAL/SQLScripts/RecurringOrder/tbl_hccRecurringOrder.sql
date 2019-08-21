/****** Object:  Table [dbo].[hccRecurringOrder]    Script Date: 11/1/2013 2:39:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[hccRecurringOrder](
	[CartID] [int] NOT NULL,
	[CartItemID] [int] NOT NULL,
	[UserProfileID] [int] NULL,
	[AspNetUserID] [uniqueidentifier] NULL,
	[PurchaseNumber] [int] NULL,
	[TotalAmount] [smallmoney] NULL,
 CONSTRAINT [PK_hccRecurringOrder] PRIMARY KEY CLUSTERED 
(
	[CartID] ASC,
	[CartItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


