CREATE PROCEDURE [Rpt].[TravelExpenseClaimAdvances]


@TravelAuthorizationNumber VARCHAR(MAX)

AS


      SELECT 
      [Name] = TA.TravelersName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  --------------------------------------------------Travel Advances
	  [Advances-Paying Office] = Adv.PayOfficeCode,
	  [Advances-DatePaid] = Adv.[DatePaid],
      [Advances-Curr.] = Curr.CurrencySymbol,
      [Advances-Amount] = Adv.AdvanceAmount,
      [Advances-Rate] = Adv.[Rate],
      [Advances-RateAmount USD] = Adv.[RateAmount],
	  [Advances-RateAmount JOD] = Adv.LocalAmount


  FROM TA.TravelAuthorization TA INNER JOIN 
       TEC.TECAdvances Adv ON TA.TravelAuthorizationNumber = Adv.TravelAuthorizationNumber LEFT JOIN 
	   [Lookups].[Lkp].Currency Curr ON Curr.CurrencyID = Adv.CurrencyID
	   

  WHERE Adv.isDeleted = 0 AND
   @TravelAuthorizationNumber =TA.TravelAuthorizationNumber


