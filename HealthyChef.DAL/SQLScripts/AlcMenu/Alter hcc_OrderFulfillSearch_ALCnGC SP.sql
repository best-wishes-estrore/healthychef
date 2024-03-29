IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_OrderFulfillSearch_ALCnGC]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[hcc_OrderFulfillSearch_ALCnGC]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_OrderFulfillSearch_ALCnGC]
    @delivDate DATETIME = NULL ,
    @purchNum INT = NULL ,
    @lastName NVARCHAR(50) = NULL ,
    @email NVARCHAR(256) = NULL
AS 
    SET NOCOUNT OFF
	
	-- AlaCarte and GiftCerts 
    SELECT  ci.*
    FROM    dbo.hccCartItems ci
            INNER JOIN dbo.hccCarts ct ON ct.CartID = ci.CartID
            INNER JOIN dbo.aspnet_Membership m ON m.UserId = ct.AspNetUserID
            LEFT JOIN dbo.hccUserProfiles up ON up.UserProfileID = ci.UserProfileID
    WHERE   ct.StatusID > 10
            AND ct.PurchaseDate IS NOT NULL 
			AND ct.PurchaseBy IS NOT NULL
            AND ci.ItemTypeID IN ( 1, 3 ) -- Status 20 == Paid, itemType 1,3 = Alacarte and GiftCerts
            AND up.IsActive = 1
            --AND ci.IsCancelled = 0 -- we want to show cancelled items that were paid for.
            AND ( ( ( @lastName IS NULL )
                    OR ( up.LastName LIKE '%' + @lastName + '%' )
                  )
                  AND ( ( @email IS NULL )
                        OR ( m.Email LIKE '%' + @email + '%' )
                      )
                  AND ( ( @purchNum IS NULL )
                        OR ( ct.PurchaseNumber = @purchNum )
                      )
                  AND ( ( @delivDate IS NULL )
                        OR ( DATEDIFF(dd,
                                      CONVERT(DATETIME, ci.DeliveryDate, 101),
                                      CONVERT(DATETIME, @delivDate, 101)) = 0 )
                      )
                )
			AND ci.CartItemID NOT IN (SELECT CartItemID FROM hccCartALCMenuItem)
	UNION ALL
    SELECT  ci.*
    FROM    dbo.hccCartItems ci
            INNER JOIN dbo.hccCarts ct ON ct.CartID = ci.CartID
            INNER JOIN dbo.aspnet_Membership m ON m.UserId = ct.AspNetUserID
            LEFT JOIN dbo.hccUserProfiles up ON up.UserProfileID = ci.UserProfileID
			LEFT JOIN dbo.hccMenuItems mi ON mi.MenuItemID = ci.Meal_MenuItemID
    WHERE   ct.StatusID > 10
            AND ct.PurchaseDate IS NOT NULL 
			AND ct.PurchaseBy IS NOT NULL
            AND ci.ItemTypeID IN ( 1, 3 ) -- Status 20 == Paid, itemType 1,3 = Alacarte and GiftCerts
            AND up.IsActive = 1
            --AND ci.IsCancelled = 0 -- we want to show cancelled items that were paid for.
            AND ( ( ( @lastName IS NULL )
                    OR ( up.LastName LIKE '%' + @lastName + '%' )
                  )
                  AND ( ( @email IS NULL )
                        OR ( m.Email LIKE '%' + @email + '%' )
                      )
                  AND ( ( @purchNum IS NULL )
                        OR ( ct.PurchaseNumber = @purchNum )
                      )
                  AND ( ( @delivDate IS NULL )
                        OR ( DATEDIFF(dd,
                                      CONVERT(DATETIME, ci.DeliveryDate, 101),
                                      CONVERT(DATETIME, @delivDate, 101)) = 0 )
                      )
                )
			AND ci.CartItemID IN (SELECT CartItemID FROM hccCartALCMenuItem)
			AND mi.MealTypeID NOT IN (20, 40, 60, 80, 100) -- exclude meal sides
   

