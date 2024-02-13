
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[CheckDuplicatedTA]
    @TravelAuthorizationID NVARCHAR(100),
    @TravelersName NVARCHAR(500),
    @FromLocationDate DATE,
    @ToLocationDate DATE,
    @FromLocationCode NVARCHAR(100),
    @ToLocationCode NVARCHAR(100),
    @TravelItinerary NVARCHAR(100)
AS
BEGIN

    -- Check if the itinerary in the same TA or Not 
    -- if the same TA we need to check the location and date
    -- if the same TA we need to check the dates only

    IF (@TravelAuthorizationID = '')
    BEGIN
        SELECT TA.*
        FROM TA.TravelAuthorization TA
            LEFT OUTER JOIN TA.TravelItinerary TI
                ON TA.TravelAuthorizationNumber = TI.TravelAuthorizationNumber
            CROSS APPLY
        (
            SELECT TOP 1
                   *
            FROM TA.StatusChangeHistory SCH
            WHERE TA.TravelAuthorizationID = SCH.TravelAuthorizationID
                  AND SCH.StatusCode <> 'CAN'
            ORDER BY CreatedDate DESC
        ) His
        WHERE REPLACE(TravelersName, '  ', ' ') = REPLACE(@TravelersName, '  ', ' ')
              AND CONVERT(VARCHAR(10), TI.FromLocationDate, 120) = CONVERT(VARCHAR(10), @FromLocationDate, 120)
              AND TI.FromLocationCode = @FromLocationCode
              AND CONVERT(VARCHAR(10), TI.ToLocationDate, 120) = CONVERT(VARCHAR(10), @ToLocationDate, 120)
              AND TI.ToLocationCode = @ToLocationCode
              AND TA.isDeleted = 0
              AND TI.isDeleted = 0;
    END;
    ELSE
    BEGIN

        IF (@TravelItinerary = '')
        BEGIN
            SELECT TA.*
            FROM TA.TravelAuthorization TA
                LEFT OUTER JOIN TA.TravelItinerary TI
                    ON TA.TravelAuthorizationNumber = TI.TravelAuthorizationNumber
                CROSS APPLY
            (
                SELECT TOP 1
                       *
                FROM TA.StatusChangeHistory SCH
                WHERE TA.TravelAuthorizationID = SCH.TravelAuthorizationID
                      AND SCH.StatusCode <> 'CAN'
                ORDER BY CreatedDate DESC
            ) His
            WHERE REPLACE(TravelersName, '  ', ' ') = REPLACE(@TravelersName, '  ', ' ')
                  AND CONVERT(VARCHAR(10), TI.FromLocationDate, 120) = CONVERT(VARCHAR(10), @FromLocationDate, 120)
                  AND TI.FromLocationCode = @FromLocationCode
                  AND CONVERT(VARCHAR(10), TI.ToLocationDate, 120) = CONVERT(VARCHAR(10), @ToLocationDate, 120)
                  AND TI.ToLocationCode = @ToLocationCode
                  AND TA.isDeleted = 0
                  AND TI.isDeleted = 0;

        END;
        ELSE
        BEGIN
            SELECT TA.*
            FROM TA.TravelAuthorization TA
                LEFT OUTER JOIN TA.TravelItinerary TI
                    ON TA.TravelAuthorizationNumber = TI.TravelAuthorizationNumber
                CROSS APPLY
            (
                SELECT TOP 1
                       *
                FROM TA.StatusChangeHistory SCH
                WHERE TA.TravelAuthorizationID = SCH.TravelAuthorizationID
                      AND SCH.StatusCode <> 'CAN'
                ORDER BY CreatedDate DESC
            ) His
            WHERE REPLACE(TravelersName, '  ', ' ') = REPLACE(@TravelersName, '  ', ' ')
                  AND CONVERT(VARCHAR(10), TI.FromLocationDate, 120) = CONVERT(VARCHAR(10), @FromLocationDate, 120)
                  AND TI.FromLocationCode = @FromLocationCode
                  AND CONVERT(VARCHAR(10), TI.ToLocationDate, 120) = CONVERT(VARCHAR(10), @ToLocationDate, 120)
                  AND TI.ToLocationCode = @ToLocationCode
                  AND TA.isDeleted = 0
                  AND TI.isDeleted = 0
                  AND TI.TravelItineraryID <> @TravelItinerary;

        END;
    -- end Itinerary check

    END;
-- end TA Check

END;

