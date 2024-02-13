CREATE PROCEDURE  [Rpt].[TravelExpenseClaimTotals] 


@TravelAuthorizationNumber VARCHAR(MAX)
AS


SELECT 
	  [No. Of Days] = SUM(DSA.NoOfDays),

	
	  [Amount USD] = SUM(DSA.RateAmount),
	  [Amount JOD] = SUM(DSA.LocalAmount)


 	FROM TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber AND t.isDeleted = 0 INNER JOIN 
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID AND TEC.isDeleted = 0 LEFT OUTER JOIN
		TEC.TECItineraryDSA DSA ON DSA.TECItineraryID = TEC.TECItineraryID AND dsa.isDeleted = 0 

  WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber AND
		DSA.isDeleted = 0

  
  

