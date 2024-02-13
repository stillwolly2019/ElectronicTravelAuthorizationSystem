CREATE PROCEDURE [Rpt].[TravelExpenseClaimMain]


@TravelAuthorizationNumber VARCHAR(MAX)

AS

SELECT 
      [Status] = 'Dep.',
      [Date] = FromLocationDate,
      [Time] = FromLocationTime,
	  [City] = Fromloc.CityDescription, 

      [Name] = ta.TravelersName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  [Mode Of Travel] = mode.[Description],
	
	  [No. Of Kms] = 0,
	  TravelItineraryID = TEC.TravelItineraryID,


	  t.CreatedDate,
	  CONVERT(datetime,t.FromLocationDate,103) AS FromLocationDate,
	  ISNULL(TEC.TECItineraryID,'00000000-0000-0000-0000-000000000000') AS TECItineraryID,
	  t.Ordering

 	FROM TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber INNER JOIN 
		Lookups.Lkp.City Fromloc ON Fromloc.CityID = t.FromLocationCode INNER JOIN 
		Lookups.Lkp.City ToLoc ON ToLoc.CityID = t.ToLocationCode INNER JOIN 
		Lkp.Lookups mode ON mode.LookupsID = t.ModeOfTravelID LEFT OUTER JOIN 
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID LEFT OUTER JOIN
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

      [Name] = ta.TravelersName,
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  [Mode Of Travel] = mode.[Description],

	  [No. Of Kms] = TEC.NoOfKms,
	  TravelItineraryID = TEC.TravelItineraryID,
	  t.CreatedDate,
	  CONVERT(datetime,t.FromLocationDate,103) AS FromLocationDate,
	  ISNULL(TEC.TECItineraryID,'00000000-0000-0000-0000-000000000000') AS TECItineraryID,
	  t.Ordering


 	FROM TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber INNER JOIN 
		Lookups.Lkp.City Fromloc ON Fromloc.CityID = t.FromLocationCode INNER JOIN 
		Lookups.Lkp.City ToLoc ON ToLoc.CityID = t.ToLocationCode INNER JOIN 
		Lkp.Lookups mode ON mode.LookupsID = t.ModeOfTravelID LEFT OUTER JOIN 
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID LEFT OUTER JOIN

		Sec.Users U ON U.UserID = TA.UserID

  WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber AND
        TA.isDeleted = 0  AND
		t.isDeleted = 0 





ORDER BY Ordering,[Status] desc
