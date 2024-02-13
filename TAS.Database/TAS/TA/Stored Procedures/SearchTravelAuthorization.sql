CREATE PROCEDURE [TA].[SearchTravelAuthorization]
    @TravelAuthorizationNumber NVARCHAR(14),
    @WBS NVARCHAR(MAX),
    @Status NVARCHAR(50),
    @location NVARCHAR(50)
AS
BEGIN


    IF OBJECT_ID('tempdb..#TravelItineraryOrder') IS NOT NULL
        DROP TABLE #TravelItineraryOrder;

    SELECT ROW_NUMBER() OVER (ORDER BY ti.Ordering) AS RowNumber,
           ti.TravelAuthorizationNumber,
           TLC.CityDescription
    INTO #TravelItineraryOrder
    FROM TA.TravelItinerary ti
        INNER JOIN TA.TravelAuthorization t
            ON t.TravelAuthorizationNumber = ti.TravelAuthorizationNumber
        CROSS APPLY
    (
        SELECT WBSCode
        FROM TA.WBS
        WHERE TravelAuthorizationID = t.TravelAuthorizationID
    ) wbs
        OUTER APPLY
    (
        SELECT TOP 1
               s.Description,
               StatusCode
        FROM TA.StatusChangeHistory
            INNER JOIN Lkp.Lookups s
                ON s.Code = TA.StatusChangeHistory.StatusCode
        WHERE TravelAuthorizationID = t.[TravelAuthorizationID]
        ORDER BY CreatedDate DESC
    ) status
        INNER JOIN [Lookups].[Lkp].[City] TLC
            ON TLC.CityID = ti.ToLocationCode
        INNER JOIN [Lookups].[Lkp].[City] FLC
            ON FLC.CityID = ti.FromLocationCode
    WHERE ti.isDeleted = 0
          AND t.isDeleted = 0
          AND
          (
              t.[TravelAuthorizationNumber] LIKE '%' + @TravelAuthorizationNumber + '%'
              OR @TravelAuthorizationNumber = ''
          )
          AND
          (
              wbs.WBSCode LIKE '%' + @WBS + '%'
              OR @WBS = ''
          )
          AND
          (
              status.StatusCode = @Status
              OR @Status = ''
          )
          AND
          (
              FLC.CityDescription LIKE '%' + @location + '%'
              OR @location = ''
              OR TLC.CityDescription LIKE '%' + @location + '%'
              OR @location = ''
          );



    SELECT DISTINCT
           [TravelAuthorizationID],
           [TravelAuthorizationNumber],
           UPPER(Employee.[FirstName]) + ' ' + UPPER(Employee.[LastName]) AS CreatedByName,
           UPPER([TravelersName]) AS [TravelersName],
           [PurposeOfTravel],
           [TripSchemaCode],
           [ModeOfTravelCode],
           [SecurityClearance],
           [SecurityTraining],
           status.Description AS [StatusCode],
           [CreatedDate],
           t.[CreatedBy],
           [UpdatedDate],
           [UpdatedBy],
           t.[isDeleted],
           UPPER(Employee.[FirstName]) + ' ' + UPPER(Employee.[LastName]) AS UpdatedByName,
           travel.FromLocationDate,
           travel.ToLocationDate,
           CONVERT(VARCHAR, travel.FromLocationDate, 120) AS FromLocationDateSorting,
           CONVERT(VARCHAR, travel.ToLocationDate, 120) AS ToLocationDateSorting,
           TopOneFrom.TopOneFrom + '-' + REPLACE(CONCATLegs.CONCATLegs, ',', '-') AS CONCATLegs
    FROM [TA].[TravelAuthorization] t
        CROSS APPLY
    (
        SELECT TOP 1
               s.Description,
               StatusCode
        FROM TA.StatusChangeHistory
            INNER JOIN Lkp.Lookups s
                ON s.Code = TA.StatusChangeHistory.StatusCode
        WHERE TravelAuthorizationID = t.[TravelAuthorizationID]
        ORDER BY CreatedDate DESC
    ) status
        CROSS APPLY
    (
        SELECT WBSCode
        FROM TA.WBS
        WHERE TravelAuthorizationID = t.TravelAuthorizationID
    ) wbs
        CROSS APPLY
    (
        SELECT MIN(FromLocationDate) AS FromLocationDate,
               MAX(ToLocationDate) AS ToLocationDate
        FROM TA.TravelItinerary ti
        WHERE ti.TravelAuthorizationNumber = t.[TravelAuthorizationNumber]
              AND ti.isDeleted = 0
        GROUP BY ti.TravelAuthorizationNumber
    ) travel
        INNER JOIN Sec.Users Employee
            ON Employee.UserID = t.UserID
        CROSS APPLY
    (
        SELECT CityDescription
        FROM Lookups.Lkp.City c
            INNER JOIN TA.TravelItinerary ti
                ON ti.FromLocationCode = c.CityID
        WHERE ti.TravelAuthorizationNumber = t.TravelAuthorizationNumber
    ) locationfrom
        CROSS APPLY
    (
        SELECT CityDescription
        FROM Lookups.Lkp.City c
            INNER JOIN TA.TravelItinerary ti
                ON ti.ToLocationCode = c.CityID
        WHERE ti.TravelAuthorizationNumber = t.TravelAuthorizationNumber
    ) locationto
        OUTER APPLY
    (
        --SELECT dbo.GROUP_CONCAT(tio.CityDescription) AS CONCATLegs
        --FROM #TravelItineraryOrder tio
        --WHERE tio.TravelAuthorizationNumber = t.TravelAuthorizationNumber
			  SELECT DISTINCT CONCATLegs = STUFF((
            SELECT ',' + CityDescription
            FROM #TravelItineraryOrder
			WHERE TravelAuthorizationNumber = t.TravelAuthorizationNumber
            FOR XML PATH('')
            ), 1, 1, '')
FROM #TravelItineraryOrder tio
WHERE tio.TravelAuthorizationNumber = t.TravelAuthorizationNumber


    ) CONCATLegs
        OUTER APPLY
    (
        SELECT TOP 1
               FLC.CityDescription AS TopOneFrom
        FROM TA.TravelItinerary ti
            INNER JOIN [Lookups].[Lkp].[City] FLC
                ON FLC.CityID = ti.FromLocationCode
        WHERE ti.TravelAuthorizationNumber = t.TravelAuthorizationNumber
              AND ti.isDeleted = 0
        ORDER BY ti.Ordering ASC
    ) TopOneFrom
    WHERE t.isDeleted = 0
          AND
          (
              t.[TravelAuthorizationNumber] LIKE '%' + @TravelAuthorizationNumber + '%'
              OR @TravelAuthorizationNumber = ''
          )
          AND
          (
              wbs.WBSCode LIKE '%' + @WBS + '%'
              OR @WBS = ''
          )
          AND
          (
              status.StatusCode = @Status
              OR @Status = ''
          )
          AND
          (
              locationfrom.CityDescription LIKE '%' + @location + '%'
              OR @location = ''
              OR locationto.CityDescription LIKE '%' + @location + '%'
              OR @location = ''
          )
    ORDER BY [CreatedDate] DESC;
END;