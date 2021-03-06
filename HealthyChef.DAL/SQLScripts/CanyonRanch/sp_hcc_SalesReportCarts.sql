/****** Object:  StoredProcedure [dbo].[hcc_SalesReportCarts]    Script Date: 04/09/2013 09:24:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_SalesReportCarts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_SalesReportCarts]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_SalesReportCarts]
    (
      @aStartDate DATETIME ,
      @aEndDate DATETIME
    )
AS 
    BEGIN
        SET NOCOUNT ON
        SELECT  c.CartID ,
                c.PurchaseNumber ,
                --up.FirstName + ' ' + up.LastName AS CustomerName ,
                c.PurchaseDate ,
                c.SubTotalAmount ,
                c.SubTotalDiscount ,
                c.CreditAppliedToBalance ,
                c.TaxableAmount ,
                c.DiscretionaryTaxAmount ,
                c.TaxAmount ,
                c.TaxDiscount ,
                c.ShippingAmount ,
                c.ShippingDiscount ,
                c.TotalAmount ,
                c.TotalDiscount ,
                c.PaymentDue ,
                c.ModifiedBy ,
                c.ModifiedDate ,
				am.Email
        FROM    hccCarts c
                LEFT JOIN aspnet_Membership am ON c.AspNetUserID = am.UserId
        WHERE   ( c.StatusID = 20
                  OR c.StatusID = 40
                  --OR ( c.StatusID = 50
                  --     AND c.PurchaseDate IS NOT NULL
                  --     AND c.PurchaseBy IS NOT NULL
                  --   )
                ) -- Paid or Fulfilled or Paid then Cancelled
                --AND up.ParentProfileID IS NULL
                AND ( CONVERT(DATETIME, c.PurchaseDate, 101) BETWEEN CONVERT(DATETIME, @aStartDate, 101)
                                                             AND
                                                              CONVERT(DATETIME, @aEndDate, 101) )
        ORDER BY c.PurchaseDate;
		
    END
					
