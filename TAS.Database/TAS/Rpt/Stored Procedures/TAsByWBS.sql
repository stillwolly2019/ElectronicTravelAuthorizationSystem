CREATE PROCEDURE [Rpt].[TAsByWBS]

@WBS VARCHAR(500)

AS


SELECT DISTINCT ta.TravelAuthorizationNumber
      ,[WBSCode]
      ,[PercentageOrAmount]
      ,[Note]
 
      ,[CreatedDate] = CONVERT(DATE,ta.[CreatedDate])
    
     
  FROM TA.WBS INNER JOIN
       TA.TravelAuthorization ta ON ta.TravelAuthorizationID = WBS.TravelAuthorizationID AND ta.isDeleted = 0
	   CROSS APPLY(
		select min(FromLocationDate) as  FromLocationDate , max(ToLocationDate) as  ToLocationDate from Ta.TravelItinerary ti
		where ti.TravelAuthorizationNumber=ta.[TravelAuthorizationNumber]  AND ti.isDeleted = 0

		group by  ti.TravelAuthorizationNumber 
		)travel
				INNER JOIN Sec.Users Employee ON Employee.UserID = ta.UserID
						CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.FromLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=ta.TravelAuthorizationNumber )
		locationfrom
		CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.ToLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=ta.TravelAuthorizationNumber )locationto


  WHERE wbs.isDeleted = 0 AND
        wbs.WBSCode = @WBS

  ORDER BY ta.TravelAuthorizationNumber
