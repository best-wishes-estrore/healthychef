SELECT        hccProgramPlans.NumDaysPerWeek, hccProgramPlans.NumWeeks, hccProgramPlans.PricePerDay, hccProgramPlans.IsTaxEligible, 
                         hccProgramOptions.OptionValue
FROM            hccCartItems INNER JOIN
                         hccRecurringOrder ON hccCartItems.CartID = hccRecurringOrder.CartID INNER JOIN
                         hccProgramPlans ON hccCartItems.Plan_PlanID = hccProgramPlans.PlanID INNER JOIN
                         hccProgramOptions ON hccCartItems.Plan_ProgramOptionID = hccProgramOptions.ProgramOptionID