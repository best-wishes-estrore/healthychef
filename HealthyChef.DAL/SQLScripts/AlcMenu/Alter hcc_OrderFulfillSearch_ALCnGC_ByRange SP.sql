IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_OrderFulfillSearch_ALCnGC_ByRange]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[hcc_OrderFulfillSearch_ALCnGC_ByRange]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_OrderFulfillSearch_ALCnGC_ByRange]
    @startDate DATETIME = NULL ,
    @endDate DATETIME = NULL
AS 
    SET NOCOUNT OFF
	
	-- AlaCarte and GiftCerts 
	SELECT T.*
	FROM (
		SELECT  ci.*
		FROM    dbo.hccCartItems ci
				INNER JOIN dbo.hccCarts ct ON ci.CartID = ct.CartID
		WHERE   ct.StatusID = 20
				AND ci.DeliveryDate BETWEEN @startDate AND @endDate
				AND ci.ItemTypeID IN ( 1, 3 ) -- Status 20 == Paid, itemType 1,3 = Alacarte and GiftCerts
				AND ci.IsCancelled = 0
				AND ci.CartItemID NOT IN (SELECT CartItemID FROM hccCartALCMenuItem)
		UNION ALL
		SELECT  ci.*
		FROM    dbo.hccCartItems ci
				INNER JOIN dbo.hccCarts ct ON ci.CartID = ct.CartID
				LEFT JOIN dbo.hccMenuItems mi ON mi.MenuItemID = ci.Meal_MenuItemID
		WHERE   ct.StatusID = 20
				AND ci.DeliveryDate BETWEEN @startDate AND @endDate
				AND ci.ItemTypeID IN ( 1, 3 ) -- Status 20 == Paid, itemType 1,3 = Alacarte and GiftCerts
				AND ci.IsCancelled = 0
				AND ci.CartItemID IN (SELECT CartItemID FROM hccCartALCMenuItem)
				AND mi.MealTypeID NOT IN (20, 40, 60, 80, 100) -- exclude meal sides
	) T
    ORDER BY T.DeliveryDate;
