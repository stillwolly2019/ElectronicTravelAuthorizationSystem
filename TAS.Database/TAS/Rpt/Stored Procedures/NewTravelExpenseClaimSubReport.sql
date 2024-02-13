CREATE PROCEDURE  [Rpt].[NewTravelExpenseClaimSubReport] 


@TECItineraryID nvarchar(100),
@Status nvarchar(100)

AS


WITH BASE AS (SELECT 
      
      [Travel Authorization No.] = TEC.[TravelAuthorizationNumber],

	  [No. Of Days] = DSA.NoOfDays,

	  [DSA Rate] = DSA.DSARate,
	  [Amount USD] = DSA.RateAmount,
	  [Amount JOD] = DSA.LocalAmount,
	  CONVERT(datetime,t.FromLocationDate,103) AS FromLocationDate,
	  TravelItineraryID = TEC.TravelItineraryID

	  ,[Rank] = ROW_NUMBER ( )  OVER (  PARTITION BY TEC.[TravelAuthorizationNumber],TEC.TravelItineraryID  order by CONVERT(datetime,t.FromLocationDate,103) ASC  )  

 	FROM 
        TA.TravelAuthorization TA INNER JOIN 
        TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber INNER JOIN 
		Lookups.Lkp.City Fromloc ON Fromloc.CityID = t.FromLocationCode INNER JOIN 
		Lookups.Lkp.City ToLoc ON ToLoc.CityID = t.ToLocationCode INNER JOIN 
		Lkp.Lookups mode ON mode.LookupsID = t.ModeOfTravelID LEFT OUTER JOIN
		TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID  LEFT OUTER JOIN
		TEC.TECItineraryDSA DSA ON DSA.TECItineraryID = TEC.TECItineraryID 

  WHERE 
		DSA.isDeleted = 0 AND  
		DSA.TECItineraryID = @TECItineraryID ) 


select  *,
        (SELECT MAX([rank]) from base group by [Travel Authorization No.]) [MaxRank],
		CASE WHEN [Rank] <(SELECT MAX([rank]) from base group by [Travel Authorization No.]) THEN 'Arr.' ELSE 'Dep.' END [Status]
from BASE

WHERE CASE WHEN [Rank] <(SELECT MAX([rank]) from base group by [Travel Authorization No.]) THEN 'Arr.' ELSE 'Dep.' END = @Status

ORDER BY FromLocationDate