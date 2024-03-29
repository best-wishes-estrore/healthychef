/****** Object:  StoredProcedure [dbo].[hcc_GetExpiringOrders]    Script Date: 10/12/2013 8:36:48 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_GetExpiringOrders]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_GetExpiringOrders]
GO

CREATE PROCEDURE [dbo].[hcc_GetExpiringOrders]
AS
BEGIN
	SET NOCOUNT ON
	--Maintains Press Releases from Transfer Tables into Prod Tables.

	DECLARE @Results TABLE (CartID int NULL,
							CartItemID int NULL,
							MaxDeliveryDate date,
							MaxCutOffDate date,
							UserProfileID int NULL)

	INSERT INTO @Results
		SELECT        
			a.CartID, a.CartItemID, MAX(c.DeliveryDate) AS MaxDeliveryDate, MAX(c.OrderCutOffDate) AS MaxCutOffDate, a.UserProfileID
		FROM            
			hccRecurringOrder AS a INNER JOIN
					hccCartItemCalendars AS b ON a.CartItemID = b.CartItemID INNER JOIN
					hccProductionCalendars AS c ON b.CalendarID = c.CalendarID INNER JOIN
					hccCartItems AS d ON b.CartItemID = d.CartItemID
		GROUP BY 
			a.CartID, a.CartItemID, d.DeliveryDate, a.UserProfileID			 
	
	DECLARE @CurrentCutOffDate datetime 
	SELECT @CurrentCutOffDate = DATEADD(day, 1,Min(MaxCutOffDate)) FROM @Results;

	SELECT 
		--DISTINCT CartID, CartItemID, MaxDeliveryDate, MaxCutOffDate
		DISTINCT CartID, CartItemID, UserProfileID
	FROM 
		@Results
	WHERE
		CAST(GETDATE() as date) between CAST(MaxDeliveryDate as date) and CAST(@CurrentCutOffDate as date) 
		OR CAST(GETDATE() as date) > CAST(MaxCutOffDate as date);

	--SELECT 
	--	CartID, CartItemID, MaxDeliveryDate, MaxCutOffDate 
	--FROM 
	--	@Results
	--WHERE
	--	CAST(GETDATE() as date) >= CAST(MaxCutOffDate as date)  		
		--MaxCutOffDate = DATEADD(wk,DATEDIFF(wk,0,GETDATE()),6) 
		--CAST(GETDATE() as date) between CAST(DATEADD(d, -6, MaxCutOffDate) as date) AND CAST(GETDATE() as date)

	RETURN
END