/****** Object:  StoredProcedure [dbo].[hcc_PurchasesSearch]    Script Date: 10/9/2013 4:39:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[hcc_PurchasesSearch]
    @email NVARCHAR(256) = NULL ,
    @purchaseNumber VARCHAR(50) = NULL ,
    @purchaseDate DATETIME = NULL ,
    @lastName NVARCHAR(256) = NULL ,
    @deliveryDate DATETIME = NULL
AS 
    SET NOCOUNT ON
    
    SELECT DISTINCT
            ( c.CartID ) ,
            c.AspNetUserID ,
            c.PurchaseNumber ,
            c.StatusID ,
            c.SubTotalAmount ,
            c.SubTotalDiscount ,
            c.ShippingAmount ,
            c.ShippingDiscount ,
            c.TaxableAmount ,
            c.DiscretionaryTaxAmount ,
            c.TaxAmount ,
            c.TaxDiscount ,
            c.TotalAmount ,
            c.TotalDiscount ,
            c.CouponID ,
            c.PurchaseDate ,
            c.PurchaseBy ,
            c.CreatedDate ,
            c.CreatedBy ,
            c.AuthNetResponse ,
            c.AnonGiftRedeemCredit ,
            c.IsTestOrder ,
            c.CreditAppliedToBalance ,
            c.PaymentDue ,
            c.PaymentProfileID ,
            c.RedeemedGiftCertCode ,
            c.AnonymousID ,
            c.ModifiedBy ,
            c.ModifiedDate,
			c.TaxRate
    FROM    dbo.hccCarts c
            LEFT JOIN dbo.hccCartItems ci ON c.CartID = ci.CartID
            LEFT JOIN dbo.aspnet_Membership a ON c.AspNetUserID = a.UserId
            LEFT JOIN dbo.hccUserProfiles p ON a.UserId = p.MembershipID
            LEFT JOIN dbo.hccCartItemCalendars cic ON cic.CartItemID = ci.CartItemID
            LEFT JOIN dbo.hccProductionCalendars pc ON pc.CalendarID = cic.CalendarID
    WHERE   ( ( ( @email IS NULL )
                OR ( a.Email LIKE '%' + @email + '%' )
              )
              AND ( ( @purchaseNumber IS NULL )
                    OR ( ci.OrderNumber LIKE '%' + @purchaseNumber + '%' )
                  )
              AND ( ( @purchaseDate IS NULL )
                    OR ( DATEDIFF(dd, CONVERT(DATETIME, c.PurchaseDate, 101),
                                  CONVERT(DATETIME, @purchaseDate, 101)) = 0 )
                  )
              AND ( ( @lastName IS NULL )
                    OR ( p.LastName LIKE '%' + @lastName + '%' )
                  )
              AND ( ( @deliveryDate IS NULL )
                    OR ( ( DATEDIFF(dd,
                                    CONVERT(DATETIME, ci.DeliveryDate, 101),
                                    CONVERT(DATETIME, @deliveryDate, 101)) = 0 )
                         OR ( DATEDIFF(dd,
                                       CONVERT(DATETIME, pc.DeliveryDate, 101),
                                       CONVERT(DATETIME, @deliveryDate, 101)) = 0 )
                       )
                  )
            )