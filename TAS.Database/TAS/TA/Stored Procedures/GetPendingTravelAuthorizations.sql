CREATE PROCEDURE [TA].[GetPendingTravelAuthorizations]
	@UserID nvarchar(100)	
AS

    IF OBJECT_ID('tempdb..#TravelItineraryOrder') IS NOT NULL
        DROP TABLE #TravelItineraryOrder;

    SELECT ROW_NUMBER() OVER (ORDER BY ti.Ordering) AS RowNumber,
           ti.TravelAuthorizationNumber,
           TLC.CityDescription
    INTO #TravelItineraryOrder
    FROM TA.TravelItinerary ti
        INNER JOIN TA.TravelAuthorization t
            ON t.TravelAuthorizationNumber = ti.TravelAuthorizationNumber
			INNER JOIN [Lookups].[Lkp].[City] TLC
            ON TLC.CityID = ti.ToLocationCode
        INNER JOIN [Lookups].[Lkp].[City] FLC
            ON FLC.CityID = ti.FromLocationCode
    WHERE ti.isDeleted = 0 


	  ;WITH cte AS
(
   SELECT *,
         ROW_NUMBER() OVER (PARTITION BY [TravelAuthorizationID] ORDER BY CreatedDate DESC) AS rn
   FROM [TA].[TATECWorkflowSteps]
)
--select * from cte where cte.rn =1

SELECT
		ta.[TravelAuthorizationID],
        [TravelAuthorizationNumber],
		ta.UserID as TravelersID,
        UPPER([TravelersName]) 'TravelersName',
        [PurposeOfTravel],
        [TripSchemaCode],
        [ModeOfTravelCode],
        [SecurityClearance],
        [SecurityTraining],
		CONVERT(VARCHAR, travel.FromLocationDate, 120) AS FromLocationDateSorting,
		CONVERT(VARCHAR, travel.ToLocationDate, 120) AS ToLocationDateSorting,
		TopOneFrom.TopOneFrom + '-' + REPLACE(CONCATLegs.CONCATLegs, ',', '-') AS CONCATLegs
	FROM cte twfs
	inner join ta.TravelAuthorization ta on  ta.TravelAuthorizationID= twfs.TravelAuthorizationID
	 CROSS APPLY
    (
        SELECT MIN(FromLocationDate) AS FromLocationDate,
               MAX(ToLocationDate) AS ToLocationDate
        FROM TA.TravelItinerary ti
        WHERE ti.TravelAuthorizationNumber = ta.[TravelAuthorizationNumber]
              AND ti.isDeleted = 0
        GROUP BY ti.TravelAuthorizationNumber
    ) travel
	 OUTER APPLY
    (
			SELECT DISTINCT CONCATLegs = STUFF((
            SELECT ',' + CityDescription
            FROM #TravelItineraryOrder
			WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber
            FOR XML PATH('')
            ), 1, 1, '')
		FROM #TravelItineraryOrder tio
		WHERE tio.TravelAuthorizationNumber = ta.TravelAuthorizationNumber


    ) CONCATLegs
        OUTER APPLY
    (
        SELECT TOP 1
               FLC.CityDescription AS TopOneFrom
        FROM TA.TravelItinerary ti
            INNER JOIN [Lookups].[Lkp].[City] FLC
                ON FLC.CityID = ti.FromLocationCode
        WHERE ti.TravelAuthorizationNumber = ta.TravelAuthorizationNumber
              AND ti.isDeleted = 0
        ORDER BY ti.Ordering ASC
    ) TopOneFrom
    WHERE (twfs.UserID = @UserID  OR 
	twfs.RoleID in(select RoleID from Sec.UsersRoles where UserID = @UserID) OR
	ta.UserID = @UserID)
	and twfs.rn = 1