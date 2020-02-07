/****** Object:  StoredProcedure [dbo].[hcc_IsCanyonRanchCustomer]    Script Date: 04/09/2013 09:24:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_IsCanyonRanchCustomer]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_IsCanyonRanchCustomer]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_IsCanyonRanchCustomer]
	@CartId as Int,
	@UserProfileId as Int

AS
BEGIN

	SET NOCOUNT ON;

	Declare @TotalCartItems int
	Declare @TotalCanyonRanchItems int

    SELECT        
		@TotalCartItems = Count(b.CartItemID)
	FROM            
		hccMenuItems AS a INNER JOIN
		hccCartItems AS b ON a.MenuItemID = b.Meal_MenuItemID
	WHERE        
		(b.CartID = @CartId and 
		b.UserProfileID = @UserProfileId);

	SELECT        
		@TotalCanyonRanchItems = Count(b.CartItemID)
	FROM            
		hccMenuItems AS a INNER JOIN
		hccCartItems AS b ON a.MenuItemID = b.Meal_MenuItemID
	WHERE        
		(b.CartID = @CartId and 
		b.UserProfileID = @UserProfileId and 
		a.CanyonRanchRecipe = 1);

	If(@TotalCartItems = @TotalCanyonRanchItems)
		BEGIN
			UPDATE 
				hccUserProfiles
			SET 
				CanyonRanchCustomer = 1
			WHERE 
				UserProfileID = @UserProfileId;
		END
	Else
		BEGIN
			UPDATE 
				hccUserProfiles
			SET 
				CanyonRanchCustomer = 0
			WHERE 
				UserProfileID = @UserProfileId;	
		END
END
GO
