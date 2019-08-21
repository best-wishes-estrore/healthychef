/****** Object:  StoredProcedure [dbo].[hcc_RecurringOrderByDeliveryDate]   ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_RecurringOrderByDeliveryDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_RecurringOrderByDeliveryDate]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_RecurringOrderByDeliveryDate]

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @I int = 1
	DECLARE @md_cartid int
	DECLARE @md_cartitemid int
	--DECLARE @md_userprofileid int
	--DECLARE @md_aspnetuserid uniqueidentifier
	--DECLARE @md_purchasenumber int
	DECLARE @md_maxdeliverydate datetime
	DECLARE @md_qunatity int

	DECLARE @MaxDateTable TABLE
	(
		cartid int,
		cartitemid int,
		userprofileid int,
		aspnetuserid uniqueidentifier,
		purchasenumber int,
		maxdeliverydate datetime,
		maxcutoffdate datetime,
		quantity int
	)

	DECLARE @rosd TABLE
	(
		ci int,
		cii int,
		mdd datetime,
		mcd datetime
	)

	INSERT INTO
		@MaxDateTable
		(cartid,
		cartitemid,
		userprofileid,
		aspnetuserid,
		purchasenumber)
			SELECT 
				[CartID]
				,[CartItemID]
				,[UserProfileID]
				,[AspNetUserID]
				,[PurchaseNumber]
			FROM [dbo].[hccRecurringOrder];
			--SELECT a.CartID, a.CartItemID, c.DeliveryDate
			--FROM hccRecurringOrder a join hccCartItemCalendars b on (a.CartItemID = b.CartItemID)
			--					 join hccProductionCalendars c on (b.CalendarID = c.CalendarID);

		DECLARE @RowCount int = (SELECT COUNT(cartid) FROM @MaxDateTable)

	-- Loop through data rows
	WHILE (@I <= @RowCount)
		BEGIN
			WITH MD AS (SELECT cartid, cartitemid, (ROW_NUMBER() OVER (ORDER BY cartid)) AS nRow FROM @MaxDateTable) 
			SELECT @md_cartid = cartid, @md_cartitemid = cartitemid FROM MD WHERE nRow = @I;
			--, @md_userprofileid = userprofileid, @md_purchasenumber = purchasenumber

			INSERT INTO  @rosd 
			EXEC [dbo].[hcc_RecurringOrderStartDate] @CartId = @md_cartid, @CartItemId = @md_cartitemid;

			UPDATE @MaxDateTable
			SET maxdeliverydate = mdd 
			FROM  @rosd				
			WHERE  cartid = @md_cartid AND cartitemid = @md_cartitemid;

			UPDATE  @MaxDateTable
			SET maxcutoffdate = mcd 
			FROM  @rosd				
			WHERE cartid = @md_cartid AND cartitemid = @md_cartitemid;

			SELECT @md_qunatity = hccCartItems.Quantity
			FROM hccCartItems
			WHERE hccCartItems.CartID = @md_cartid AND hccCartItems.CartItemID = @md_cartitemid;

			UPDATE @MaxDateTable
			SET quantity = @md_qunatity
			WHERE cartid = @md_cartid AND cartitemid = @md_cartitemid;

			DELETE FROM @rosd;
				
			SET @I = @I + 1;
		END

	SELECT * FROM @MaxDateTable ORDER BY maxdeliverydate;

		--(a.CartID, a.CartItemID, MaxDeliveryDate = MAX(c.DeliveryDate)
		--FROM hccRecurringOrder a join hccCartItemCalendars b on (a.CartItemID = b.CartItemID)
		--						 join hccProductionCalendars c on (b.CalendarID = c.CalendarID)
		--Group by c.DeliveryDate, a.CartID, a.CartItemID
		--Order by MAX(c.DeliveryDate))

END
GO
