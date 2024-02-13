CREATE PROCEDURE [Rpt].[TravelAuthorizationFormTravelMode]

@TravelAuthorizationNumber VARCHAR(MAX)

AS

SELECT DISTINCT
      [Name] = TA.TravelersName,


	  [Air] = CASE WHEN mode.[Description] = 'Air' THEN 20 ELSE  40 END,
	  [Ferry] = CASE WHEN mode.[Description] = 'Ferry' THEN 20 ELSE  40 END,
	  [Ship] = CASE WHEN mode.[Description] = 'Ship' THEN 20 ELSE  40 END,
	  [Bus/Train] = CASE WHEN mode.[Description] LIKE '%Train%' OR mode.[Description] LIKE '%Bus%' THEN 20 ELSE  40 END,
	  [Car] = CASE WHEN mode.[Description] = 'Car' THEN 20 ELSE  40 END

	                
	               
				 
   FROM
       TA.TravelAuthorization TA INNER JOIN 
       TA.WBS wbs ON wbs.TravelAuthorizationID = TA.[TravelAuthorizationID] INNER JOIN 
       TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber LEFT JOIN 
	   Lkp.Lookups mode ON mode.LookupsID = t.ModeOfTravelID LEFT JOIN
	   TEC.TECItinerary TEC ON TEC.TravelItineraryID = t.TravelItineraryID LEFT JOIN 
       TEC.TECExpenditure Ex ON TA.TravelAuthorizationNumber = Ex.TravelAuthorizationNumber LEFT JOIN 
       TEC.TECAdvances Adv ON TA.TravelAuthorizationNumber = Adv.TravelAuthorizationNumber

    WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber