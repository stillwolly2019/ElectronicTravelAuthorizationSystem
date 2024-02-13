 CREATE PROCEDURE [Rpt].[TravelAuthorizationFormTravelItinerary]
 

@TravelAuthorizationNumber VARCHAR(MAX)

 AS
WITH base AS (
SELECT 
      [Name] = TA.TravelersName,
      [TravelAuthorizationNumber] = t.TravelAuthorizationNumber,
      [FromDate] = t.FromLocationDate,
      [FromTime] =t.FromLocationTime,
	  [FromLoc] = Fromloc.CityDescription,
	  [ToDate] = t.ToLocationDate,
	  [ToTime] = t.ToLocationTime,
	  [ToLoc] = Toloc.CityDescription,
	  Ordering = t.Ordering
  FROM 
       TA.TravelAuthorization TA INNER JOIN 
       TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber LEFT JOIN 
	   [Lookups].[Lkp].[City] Fromloc ON FromLoc.CityID = t.FromLocationCode LEFT JOIN 
	   [Lookups].[Lkp].[City] Toloc ON ToLoc.CityID = t.ToLocationCode
  WHERE t.isDeleted=0
	   ) ,
rnk AS (select *,RANK() over (partition by TravelAuthorizationNumber ORDER BY Ordering) as rnk from base)

  
 
 SELECT 
 [Rank] = rnk,
 [Name] = Name,
 [TravelAuthorizationNumber] = TravelAuthorizationNumber,
 -------------------------Departure First Day
 [First-FromDate] = FromDate,
 [First-FromTime] =FromTime,
 [First-FromLoc] = FromLoc,
 -------------------------Arrival First Day
 [First-ToDate] = ToDate,
 [First-ToTime] = ToTime,
 [First-ToLoc] = ToLoc,

 [RankFilter] = CASE WHEN rnk%2=1 THEN 'Odd' ELSE 'Even' END
 


  FROM rnk  
  
  WHERE @TravelAuthorizationNumber = TravelAuthorizationNumber

  ORDER BY rnk




