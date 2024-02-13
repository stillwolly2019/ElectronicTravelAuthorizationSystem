CREATE PROCEDURE [Rpt].[TravelExpenseClaim]


@TravelAuthorizationNumber VARCHAR(MAX)

AS

WITH DeparturesAndArrivals AS (
SELECT 
      [Status] = 'Dep.',
      [Date] = FromLocationDate,
      [Time] = FromLocationTime,
	  [City] = Fromloc.CityDescription, 

      [Name] = U.LastName + ', ' + U.FirstName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  [Mode Of Travel] = mode.[Description],
	  [No. Of Days] = 0,
	  [No. Of Kms] = 0,
	  [DSA Rate] = 0,
	  [Amount USD] = 0,
	  [Amount JOD] = 0,
	  t.CreatedDate,
	  CONVERT(datetime,t.FromLocationDate,103) AS FromLocationDate

 	FROM TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber INNER JOIN 
		Lookups.Lkp.City Fromloc ON Fromloc.CityID = t.FromLocationCode INNER JOIN 
		Lookups.Lkp.City ToLoc ON ToLoc.CityID = t.ToLocationCode INNER JOIN 
		Lkp.Lookups mode ON mode.LookupsID = t.ModeOfTravelID LEFT OUTER JOIN 
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID LEFT OUTER JOIN
		TEC.TECItineraryDSA DSA ON DSA.TECItineraryID = TEC.TECItineraryID INNER JOIN
		Sec.Users U ON U.UserID = TA.UserID

  WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber AND
        TA.isDeleted = 0 AND
		t.isDeleted = 0

  
  UNION ALL

SELECT 
      [Status] = 'Arr.',
      [Date] = ToLocationDate,
      [Time] = ToLocationTime,  
	  [City] = Toloc.CityDescription, 

      [Name] = U.LastName + ', ' + U.FirstName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  [Mode Of Travel] = mode.[Description],
	  [No. Of Days] = DSA.NoOfDays,
	  [No. Of Kms] = TEC.NoOfKms,
	  [DSA Rate] = TEC.DSARate,
	  [Amount USD] = DSA.RateAmount,
	  [Amount JOD] = DSA.LocalAmount,
	  t.CreatedDate,
	  CONVERT(datetime,t.FromLocationDate,103) AS FromLocationDate


 	FROM TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber INNER JOIN 
		Lookups.Lkp.City Fromloc ON Fromloc.CityID = t.FromLocationCode INNER JOIN 
		Lookups.Lkp.City ToLoc ON ToLoc.CityID = t.ToLocationCode INNER JOIN 
		Lkp.Lookups mode ON mode.LookupsID = t.ModeOfTravelID LEFT OUTER JOIN 
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID LEFT OUTER JOIN
		TEC.TECItineraryDSA DSA ON DSA.TECItineraryID = TEC.TECItineraryID INNER JOIN
		Sec.Users U ON U.UserID = TA.UserID

  WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber AND
        TA.isDeleted = 0  AND
		t.isDeleted = 0 

		


)

SELECT  
      [Status],
      [Date],
      [Time],  
	  [City], 
      [Name],
      [Travel Authorization No.],
	  [Mode Of Travel],
	  [No. Of Days],
	  [No. Of Kms],
	  [DSA Rate],
	  [Amount USD],
	  [Amount JOD],
	  CreatedDate,
	  FromLocationDate,
	  [Row] = ROW_NUMBER() OVER (PARTITION BY [Status],[Date],[City] ORDER BY [Date]),
	  [Show/Hide] = CASE WHEN ROW_NUMBER() OVER (PARTITION BY [Status],[Date],[City] ORDER BY [Date]) <>1 AND  [Amount USD] = 0 THEN 'HIDE' ELSE 'SHOW' END ----Expression used in SSRS
	  
FROM DeparturesAndArrivals


ORDER BY CONVERT(datetime,FromLocationDate,103) ASC, [Status] DESC
