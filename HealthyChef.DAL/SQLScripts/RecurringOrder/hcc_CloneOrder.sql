/****** Object:  StoredProcedure [dbo].[hcc_CloneOrder]    Script Date: 1/27/2014 1:24:25 PM ******/
DROP PROCEDURE [dbo].[hcc_CloneOrder]
GO

/****** Object:  StoredProcedure [dbo].[hcc_CloneOrder]    Script Date: 1/27/2014 1:24:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[hcc_CloneOrder] (@CartID int, @CartItemId int, @TaxRate smallmoney, @NewCartID int out)
AS
BEGIN
	SET NOCOUNT ON
	
		DECLARE @CurrentDeliveryDate datetime, 
				@PlanNumberOfWeeks int, 
				@CursorCartItemID int, 
				@OldId varchar(Max),
				@MaxPurchaseNumber int,
				@OriginalPurchaseNumber int,
				@TotalAmount smallmoney,
				@AmountDue smallmoney,
				@CreditAvailable smallmoney,
				@CreditAppliedToBalance smallmoney = 0,
				@MembershipId uniqueidentifier,
				@UserProfileId int,
				@LastBillAddrId int,
				@LastShipAddrId int,
				@NewCartItemId int,
				@RecNumber int

			--Logic to Update Order Price
			DECLARE @NetSubTotal float,
					@SubTotalAmount float,
					@SubTotalDiscount float,
					@PricePerDay float,
					@OptionValue float,
					@PlanId int,
					@ProgramId int,
					@ProgramOptionId int,
					@OrderTaxAmount float,
					@OrderTaxableAmount float,
					@OrderLineAmount float,
					@OrderLineTaxableAmount float,
					@NewOrderDeliveryDate Datetime,
					@LastCalendarID int,
					@CartItemCurrentDeliveryDate datetime,
					@UpcomingDeliveryDate datetime


		--Used to Compute/Recompute Taxes per line
		DECLARE @TaxAmounts TABLE (CartID int NULL,
							CartItemID int NULL,
							UserProfileID int NULL,
							TaxAmount float NULL)


		--Order Header
		--This means that none of the items on this order have been processed and therefore we don't have a header record
		IF NOT EXISTS(SELECT 1 FROM hccRecurringOrder WHERE PrevCartID = @CartID)
		BEGIN
				SELECT @MaxPurchaseNumber = Max(PurchaseNumber) + 1 FROM hccCarts
				SELECT @OriginalPurchaseNumber = PurchaseNumber FROM hccCarts WHERE CartID = @CartID

				--Add hccCarts
				INSERT INTO [dbo].[hccCarts]
							([AspNetUserID]
							,[PurchaseNumber]
							,[StatusID]
							,[SubTotalAmount]
							,[SubTotalDiscount]
							,[ShippingAmount]
							,[ShippingDiscount]
							,[TaxAmount]
							,[TaxDiscount]
							,[TotalAmount]
							,[TotalDiscount]
							,[CouponID]
							,[PurchaseDate]
							,[PurchaseBy]
							,[CreatedDate]
							,[CreatedBy]
							,[AuthNetResponse]
							,[AnonGiftRedeemCredit]
							,[IsTestOrder]
							,[CreditAppliedToBalance]
							,[PaymentDue]
							,[PaymentProfileID]
							,[RedeemedGiftCertCode]
							,[AnonymousID]
							,[TaxableAmount]
							,[DiscretionaryTaxAmount]
							,[ModifiedDate]
							,[ModifiedBy]
							,[TaxRate])
				SELECT		[AspNetUserID]
							,@MaxPurchaseNumber
							,10
							,0 -- [SubTotalAmount]
							,0 -- Per Duncan, SubTotalDiscount does not carry over 4/16/14
							,0 -- Per Duncan, ShippingAmount does not carry over 4/16/14
							,0 -- [ShippingDiscount]
							,0 -- [TaxAmount]
							,0 -- [TaxDiscount]
							,0 -- [TotalAmount]
							,0 -- [TotalDiscount]
							,null -- Per Duncan, CouponID does not carry over 4/16/14
							,getdate()
							,[PurchaseBy]
							,getdate()
							,[CreatedBy]
							,''
							,[AnonGiftRedeemCredit]
							,[IsTestOrder]
							,[CreditAppliedToBalance]
							,[PaymentDue]
							,[PaymentProfileID]
							,[RedeemedGiftCertCode]
							,[AnonymousID]
							,0 -- [TaxableAmount]
							,0 -- [DiscretionaryTaxAmount]
							,getdate()
							,[ModifiedBy]
							,null --[TaxRate]
					FROM hccCarts 
					WHERE CartID = @CartID
	
			--Get the new CartID
			SELECT @NewCartID = @@IDENTITY 
		END
		ELSE
		BEGIN
				--Header already created, so take the values from there
				SELECT @MaxPurchaseNumber = PurchaseNumber FROM hccRecurringOrder WHERE PrevCartID = @CartID
				
				SELECT @OriginalPurchaseNumber = a.PurchaseNumber 
				FROM hccCarts a JOIN hccRecurringOrder b ON (a.CartID = b.CartID)
				WHERE b.PrevCartID = @CartID
				
				SELECT @NewCartID = CartID FROM hccRecurringOrder WHERE PrevCartID = @CartID
		END

		--Gets Record Number to conform Order Number
		SELECT @RecNumber = COUNT(*) + 1
		FROM  hccCartItems
		WHERE CartID = @NewCartID

		
		--Order Line (Processing one line at the time now...)
		--Add hccCartItems
		INSERT INTO [dbo].[hccCartItems]
				   ([CartID]
				   ,[UserProfileID]
				   ,[ItemTypeID]
				   ,[ItemName]
				   ,[ItemDesc]
				   ,[ItemPrice]
				   ,[Quantity]
				   ,[IsTaxable]
				   ,[OrderNumber]
				   ,[DeliveryDate]
				   ,[Gift_RedeemCode]
				   ,[Gift_IssuedTo]
				   ,[Gift_IssuedDate]
				   ,[Gift_RedeemedBy]
				   ,[Gift_RedeemedDate]
				   ,[Gift_RecipientAddressId]
				   ,[Gift_RecipientEmail]
				   ,[Gift_RecipientMessage]
				   ,[Meal_MenuItemID]
				   ,[Meal_MealSizeID]
				   ,[Meal_ShippingCost]
				   ,[Plan_PlanID]
				   ,[Plan_ProgramOptionID]
				   ,[Plan_IsAutoRenew]
				   ,[CreatedBy]
				   ,[CreatedDate]
				   ,[IsCompleted]
				   ,[IsCancelled]
				   ,[IsFulfilled]
				   ,[DiscountPerEach]
				   ,[DiscountAdjPrice]
				   ,[SnapBillAddrId]
				   ,[SnapShipAddrId]
				   ,[TaxRate]
				   ,[TaxableAmount]
				   ,[DiscretionaryTaxAmount]
				   ,[TaxRateAssigned])
		SELECT		@NewCartID
				   ,[UserProfileID]
				   ,[ItemTypeID]
				   ,LEFT([ItemName], LEN(ItemName) - 11)
				   ,[ItemDesc]
				   ,[ItemPrice]
				   ,[Quantity]
				   ,[IsTaxable]
				   ,CAST(@MaxPurchaseNumber AS nvarchar) + '-' + REPLACE(STR(@RecNumber, 2, 0), ' ', '0')
				   --,REPLACE(CAST(@OriginalPurchaseNumber AS nvarchar), CAST(@MaxPurchaseNumber AS nvarchar), [OrderNumber])
				   ,[DeliveryDate]
				   ,NULL
				   ,NULL
				   ,NULL
				   ,NULL
				   ,NULL
				   ,NULL
				   ,NULL
				   ,CAST(CartItemID as nvarchar) --Use it to store the Old ID
				   ,[Meal_MenuItemID]
				   ,[Meal_MealSizeID]
				   ,[Meal_ShippingCost]
				   ,[Plan_PlanID]
				   ,[Plan_ProgramOptionID]
				   ,[Plan_IsAutoRenew]
				   ,[CreatedBy]
				   ,getdate()
				   ,0
				   ,0
				   ,0
				   ,0 -- Per Duncan, DiscountPerEach does not carry over 4/16/14
				   ,[DiscountAdjPrice]
				   ,[SnapBillAddrId]
				   ,[SnapShipAddrId]
				   ,[TaxRate]
				   ,[TaxableAmount]
				   ,[DiscretionaryTaxAmount]
				   ,@TaxRate
			  FROM hccCartItems
			  WHERE CartID = @CartID and CartItemID = @CartItemId and ItemTypeID = 2 --Plans Only

			SELECT @NewCartItemId = @@IDENTITY

		

			--Need to apply the discount in order to find the correct Tax Rate used in the computation.
			SELECT @SubTotalDiscount = SubTotalDiscount FROM hccCarts WHERE CartID = @CartID
			SET @PricePerDay = 0
			SET @OptionValue = 0
			SET @PlanId = 0	
			SET @ProgramId = 0	
			SET @ProgramOptionId = 0	
			SET @SubTotalAmount = 0
			SET @OrderTaxAmount = 0
			SET @OrderTaxableAmount = 0
			--Reprice Logic

			SELECT @PlanId = Plan_PlanID, @ProgramOptionId = Plan_ProgramOptionID
			FROM hccCartItems
			WHERE CartID = @NewCartID and CartItemID = @NewCartItemId;

			SELECT @ProgramId = ProgramID, @PlanNumberOfWeeks = NumWeeks
			FROM hccProgramPlans
			WHERE PlanID = @PlanId

			------------------------Computation of Dates------------------------------------------------------

			--Find the last date used in the Production Calendar (Last Scheduled Delivery Date)
			SELECT @LastCalendarID = Max(CalendarID) 
			FROM hccCartItemCalendars
			WHERE CartItemID = @CartItemId

			
			--Check it the given delivery date is less than today
			SELECT @CartItemCurrentDeliveryDate = DeliveryDate
			FROM hccProductionCalendars
			WHERE CalendarID = @LastCalendarID

			IF (@CartItemCurrentDeliveryDate <= GETDATE())
			BEGIN
				--If the item is overdue, then pick the current production calendar record.
				SELECT TOP 1 @LastCalendarID = CalendarID, @UpcomingDeliveryDate = DeliveryDate
				FROM hccProductionCalendars 
				WHERE DeliveryDate <= CAST(GETDATE() AS DATE)
				ORDER BY DeliveryDate DESC

				--If the next delivery date is today, (Friday) then skip to the following production calendar record
				IF (@UpcomingDeliveryDate = CAST(GETDATE() AS DATE))
					SELECT TOP 1 @LastCalendarID = CalendarID
					FROM hccProductionCalendars 
					WHERE DeliveryDate > CAST(GETDATE() AS DATE)
			END


			--Creating delivery dates, these entries are used for Order Fullfilment
			INSERT INTO [hccCartItemCalendars] ([CartItemID], [CalendarID], [IsFulfilled])
			SELECT TOP (@PlanNumberOfWeeks) @NewCartItemId, CalendarID, 0
			FROM hccProductionCalendars b
			WHERE CalendarID > @LastCalendarID
			--Using the Key to get the records assuming the calendar was entered in the right sequence. May need to use the delivery date instead.

			--New Start Delivery Date
			SELECT @CurrentDeliveryDate = Min(b.DeliveryDate)
			FROM hccCartItemCalendars a JOIN hccProductionCalendars b ON (a.CalendarID = b.CalendarID)
			WHERE CartItemID = @NewCartItemId

			------------------------End Computation of Dates------------------------------------------------------

		     -----------------------Re-Pricing Logic--------------------------------------------------------------

			SELECT @OptionValue = IsNull(OptionValue, 0)
			FROM hccProgramOptions
			WHERE ProgramOptionID = @ProgramOptionId and ProgramID = @ProgramId

			SELECT @PricePerDay = (PricePerDay + @OptionValue) * (NumWeeks * NumDaysPerWeek)
			FROM hccProgramPlans
			WHERE PlanID = @PlanId

			--SELECT 'Step 1: Price per Day', @PricePerDay

			UPDATE hccCartItems	
			SET 
				ItemName = ItemName + ' ' + CONVERT(nvarchar(10), @CurrentDeliveryDate, 101),
				DeliveryDate = @CurrentDeliveryDate,
				--Re-Pricing Logic
				ItemPrice = Round(@PricePerDay, 2),
				DiscountAdjPrice = Round(@PricePerDay, 2) - DiscountPerEach,
				TaxableAmount = Round(@PricePerDay * Quantity, 2) - DiscountPerEach,
				--End Re-Pricing Logic
				Gift_RecipientMessage = NULL
			WHERE CartID = @NewCartID and CartItemID = @NewCartItemId;


			--Re-Pricing Header -- Get new line pricess and add them
			SELECT @SubTotalAmount = SUM(ItemPrice * Quantity)
			FROM hccCartItems
			WHERE CartID = @NewCartID and ItemTypeID = 2

			--SELECT 'Step 2: SubTotalAmount', @SubTotalAmount

			--Compute Taxes per line on the order
			INSERT INTO @TaxAmounts (CartID, CartItemID, UserProfileID, TaxAmount)
			SELECT CartID, CartItemID, UserProfileID, Round(IsNull(TaxableAmount, 0) * IsNull(TaxRateAssigned * 0.01, 0), 2)
			FROM hccCartItems
			WHERE CartID = @NewCartID and ItemTypeID = 2

			SELECT @OrderTaxAmount = SUM(TaxAmount)
			FROM @TaxAmounts
			WHERE CartID = @NewCartID 

			--SELECT 'Step 3: OrderTaxAmount', @OrderTaxAmount

			--Update the Order header with prices from lines
			UPDATE hccCarts
			SET SubTotalAmount = Round(@SubTotalAmount,2), 
				TaxAmount = Round(@OrderTaxAmount, 2),
				TotalAmount = Round(@SubTotalAmount + @OrderTaxAmount, 2), -- Modified to ignore discounts
				TaxableAmount = @SubTotalAmount -- @SubTotalDiscount, No Discounts
			WHERE CartId = @NewCartID

			--If lines don't have a tax rate, then nullify columns, this is done with the sole purpose of matching illogical front-end
			UPDATE hccCartItems
			SET DiscountAdjPrice = 0, TaxableAmount = NULL, DiscretionaryTaxAmount = NULL, TaxRateAssigned = NULL
			WHERE CartID = @NewCartID and CartItemID = @NewCartItemId and TaxRateAssigned = 0
			
			SELECT @OrderTaxableAmount = SUM(IsNull(TaxableAmount, 0))
			FROM hccCartItems
			WHERE CartID = @NewCartID

			--SELECT 'Step 4: @OrderTaxableAmount', @OrderTaxableAmount

			UPDATE hccCarts
			SET TaxableAmount = @OrderTaxableAmount
			WHERE CartID = @NewCartID 
			--End Tax Rate updates

			--Apply Customer Credits
			SELECT @TotalAmount = TotalAmount, @AmountDue = TotalAmount, @MembershipId = AspNetUserID
			FROM hccCarts
			WHERE CartID = @NewCartID 

			SELECT @UserProfileId = UserProfileID
			FROM hccCartItems
			WHERE CartID = @NewCartID and CartItemID = @NewCartItemId

			--SELECT 'Step 5: @TotalAmount, @AmountDue, @MembershipId, @UserProfileId', @TotalAmount, @AmountDue, @MembershipId, @UserProfileId
			
			--Credit Available comes from the parent so use the Membership ID to find the record, 
			--Using Membership ID instead of the UserProfileID due to inconsistent table design by rcreecy
			--Need to check for ParentProfileID is NULL because the system assigns the client to the parent profile only
			SELECT @CreditAvailable = IsNull(AccountBalance, 0)
			FROM hccUserProfiles 
			WHERE MembershipID = @MembershipId and ParentProfileID is NULL

			--SELECT 'Step 6: @@CreditAvailable', @CreditAvailable

			--Get latest billing and shipping address from the profile/subprofile because they may differ
			SELECT @LastBillAddrId = BillingAddressID, @LastShipAddrId = ShippingAddressID 
			FROM hccUserProfiles 
			WHERE UserProfileID = @UserProfileId


			SELECT @OrderLineTaxableAmount = TaxableAmount * (IsNull(TaxRateAssigned, 0) * 0.01),
				   @OrderLineAmount = TaxableAmount
			FROM hccCartItems
			WHERE CartID = @NewCartID and CartItemID = @NewCartItemId

			SET @OrderLineAmount = @OrderLineAmount + @OrderLineTaxableAmount;
			
			--SELECT 'Step 7: @OrderLineTaxableAmount, @OrderLineAmount', @OrderLineTaxableAmount, @OrderLineAmount

			--SELECT @CreditAppliedToBalance = CreditAppliedToBalance, @AmountDue = PaymentDue
			--FROM hccCarts
			--WHERE CartID = @NewCartID

			--SELECT 'Step 8: @@CreditAppliedToBalance, @@AmountDue', @CreditAppliedToBalance, @AmountDue

			IF (@CreditAvailable > 0)
			BEGIN
				IF (@CreditAvailable >= @OrderLineAmount)
				BEGIN
					SELECT @AmountDue = 0
					SET @CreditAppliedToBalance = Round(@OrderLineAmount,2) --What's applied is the total value of the line... No discounts applied.

					--SELECT 'Step 9: @AmountDue, @CreditAppliedToBalance', @AmountDue, @CreditAppliedToBalance
				END
				ELSE
				BEGIN
					SET @AmountDue = @TotalAmount - @CreditAvailable
					SET @CreditAppliedToBalance = @CreditAvailable

					--SELECT 'Step 9: @AmountDue, @CreditAppliedToBalance', @AmountDue, @CreditAppliedToBalance
				END
			END
			ELSE
			BEGIN
				SET @AmountDue = @TotalAmount
				SET @CreditAppliedToBalance = 0
			END

			UPDATE hccCarts
			SET CreditAppliedToBalance = @CreditAppliedToBalance, PaymentDue = @AmountDue
			WHERE CartId = @NewCartID
			--End Customer Credits

			--Make sure we have the latest address IDs
			UPDATE hccCartItems
			SET SnapBillAddrId = @LastBillAddrId, SnapShipAddrId = @LastShipAddrId
			WHERE CartID = @NewCartID and CartItemID = @NewCartItemId;
			--Address ID

			-----------------End Re-Pricing Logic---------------------------------------------------


			--Eliminate the recurring order records and add records for the new Order
			IF EXISTS (SELECT 1 FROM hccRecurringOrder WHERE CartID = @CartID)
				DELETE FROM hccRecurringOrder WHERE CartID = @CartID
			
			--Add the new records to the Recurring Order table
			INSERT INTO [dbo].[hccRecurringOrder]
           ([CartID]
           ,[CartItemID]
           ,[UserProfileID]
           ,[AspNetUserID]
           ,[PurchaseNumber]
           ,[TotalAmount]
		   ,[PrevCartID])
		   SELECT a.CartID,  b.CartItemID, b.UserProfileID, a.AspNetUserID, a.PurchaseNumber, Round(b.ItemPrice * b.Quantity, 2), @CartID
		   FROM hccCarts a join hccCartItems b on (a.CartID = b.CartID)
		   WHERE a.CartID = @NewCartID and CartItemID = @NewCartItemId;

	RETURN 	
END
GO
			--Obsolete Code left here for reference only-------------------------------------------------------------

			/*
			--Loop thru the new CartItems to create delivery dates
			DECLARE hccCartItems_cursor CURSOR FOR  
			SELECT CartItemID, Gift_RecipientMessage
			FROM hccCartItems
			WHERE CartID = @NewCartID

			OPEN hccCartItems_cursor   
			FETCH NEXT FROM hccCartItems_cursor INTO @CursorCartItemID, @OldId   

			WHILE @@FETCH_STATUS = 0   
			BEGIN   

				--This gets the Max Current Delivery Date
				--SELECT TOP 1 @CurrentDeliveryDate = DATEADD(D, 1, c.DeliveryDate), @PlanNumberOfWeeks = d.NumWeeks
				--FROM hccCartItems a join hccCartItemCalendars b on (a.CartItemID = b.CartItemID)
				--					join hccProductionCalendars c on (b.CalendarID = c.CalendarID)
				--					join hccProgramPlans d on (a.Plan_PlanID = d.PlanID)
				--WHERE a.CartID = @CartID and a.CartItemID = CAST(@OldId as INT)
				--ORDER BY c.DeliveryDate DESC

				--if(DateDiff(D, cast(GetDate() as Date), @CurrentDeliveryDate) < 7)
				--if(DateDiff(D, cast(GetDate() as Date), @CurrentDeliveryDate) < 7)
				--	SELECT @CurrentDeliveryDate = DATEADD(DAY, 13, @CurrentDeliveryDate);
				----Else if(DateDiff(D, cast(GetDate() as Date), @CurrentDeliveryDate) > 7 AND @PlanNumberOfWeeks = 1)
				----	SELECT @CurrentDeliveryDate = DATEADD(DAY, 6, @CurrentDeliveryDate);	
				--Else 
				--	SELECT @CurrentDeliveryDate = DATEADD(DAY, 6, @CurrentDeliveryDate);

				INSERT INTO [hccCartItemCalendars] ([CartItemID], [CalendarID], [IsFulfilled])
				SELECT  @CursorCartItemID, CalendarID, 0
				FROM hccProductionCalendars 
				WHERE DeliveryDate = @CurrentDeliveryDate --between @CurrentDeliveryDate and DATEADD(WEEK, @PlanNumberOfWeeks, @CurrentDeliveryDate)	

				SELECT @CalendarId = @@IDENTITY 

				SELECT @NewItemName = ItemName 
				FROM hccCartItems
				WHERE CartItemID = @CursorCartItemID;

				--SELECT @NewOrderDeliveryDate = DATEADD(DAY, 6, @CurrentDeliveryDate);

				--if(DateDiff(D, cast(GetDate() as Date), @CurrentDeliveryDate) < 7)
				--	SELECT @NewOrderDeliveryDate = DATEADD(DAY, 13, @CurrentDeliveryDate);	
				--Else 
				--	SELECT @NewOrderDeliveryDate = DATEADD(DAY, 6, @CurrentDeliveryDate);
				--if(cast(@CurrentDeliveryDate as Date) <= cast(GetDate() as Date))
				--	SELECT @NewOrderDeliveryDate = DATEADD(DAY, 12, @CurrentDeliveryDate);	
				--Else 
				--	SELECT @NewOrderDeliveryDate = DATEADD(DAY, 6, @CurrentDeliveryDate);



				--Re-Pricing Logic
				SELECT @PlanId = Plan_PlanID, @ProgramOptionId = Plan_ProgramOptionID
				FROM hccCartItems
				WHERE CartItemID = @CursorCartItemID and CartID = @NewCartID;

				SELECT @ProgramId = ProgramID
				FROM hccProgramPlans
				WHERE PlanID = @PlanId

				SELECT @OptionValue = IsNull(OptionValue, 0)
				FROM hccProgramOptions
				WHERE ProgramOptionID = @ProgramOptionId and ProgramID = @ProgramId

				SELECT @PricePerDay = (PricePerDay + @OptionValue) * (NumWeeks * NumDaysPerWeek)
				FROM hccProgramPlans
				WHERE PlanID = @PlanId
				--End Re-Pricing Logic


				UPDATE 
					hccCartItems	
				SET 
					--CONVERT(nvarchar(10), CAST(DATEADD(WEEK, @PlanNumberOfWeeks, @CurrentDeliveryDate) as Date))
					--CONVERT(nvarchar(10), CONVERT(DateTime, CAST(DATEADD(WEEK, @PlanNumberOfWeeks, @CurrentDeliveryDate) AS DateTime), 101));
					ItemName = ItemName + ' ' + CONVERT(nvarchar(10), @CurrentDeliveryDate, 101),
					DeliveryDate = @CurrentDeliveryDate,
					--ItemName = ItemName + ' ' + CONVERT(nvarchar(10), @NewOrderDeliveryDate, 101),
					--DeliveryDate = @NewOrderDeliveryDate

					--Re-Pricing Logic
					ItemPrice = Round(@PricePerDay * Quantity, 2),
					DiscountAdjPrice = Round(@PricePerDay * Quantity, 2) - DiscountPerEach,
					TaxableAmount = Round(@PricePerDay * Quantity, 2) - DiscountPerEach
					--End Re-Pricing Logic
				WHERE 
					CartItemID =  @CursorCartItemID;
						 
				FETCH NEXT FROM hccCartItems_cursor INTO @CursorCartItemID, @OldId
			END   

			CLOSE hccCartItems_cursor   
			DEALLOCATE hccCartItems_cursor
			*/



