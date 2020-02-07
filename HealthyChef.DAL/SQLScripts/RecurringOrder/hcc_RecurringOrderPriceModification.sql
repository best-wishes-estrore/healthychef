/****** Object:  StoredProcedure [dbo].[hcc_RecurringOrderPriceModification] ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_RecurringOrderPriceModification]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_RecurringOrderPriceModification]
GO

CREATE PROCEDURE [dbo].[hcc_RecurringOrderPriceModification] 
	@CartId Int
AS
BEGIN

	-- Decrement the price by a cent if an order is a duplicate during a auto-renewal operation
	SET NOCOUNT ON;

	UPDATE
		hccCarts
	SET
		SubTotalAmount = SubTotalAmount - .01,
		PaymentDue = PaymentDue - .01,
		TotalAmount = TotalAmount - .01,
		TaxableAmount = TaxableAmount - .01
	WHERE
		CartID = @CartId;

END
GO
