/****** Object:  StoredProcedure [dbo].[hcc_RecurringOrderStartDate]    ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_RecurringOrderStartDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_RecurringOrderStartDate]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_RecurringOrderStartDate]
	-- Add the parameters for the stored procedure here
	@CartId			int,
	@CartItemId		int
AS
BEGIN

	SET NOCOUNT ON;

    SELECT a.CartID, a.CartItemID, MaxDeliveryDate = MAX(c.DeliveryDate), MAX(c.OrderCutOffDate) AS MaxCutOffDate
	FROM hccRecurringOrder a join hccCartItemCalendars b on (a.CartItemID = b.CartItemID)
							 join hccProductionCalendars c on (b.CalendarID = c.CalendarID)
	where a.CartID = @CartId and a.CartItemID = @CartItemId
	GROUP BY a.CartID, a.CartItemID;

END
GO
