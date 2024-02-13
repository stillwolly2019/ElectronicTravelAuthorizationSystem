-- Procedure
CREATE PROCEDURE [Rpt].[TravelExpenseClaimExpenses]


@TravelAuthorizationNumber VARCHAR(MAX)

AS


    SELECT DISTINCT
      [Name] = TA.TravelersName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  [Expenses-Details] = Ex.ExpenditureDetails,
	  [Expenses-Date] = Ex.ExpenditureDate,
      [Expenses-Curr.] = Curr.CurrencySymbol,
      [Expenses-Amount] = Ex.ExpenseAmount,
      [Expenses-Rate] = Ex.Rate,
      [Expenses-RateAmount USD] = Ex.RateAmount,
	  [Expenses-RateAmount JOD] = Ex.RateAmount*0.70900

  FROM TA.TravelAuthorization TA INNER JOIN 
       TEC.TECExpenditure Ex ON TA.TravelAuthorizationNumber = Ex.TravelAuthorizationNumber LEFT JOIN 
	   [Lookups].[Lkp].Currency Curr ON Curr.CurrencyID = Ex.CurrencyID 
	   

  WHERE Ex.isDeleted = 0 AND
        @TravelAuthorizationNumber = TA.TravelAuthorizationNumber






