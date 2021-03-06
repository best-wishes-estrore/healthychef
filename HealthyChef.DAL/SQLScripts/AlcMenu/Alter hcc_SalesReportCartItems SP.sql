IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_SalesReportCartItems]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[hcc_SalesReportCartItems]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_SalesReportCartItems] 
( @CartId INT )
AS 
    BEGIN
        SET NOCOUNT ON
		SELECT T.*
		FROM (
			SELECT  --up.ProfileName ,
					ad.PostalCode ,
					ci.ItemName ,
					ci.Quantity ,
					ci.TaxRate ,
					ci.TaxableAmount ,
					ci.DiscretionaryTaxAmount ,
					ci.OrderNumber ,
					ci.DeliveryDate ,
					ci.IsCompleted ,
					ci.IsCancelled ,
					ci.IsFulfilled ,
					ci.DiscountPerEach ,
					ci.DiscountAdjPrice ,
					( CASE WHEN ci.TaxRate > 6.5
								OR ci.TaxRate < 6.5 THEN TaxableAmount
						   ELSE 0
					  END ) AS TaxableSalesDifferentRate
			FROM    hccCartItems ci
					LEFT JOIN hccUserProfiles up ON ci.UserProfileID = up.UserProfileID
					LEFT JOIN dbo.hccAddresses ad ON ad.AddressID = ci.SnapShipAddrId
			WHERE   ci.CartID = @CartId
					AND ci.IsCancelled = 0
					AND ci.CartItemID NOT IN (SELECT CartItemID FROM hccCartALCMenuItem)
			UNION ALL
			SELECT  --up.ProfileName ,
					ad.PostalCode ,
					ci.ItemName ,
					ci.Quantity ,
					ci.TaxRate ,
					ci.TaxableAmount ,
					ci.DiscretionaryTaxAmount ,
					ci.OrderNumber ,
					ci.DeliveryDate ,
					ci.IsCompleted ,
					ci.IsCancelled ,
					ci.IsFulfilled ,
					ci.DiscountPerEach ,
					ci.DiscountAdjPrice ,
					( CASE WHEN ci.TaxRate > 6.5
								OR ci.TaxRate < 6.5 THEN TaxableAmount
						   ELSE 0
					  END ) AS TaxableSalesDifferentRate
			FROM    hccCartItems ci
					LEFT JOIN hccUserProfiles up ON ci.UserProfileID = up.UserProfileID
					LEFT JOIN dbo.hccAddresses ad ON ad.AddressID = ci.SnapShipAddrId
					LEFT JOIN dbo.hccMenuItems mi ON mi.MenuItemID = ci.Meal_MenuItemID
			WHERE   ci.CartID = @CartId
					AND ci.IsCancelled = 0
					AND ci.CartItemID IN (SELECT CartItemID FROM hccCartALCMenuItem)
					AND mi.MealTypeID NOT IN (20, 40, 60, 80, 100) -- exclude meal sides
		) T
		ORDER BY T.OrderNumber;
		
    END
					
