IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hcc_MenuItemsByDateRange]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[hcc_MenuItemsByDateRange]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[hcc_MenuItemsByDateRange]
    @start DATETIME,
    @end DATETIME,
	@includeAlaCarte bit = 1
AS 
    BEGIN
        SET NOCOUNT ON  
--alacarte
	IF (@includeAlaCarte = 1)
	BEGIN
        DECLARE @ALCItemsForDate TABLE
            (
              OrderNumber VARCHAR(10) ,
              ItemName VARCHAR(50) ,
              MealTypeName VARCHAR(50) ,
              MealSizeName VARCHAR(50) ,
              Quantity INT ,
              DeliveryDate DATETIME ,
              ParentTypeId INT ,
              CartItemId INT ,
              MealTypeId INT ,
              MenuItemId INT ,
              MealSizeId INT ,
              Prefs VARCHAR(MAX) ,
              UserProfileId INT
            )  

        INSERT  INTO @ALCItemsForDate
                SELECT  ci.OrderNumber ,
                        mi.Name ,
                        CASE mi.MealTypeID
                          WHEN 0 THEN 'Unknown'
                          WHEN 10 THEN 'Breakfast Entree'
                          WHEN 20 THEN 'Breakfast Side'
                          WHEN 30 THEN 'Lunch Entree'
                          WHEN 40 THEN 'Lunch Side'
                          WHEN 50 THEN 'Dinner Entree'
                          WHEN 60 THEN 'Dinner Side'
                          WHEN 70 THEN 'Other Entree'
                          WHEN 80 THEN 'Other Side'
                          WHEN 90 THEN 'Child Entree'
                          WHEN 100 THEN 'Child Side'
                          WHEN 110 THEN 'Salad'
                          WHEN 120 THEN 'Soup'
                          WHEN 130 THEN 'Dessert'
                          WHEN 140 THEN 'Beverage'
                          WHEN 150 THEN 'Snack'
                          WHEN 160 THEN 'Supplement'
                          WHEN 170 THEN 'Goods'
                          WHEN 180 THEN 'Miscellaneous'
                        END ,
                        CASE ci.Meal_MealSizeID
                          WHEN 0 THEN 'No Size'
                          WHEN 1 THEN 'Child Size'
                          WHEN 2 THEN 'Small'
                          WHEN 3 THEN 'Regular'
                          WHEN 4 THEN 'Large'
                        END ,
                        ci.Quantity ,
                        ci.DeliveryDate ,
                        1 ,
                        ci.CartItemID ,
                        mi.MealTypeID ,
                        mi.MenuItemID ,
                        ci.Meal_MealSizeID ,
                        '' ,
                        ci.UserProfileID
                FROM    dbo.hccCartItems ci
                        INNER JOIN dbo.hccCarts c ON c.CartID = ci.CartID
                        INNER JOIN dbo.hccMenuItems mi ON mi.MenuItemID = ci.Meal_MenuItemID
                WHERE   c.StatusID = 20
                        AND ci.ItemTypeID = 1
                        AND ci.IsCancelled = 0
                        AND DeliveryDate BETWEEN @start AND @end
                ORDER BY mi.MealTypeID ,
                        mi.Name ,
                        ci.Meal_MealSizeID
      
 --SELECT *
 --FROM   @ALCItemsForDate;

 -- add prefs to alc meal items
        DECLARE @cartItemId INT ,
            @alcRowNum INT ,
            @alcMaxRows INT ,
            @alcPrefs VARCHAR(MAX);
      
        SELECT TOP 1
                @cartItemId = CartItemId
        FROM    @ALCItemsForDate
        ORDER BY CartItemId;

        SELECT  @alcMaxRows = COUNT(*)
        FROM    @ALCItemsForDate;
      
        SET @alcRowNum = 0

        WHILE @alcRowNum < @alcMaxRows 
            BEGIN
                SET @alcRowNum = @alcRowNum + 1     
		--PRINT ( 'cartItemId: ' + CONVERT(VARCHAR, @cartItemId) )
                SET @alcPrefs = '';

                SELECT  @alcPrefs = COALESCE(@alcPrefs + ', ', '')
                        + c.[Description]
                FROM    @ALCItemsForDate a
                        JOIN hccCartItemMealPreferences b ON ( a.CartItemID = b.CartItemID )
                        JOIN hccPreferences c ON ( b.PreferenceID = c.PreferenceID )
                WHERE   a.CartItemID = @CartItemID

                IF LEN(@alcPrefs) > 0 
                    BEGIN
                    -- remove leading whitespace and comma  
                        SET @alcPrefs = RTRIM(LTRIM(@alcPrefs));
                        IF ASCII(LEFT(@alcPrefs, 1)) = 44 
                            SET @alcPrefs = LTRIM(SUBSTRING(@alcPrefs, 2,
                                                            LEN(@alcPrefs) - 1));
								     
                        IF LEN(@alcPrefs) > 0 
                            BEGIN         
                                UPDATE  @ALCItemsForDate
                                SET     Prefs = @alcPrefs
                                WHERE   CartItemID = @CartItemID
                            END
                    END
        
                SELECT TOP 1
                        @cartItemId = CartItemId
                FROM    @ALCItemsForDate
                WHERE   CartItemId > @cartItemId
                ORDER BY CartItemId;        
            END

 --SELECT *
 --FROM   @ALCItemsForDate;
--/alacarte    
	END


-- Programs
      
        DECLARE @PlanDefExItemsForDate TABLE
            (
              OrderNumber VARCHAR(10) ,
              ItemName VARCHAR(50) ,
              MealTypeName VARCHAR(50) ,
              MealSizeName VARCHAR(50) ,
              Quantity INT ,
              DeliveryDate DATETIME ,
              DayNum INT ,
              ParentTypeId INT ,
              DefMenuExId INT ,
              DefMenuId INT ,
              CartCalendarId INT ,
              MealTypeId INT ,
              MenuItemId INT ,
              MealSizeId INT ,
              Prefs VARCHAR(MAX) ,
              CartItemId INT ,
              PlanId INT ,
              PlanName VARCHAR(250) ,
              UserProfileId INT ,
              RowId INT IDENTITY(1, 1)
            )    

-- programs exceptions
        INSERT  INTO @PlanDefExItemsForDate
                SELECT  ci.OrderNumber ,
                        mi.Name ,
                        CASE mi.MealTypeID
                          WHEN 0 THEN 'Unknown'
                          WHEN 10 THEN 'Breakfast Entree'
                          WHEN 20 THEN 'Breakfast Side'
                          WHEN 30 THEN 'Lunch Entree'
                          WHEN 40 THEN 'Lunch Side'
                          WHEN 50 THEN 'Dinner Entree'
                          WHEN 60 THEN 'Dinner Side'
                          WHEN 70 THEN 'Other Entree'
                          WHEN 80 THEN 'Other Side'
                          WHEN 90 THEN 'Child Entree'
                          WHEN 100 THEN 'Child Side'
                          WHEN 110 THEN 'Salad'
                          WHEN 120 THEN 'Soup'
                          WHEN 130 THEN 'Dessert'
                          WHEN 140 THEN 'Beverage'
                          WHEN 150 THEN 'Snack'
                          WHEN 160 THEN 'Supplement'
                          WHEN 170 THEN 'Goods'
                          WHEN 180 THEN 'Miscellaneous'
                        END ,
                        CASE dX.MenuItemSizeID
                          WHEN 0 THEN 'No Size'
                          WHEN 1 THEN 'Child Size'
                          WHEN 2 THEN 'Small'
                          WHEN 3 THEN 'Regular'
                          WHEN 4 THEN 'Large'
                        END ,
                        ci.Quantity ,
                        pc.DeliveryDate ,
                        dm.DayNumber ,
                        2 ,
                        dX.DefaultMenuExceptID ,
                        dX.DefaultMenuID ,
                        dx.CartCalendarID ,
                        mi.MealTypeID ,
                        mi.MenuItemID ,
                        dX.MenuItemSizeID ,
                        '' ,
                        ci.CartItemID ,
                        pp.PlanID ,
                        pp.Name ,
                        ci.UserProfileID
                FROM    dbo.hccCartItemCalendars cic
                        LEFT JOIN dbo.hccProductionCalendars pc ON pc.CalendarID = cic.CalendarID
                        LEFT JOIN dbo.hccCartDefaultMenuExceptions dX ON dX.CartCalendarID = cic.CartCalendarID
                        LEFT JOIN dbo.hccCartItems ci ON ci.CartItemID = cic.CartItemID
                        LEFT JOIN dbo.hccCarts c ON c.CartID = ci.CartID
                        LEFT JOIN dbo.hccProgramPlans pp ON pp.PlanID = ci.Plan_PlanID
                        LEFT JOIN dbo.hccProgramDefaultMenus dm ON dm.DefaultMenuID = dx.DefaultMenuID
                        LEFT JOIN dbo.hccMenuItems mi ON mi.MenuItemID = dX.MenuItemID
                WHERE   pc.DeliveryDate BETWEEN @start AND @end
                        AND c.StatusID = 20
                        AND ci.ItemTypeID = 2
                        AND ci.IsCancelled = 0
                ORDER BY mi.MealTypeID ,
                        mi.Name 
               

 --SELECT *
 --FROM   @PlanDefExItemsForDate ORDER BY ordernumber;

-- add prefs to alc defMenuEx items 
        DECLARE @DefMenuExId INT ,
            @dfxRowNum INT ,
            @dfxMaxRows INT ,
            @dfxPrefs VARCHAR(MAX);
      
        SELECT TOP 1
                @DefMenuExId = DefMenuExId
        FROM    @PlanDefExItemsForDate
        WHERE   DefMenuExId IS NOT NULL
        ORDER BY DefMenuExId;

        SELECT  @dfxMaxRows = COUNT(*)
        FROM    @PlanDefExItemsForDate;
      
        SET @dfxRowNum = 0

        WHILE @dfxRowNum < @dfxMaxRows 
            BEGIN
                SET @dfxRowNum = @dfxRowNum + 1     
		--PRINT ( 'cartItemId: ' + CONVERT(VARCHAR, @cartItemId) )
                SET @dfxPrefs = '';

                SELECT  @dfxPrefs = COALESCE(@dfxPrefs + ', ', '') + c.Name
                FROM    @PlanDefExItemsForDate a
                        JOIN dbo.hccCartDefaultMenuExPrefs b ON ( a.DefMenuExId = b.DefaultMenuExceptID )
                        JOIN hccPreferences c ON ( b.PreferenceID = c.PreferenceID )
                WHERE   a.DefMenuExId = @DefMenuExId

                IF LEN(@dfxPrefs) > 0 
                    BEGIN
					-- remove leading whitespace and comma  					           
                        SET @dfxPrefs = RTRIM(LTRIM(@dfxPrefs));					
						       
                        IF ASCII(LEFT(@dfxPrefs, 1)) = 44 
                            SET @dfxPrefs = LTRIM(SUBSTRING(@dfxPrefs, 2,
                                                            LEN(@dfxPrefs) - 1));
					     
                        IF LEN(@dfxPrefs) > 0 
                            BEGIN 
                                UPDATE  @PlanDefExItemsForDate
                                SET     Prefs = @dfxPrefs
                                WHERE   DefMenuExId = @DefMenuExId
                            END  
                    END
        
        -- now we grab the next row making sure the ID of the next row
        -- is greater than previous row		
                SELECT TOP 1
                        @DefMenuExId = DefMenuExId
                FROM    @PlanDefExItemsForDate
                WHERE   DefMenuExId IS NOT NULL
                        AND DefMenuExId > @DefMenuExId
                ORDER BY DefMenuExId;
	    
            END

 --SELECT *
 --FROM   @PlanDefExItemsForDate
 --ORDER BY ordernumber ,
 --       DayNum ,
 --       MealTypeId;
 
        DECLARE @DefaultMenuItems TABLE
            (
              DefMenuId INT ,
              ProgramId INT ,
              CalendarId INT ,
              MenuItemId INT ,
              MealSizeId INT ,
              MealTypeId INT ,
              DayNum INT ,
              Ordinal INT
            )	

        INSERT  INTO @DefaultMenuItems
                SELECT  dm.*
                FROM    dbo.hccProgramDefaultMenus dm
                        INNER JOIN hccProductionCalendars pc ON pc.CalendarID = dm.CalendarID
                WHERE   pc.DeliveryDate BETWEEN @start AND @end;

--SELECT  *
--FROM    @DefaultMenuItems;

        DECLARE @Orders TABLE
            (
              CartCalendarId INT ,
              CalendarId INT ,
              ProgramId INT ,
              PlanId INT ,
              CartId INT ,
              CartItemId INT ,
              MenuId INT ,
              Quantity INT ,
              OrderNumber VARCHAR(50) ,
              DayPerWeek INT ,
              UserProfileId INT ,
              DeliveryDate DATETIME ,
              PlanName VARCHAR(250)
            )

        INSERT  INTO @Orders
                SELECT  cic.CartCalendarID ,
                        cic.CalendarID ,
                        pp.ProgramID ,
                        pp.PlanID ,
                        ci.CartID ,
                        cic.CartItemID ,
                        pc.MenuID ,
                        ci.Quantity ,
                        ci.OrderNumber ,
                        pp.NumDaysPerWeek ,
                        ci.UserProfileID ,
                        pc.DeliveryDate ,
                        pp.Name
                FROM    dbo.hccCartItemCalendars cic
                        LEFT JOIN dbo.hccProductionCalendars pc ON pc.CalendarID = cic.CalendarID
                        LEFT JOIN dbo.hccCartItems ci ON ci.CartItemID = cic.CartItemID
                        LEFT JOIN dbo.hccCarts c ON c.CartID = ci.CartID
                        LEFT JOIN dbo.hccProgramPlans pp ON pp.PlanID = ci.Plan_PlanID
                WHERE   pc.DeliveryDate BETWEEN @start AND @end
                        AND c.StatusID = 20
                        AND ci.ItemTypeID = 2
                        AND ci.IsCancelled = 0;

--SELECT  *
--FROM    @Orders ORDER BY CartCalendarId;

        DECLARE @PlanDefItemsForDate TABLE
            (
              OrderNumber VARCHAR(10) ,
              ItemName VARCHAR(50) ,
              MealTypeName VARCHAR(50) ,
              MealSizeName VARCHAR(50) ,
              Quantity INT ,
              DeliveryDate DATETIME ,
              DayNum INT ,
              ParentTypeId INT ,
              DefMenuId INT ,
              MealTypeId INT ,
              MenuItemId INT ,
              MealSizeId INT ,
              Prefs VARCHAR(MAX) ,
              CartItemId INT ,
              PlanId INT ,
              PlanName VARCHAR(250) ,
              UserProfileId INT
            )

        DECLARE @cartcalId INT ,
            @calid INT ,
            @progid INT ,
            @rowNum INT ,
            @maxrows INT;
      
        SELECT TOP 1
                @cartcalId = CartCalendarId ,
                @calid = CalendarId ,
                @progid = ProgramId
        FROM    @Orders
        ORDER BY CartCalendarId;

        SELECT  @maxRows = COUNT(*)
        FROM    @Orders;
      
        SET @rowNum = 0

        WHILE @rowNum < @maxRows 
            BEGIN
                SET @rowNum = @rowNum + 1     
        --PRINT ( 'cartcal: ' + CONVERT(VARCHAR, @cartcalId) )

                INSERT  INTO @PlanDefItemsForDate
                        SELECT  o.OrderNumber ,
                                mi.Name ,
                                CASE mi.MealTypeID
                                  WHEN 0 THEN 'Unknown'
                                  WHEN 10 THEN 'Breakfast Entree'
                                  WHEN 20 THEN 'Breakfast Side'
                                  WHEN 30 THEN 'Lunch Entree'
                                  WHEN 40 THEN 'Lunch Side'
                                  WHEN 50 THEN 'Dinner Entree'
                                  WHEN 60 THEN 'Dinner Side'
                                  WHEN 70 THEN 'Other Entree'
                                  WHEN 80 THEN 'Other Side'
                                  WHEN 90 THEN 'Child Entree'
                                  WHEN 100 THEN 'Child Side'
                                  WHEN 110 THEN 'Salad'
                                  WHEN 120 THEN 'Soup'
                                  WHEN 130 THEN 'Dessert'
                                  WHEN 140 THEN 'Beverage'
                                  WHEN 150 THEN 'Snack'
                                  WHEN 160 THEN 'Supplement'
                                  WHEN 170 THEN 'Goods'
                                  WHEN 180 THEN 'Miscellaneous'
                                END ,
                                CASE d.MealSizeId
                                  WHEN 0 THEN 'No Size'
                                  WHEN 1 THEN 'Child Size'
                                  WHEN 2 THEN 'Small'
                                  WHEN 3 THEN 'Regular'
                                  WHEN 4 THEN 'Large'
                                END ,
                                o.Quantity ,
                                o.DeliveryDate ,
                                d.DayNum ,
                                3 ,
                                d.DefMenuId ,
                                d.MealTypeId ,
                                d.MenuItemId ,
                                d.MealSizeId ,
                                '' ,
                                o.CartItemId ,
                                o.PlanId ,
                                o.PlanName ,
                                o.UserProfileId
                        FROM    @DefaultMenuItems d
                                JOIN @Orders o ON o.CalendarId = d.CalendarId
                                                  AND o.ProgramId = d.ProgramId
                                JOIN dbo.hccMenuItems mi ON mi.MenuItemID = d.MenuItemId
                        WHERE   d.ProgramId = @progid
                                AND d.CalendarId = @calid
                                AND o.CartCalendarId = @cartcalId
                                AND d.DayNum <= o.DayPerWeek
                                AND d.DefMenuId NOT IN (
                                SELECT  DefMenuId
                                FROM    @PlanDefExItemsForDate
                                WHERE   CartCalendarId = @cartcalId )
        
        -- now we grab the next row making sure the ID of the next row
        -- is greater than previous row		
                SELECT TOP 1
                        @cartcalId = CartCalendarId ,
                        @calid = CalendarId ,
                        @progid = ProgramId
                FROM    @Orders
                WHERE   CartCalendarId > @cartcalId
                ORDER BY CartCalendarId;
            END

--SELECT  *
--FROM    @PlanDefItemsForDate
--ORDER BY OrderNumber ,
--        DayNum ,
--        MealTypeId;


        DECLARE @ProgramItemsForDate TABLE
            (
              OrderNumber VARCHAR(10) ,
              ItemName VARCHAR(50) ,
              MealTypeName VARCHAR(50) ,
              MealSizeName VARCHAR(50) ,
              Quantity INT ,
              DeliveryDate DATETIME ,
              DayNum INT ,
              ParentType VARCHAR(50) ,
              ParentTypeId INT ,
              ParentId INT ,
              MealTypeId INT ,
              MenuItemId INT ,
              MealSizeId INT ,
              Prefs VARCHAR(MAX) ,
              CartItemId INT ,
              PlanId INT ,
              PlanName VARCHAR(250) ,
              UserProfileId INT
            )
	
        INSERT  INTO @ProgramItemsForDate
                SELECT  OrderNumber ,
                        ItemName ,
                        MealTypeName ,
                        MealSizeName ,
                        Quantity ,
                        DeliveryDate ,
                        DayNum ,
                        'ProgramException' ,
                        ParentTypeId ,
                        DefMenuExId ,
                        MealTypeId ,
                        MenuItemId ,
                        MealSizeId ,
                        Prefs ,
                        CartItemId ,
                        PlanId ,
                        PlanName ,
                        UserProfileId
                FROM    @PlanDefExItemsForDate      

        INSERT  INTO @ProgramItemsForDate
                SELECT  OrderNumber ,
                        ItemName ,
                        MealTypeName ,
                        MealSizeName ,
                        Quantity ,
                        DeliveryDate ,
                        DayNum ,
                        'ProgramDefault' ,
                        ParentTypeId ,
                        DefMenuId ,
                        MealTypeId ,
                        MenuItemId ,
                        MealSizeId ,
                        Prefs ,
                        CartItemId ,
                        PlanId ,
                        PlanName ,
                        UserProfileId
                FROM    @PlanDefItemsForDate  

--SELECT  * FROM @ProgramItemsForDate ORDER BY OrderNumber, DayNum, MealSizeName;
----end programs

        DECLARE @FinalItemsForDate TABLE
            (
              OrderNumber VARCHAR(10) ,
              ItemName VARCHAR(50) ,
              MealTypeName VARCHAR(50) ,
              MealSizeName VARCHAR(50) ,
              Quantity INT ,
              DayNum INT ,
              Prefs VARCHAR(MAX) ,
              ParentType VARCHAR(50) ,
              ParentTypeId INT ,
              ParentId INT ,
              MealTypeId INT ,
              MenuItemId INT ,
              MealSizeId INT ,
              DeliveryDate DATETIME ,
              CartItemId INT ,
              PlanId INT ,
              PlanName VARCHAR(250) ,
              UserProfileId INT ,
              RowId INT IDENTITY(1, 1)
                        PRIMARY KEY
            )

	IF (@includeAlaCarte = 1)
	BEGIN
-- insert alc items to final list
        INSERT  INTO @FinalItemsForDate
                SELECT  OrderNumber ,
                        ItemName ,
                        MealTypeName ,
                        MealSizeName ,
                        Quantity ,
                        0 ,
                        Prefs ,
                        'A la Carte' ,
                        ParentTypeId ,
                        CartItemId ,
                        MealTypeId ,
                        MenuItemId ,
                        MealSizeId ,
                        DeliveryDate ,
                        CartItemId ,
                        0 ,
                        '' ,
                        UserProfileId
                FROM    @ALCItemsForDate c;
	END
-- insert program items to final list
        INSERT  INTO @FinalItemsForDate
                SELECT  OrderNumber ,
                        ItemName ,
                        MealTypeName ,
                        MealSizeName ,
                        Quantity ,
                        DayNum ,
                        Prefs ,
                        ParentType ,
                        ParentTypeId ,
                        ParentId ,
                        MealTypeId ,
                        MenuItemId ,
                        MealSizeId ,
                        DeliveryDate ,
                        CartItemId ,
                        PlanId ,
                        PlanName ,
                        UserProfileId
                FROM    @ProgramItemsForDate;

        DELETE  FROM @FinalItemsForDate
        WHERE   ItemName IS NULL;

-------End data compilation


----select by order num
        SELECT  OrderNumber ,
                ItemName ,
                MealTypeName ,
                MealSizeName ,
                Quantity ,
                DayNum ,
                Prefs ,
                ParentType ,
                ParentTypeId ,
                ParentId ,
                MealTypeId ,
                MenuItemId ,
                MealSizeId ,
                DeliveryDate ,
                CartItemId ,
                PlanId ,
                PlanName ,
                UserProfileId ,
                RowId
        FROM    @FinalItemsForDate
        ORDER BY OrderNumber ,
                CartItemId ,
                DayNum ,
                MealTypeId


 --by mealtype
 --SELECT *
 --FROM   @FinalItemsForDate
 --ORDER BY RowId
 --ORDER BY MealTypeName ,
 --       ItemName ,
 --       MealSizeName ,
 --       Prefs 

 --SELECT ItemName, COUNT(ItemName) AS Cnt, Quantity
	--FROM @FinalItemsForDate

    END
GO