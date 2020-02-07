/****** Object:  StoredProcedure [dbo].[hcc_AlcMenu]  ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_AlcMenu]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[hcc_AlcMenu]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_AlcMenu] 
	@DeliveryDate as DateTime,
	@MealTypeID as Int
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @AllergenList AS varchar(MAX)

    SELECT        
		g.MenuItemID, 
		g.Name, 
		g.Description, 
		g.UseCostRegular,
		g.CostRegular,
		g.UseCostSmall,
		g.CostSmall,
		g.UseCostLarge,
		g.CostLarge,
		g.UseCostChild,
		g.CostChild,
		g.CanyonRanchRecipe,
		g.VegetarianOptionAvailable,
		g.VeganOptionAvailable,
		g.GlutenFreeOptionAvailable,
		g.MealTypeID,
		ISNULL((SELECT DISTINCT  
				CAST(hccAllergens.Name + ', ' AS varchar(MAX))
			FROM            
				hccAllergens INNER JOIN
				hccIngredientAllergens ON hccAllergens.AllergenID = hccIngredientAllergens.AllergenID INNER JOIN
				hccMenuItemIngredients ON hccIngredientAllergens.IngredientID = hccMenuItemIngredients.IngredientID INNER JOIN
				hccMenuItems ON hccMenuItemIngredients.MenuItemID = hccMenuItems.MenuItemID
			WHERE        
				(hccMenuItems.MenuItemID = g.MenuItemID)
			FOR XML PATH('')), 'None')
		AS AllergensList

	FROM            
		hccMenuItems AS g INNER JOIN
		hccMenuMenuItems AS a ON g.MenuItemID = a.MenuItemID INNER JOIN
		hccMenus AS b ON a.MenuID = b.MenuID INNER JOIN
		hccProductionCalendars AS c ON b.MenuID = c.MenuID
	WHERE        
		(c.DeliveryDate = @DeliveryDate) AND (g.MealTypeID = @MealTypeID)
	ORDER BY 
		g.Name

END
GO
