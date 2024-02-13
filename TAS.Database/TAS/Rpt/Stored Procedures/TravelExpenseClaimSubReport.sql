CREATE PROCEDURE  [Rpt].[TravelExpenseClaimSubReport] 


@TECItineraryID nvarchar(100)

AS


SELECT 
       

      [Name] = U.LastName + ', ' + U.FirstName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],

	  [No. Of Days] = DSA.NoOfDays,
	  [Percentage] = Dsa.[Percentage],
	  [DSA Rate] = DSA.DSARate,
	  [Amount USD] = DSA.RateAmount,
	  [Amount JOD] = DSA.LocalAmount,
	  CONVERT(datetime,t.FromLocationDate,103) AS FromLocationDate,
	  TravelItineraryID = TEC.TravelItineraryID

 	FROM TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber AND t.isDeleted = 0  INNER JOIN 
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID AND TEC.isDeleted = 0  LEFT OUTER JOIN
		TEC.TECItineraryDSA DSA ON DSA.TECItineraryID = TEC.TECItineraryID AND dsa.isDeleted = 0  INNER JOIN
		Sec.Users U ON U.UserID = TA.UserID

  WHERE DSA.TECItineraryID = @TECItineraryID AND
		DSA.isDeleted = 0

  
  

ORDER BY CONVERT(datetime,FromLocationDate,103) ASC

