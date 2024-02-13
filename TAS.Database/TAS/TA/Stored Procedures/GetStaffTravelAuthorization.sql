CREATE PROCEDURE [TA].[GetStaffTravelAuthorization]
    @UserID NVARCHAR(100),
    @TravelAuthorizationNumber NVARCHAR(14),
    @Status NVARCHAR(50),
    @location NVARCHAR(50)
AS
BEGIN

    DECLARE @UserName NVARCHAR(50);
    SELECT @UserName = Username
    FROM Sec.Users
    WHERE UserID = @UserID
          AND IsDeleted = 0;

    DECLARE @UserIDOLD NVARCHAR(50);

    SELECT @UserIDOLD = UserID
    FROM Sec.Users
    WHERE Username = 'as_' + @UserName
          AND IsDeleted = 0;

     IF OBJECT_ID('tempdb..#TravelItineraryOrder') IS NOT NULL
        DROP TABLE #TravelItineraryOrder;

    SELECT ROW_NUMBER() OVER (Order by ti.Ordering) AS RowNumber,  
	      ti.TravelAuthorizationNumber,
           TLC.CityDescription
    INTO #TravelItineraryOrder FROM TA.TravelItinerary ti
        INNER JOIN TA.TravelAuthorization t
            ON t.TravelAuthorizationNumber = ti.TravelAuthorizationNumber
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
          AND t.UserID IN ( @UserID, @UserIDOLD )
          AND t.isDeleted = 0
          AND
          (
              t.[TravelAuthorizationNumber] LIKE '%' + @TravelAuthorizationNumber + '%'
              OR @TravelAuthorizationNumber = ''
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
		  
		  --ORDER BY t.TravelAuthorizationNumber,ti.Ordering;


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
           t.IsTecComplete,
           UPPER(Employee.[FirstName]) + ' ' + UPPER(Employee.[LastName]) AS UpdatedByName,
           travel.FromLocationDate,
           travel.ToLocationDate,
           CONVERT(VARCHAR, travel.FromLocationDate, 120) AS FromLocationDateSorting,
           CONVERT(VARCHAR, travel.ToLocationDate, 120) AS ToLocationDateSorting,
           TopOneFrom.TopOneFrom + '-' + REPLACE(CONCATLegs.CONCATLegs, ',', '-') AS CONCATLegs
    FROM [TA].[TravelAuthorization] t
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
        OUTER APPLY
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
        OUTER APPLY
    (
        SELECT CityDescription
        FROM Lookups.Lkp.City c
            INNER JOIN TA.TravelItinerary tt
                ON tt.FromLocationCode = c.CityID
        WHERE tt.TravelAuthorizationNumber = t.TravelAuthorizationNumber
    ) locationfrom
        OUTER APPLY
    (
        SELECT CityDescription
        FROM Lookups.Lkp.City c
            INNER JOIN TA.TravelItinerary ti
                ON ti.ToLocationCode = c.CityID
        WHERE ti.TravelAuthorizationNumber = t.TravelAuthorizationNumber
    ) locationto
        OUTER APPLY
    (
  --      SELECT dbo.GROUP_CONCAT(tio.CityDescription) AS CONCATLegs
  --      FROM #TravelItineraryOrder tio 
		--WHERE tio.TravelAuthorizationNumber = t.TravelAuthorizationNumber
		    SELECT DISTINCT
               CONCATLegs = STUFF(
                            (
                                SELECT ',' + CityDescription FROM #TravelItineraryOrder WHERE TravelAuthorizationNumber = t.TravelAuthorizationNumber FOR XML PATH('')
                            ),
                            1,
                            1,
                            ''
                                 )
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
    WHERE t.UserID IN ( @UserID, @UserIDOLD )
          AND t.isDeleted = 0
          AND
          (
              t.[TravelAuthorizationNumber] LIKE '%' + @TravelAuthorizationNumber + '%'
              OR @TravelAuthorizationNumber = ''
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
    ORDER BY CreatedDate DESC;

END;