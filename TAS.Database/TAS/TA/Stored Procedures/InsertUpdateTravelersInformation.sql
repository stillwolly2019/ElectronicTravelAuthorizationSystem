
CREATE PROCEDURE [TA].[InsertUpdateTravelersInformation]
    @TravelAuthorizationID NVARCHAR(100),
    @PRISMMissionCode NVARCHAR(20),
    @UserID NVARCHAR(100),
    @FirstName NVARCHAR(500),
    @MiddleName NVARCHAR(500),
    @LastName NVARCHAR(500),
    @PurposeOfTravel NVARCHAR(100),
    @TripSchemaCode NVARCHAR(100),
    @ModeOfTravelCode NVARCHAR(100),
    @StatusCode NVARCHAR(3),
    @CreatedBy NVARCHAR(100),
    @CityOfAccommodation NVARCHAR(500),
    @IsPrivateStay BIT,
    @PrivateStayDates NVARCHAR(500),
    @IsPrivateDeviation BIT,
    @PrivateDeviationLegs NVARCHAR(500),
    @IsAccommodationProvided BIT,
    @AccommodationDetails NVARCHAR(500),
    @PrivateStayDateFrom DATETIME,
    @PrivateStayDateTo DATETIME,
    @FamilyMembers NVARCHAR(4000)
AS
BEGIN
    BEGIN TRY ------------BEGIN TRY
        BEGIN TRANSACTION; -- BEGIN Main Transaction

        IF @TravelAuthorizationID = ''
        BEGIN

            DECLARE @TravelAuthorizationNumber AS NVARCHAR(14);
            DECLARE @TANumber AS INT =
                    (
                        SELECT TOP 1
                               SUBSTRING([TravelAuthorizationNumber], 6, 4)
                        FROM [TA].[TravelAuthorization]
                        ORDER BY CreatedDate DESC
                    );
            DECLARE @CurrentYear AS INT = RIGHT(YEAR(GETDATE()), 2);

            IF (
               (
                   SELECT COUNT(*)
                   FROM [TA].[TravelAuthorization]
                   WHERE SUBSTRING([TravelAuthorizationNumber], 11, 2) = @CurrentYear
               ) > 0
               )
            BEGIN
                SET @TravelAuthorizationNumber
                    = @PRISMMissionCode + '/' + REPLACE(STR(@TANumber + 1, 4), SPACE(1), '0') + '/'
                      + RIGHT(YEAR(GETDATE()), 2);
            END;
            ELSE
            BEGIN
                SET @TravelAuthorizationNumber = @PRISMMissionCode + '/' + '0001' + '/' + RIGHT(YEAR(GETDATE()), 2);
            END;


            DECLARE @myNewPKTable TABLE
            (
                myNewPK UNIQUEIDENTIFIER
            );
            DECLARE @NewID UNIQUEIDENTIFIER;


            INSERT INTO [TA].[TravelAuthorization]
            (
                [TravelAuthorizationNumber],
                [UserID],
                [TravelersName],
                [FirstName],
                [MiddleName],
                [LastName],
                [PurposeOfTravel],
                [TripSchemaCode],
                [ModeOfTravelCode],
                [StatusCode],
                [CreatedBy],
                CityOfAccommodation,
                IsPrivateStay,
                PrivateStayDates,
                IsPrivateDeviation,
                PrivateDeviationLegs,
                IsAccommodationProvided,
                AccommodationDetails,
                PrivateStayDateFrom,
                PrivateStayDateTo,
                FamilyMembers
            )
            OUTPUT INSERTED.TravelAuthorizationID
            INTO @myNewPKTable
            VALUES
            (@TravelAuthorizationNumber, @UserID, @FirstName + ' ' + @MiddleName + ' ' + @LastName, @FirstName,
             @MiddleName, @LastName, @PurposeOfTravel, @TripSchemaCode, @ModeOfTravelCode, @StatusCode, @CreatedBy,
             @CityOfAccommodation, @IsPrivateStay, @PrivateStayDates, @IsPrivateDeviation, @PrivateDeviationLegs,
             @IsAccommodationProvided, @AccommodationDetails, @PrivateStayDateFrom, @PrivateStayDateTo, @FamilyMembers);

            IF
            (
                SELECT UserID
                FROM TA.TravelAuthorization
                WHERE TravelAuthorizationID =
                (
                    SELECT myNewPK FROM @myNewPKTable
                )
            ) = '00000000-0000-0000-0000-000000000000'
            BEGIN
                UPDATE TA.TravelAuthorization
                SET UserID = CreatedBy
                WHERE TravelAuthorizationID =
                (
                    SELECT myNewPK FROM @myNewPKTable
                );
            END;
            -- Status History

            INSERT INTO TA.StatusChangeHistory
            (
                TravelAuthorizationID,
                StatusCode,
                CreatedBy
            )
            SELECT *,
                   'PEN',
                   @CreatedBy
            FROM @myNewPKTable;

            SELECT *
            FROM @myNewPKTable;

        END;
        ELSE
        BEGIN
            UPDATE [TA].[TravelAuthorization]
            SET [TravelersName] = @FirstName + ' ' + @MiddleName + ' ' + @LastName,
                [FirstName] = @FirstName,
                [MiddleName] = @MiddleName,
                [LastName] = @LastName,
                [PurposeOfTravel] = @PurposeOfTravel,
                [TripSchemaCode] = @TripSchemaCode,
                [ModeOfTravelCode] = @ModeOfTravelCode,
                [UpdatedBy] = @CreatedBy,
                UpdatedDate = GETDATE(),
                CityOfAccommodation = @CityOfAccommodation,
                IsPrivateStay = @IsPrivateStay,
                PrivateStayDates = @PrivateStayDates,
                IsPrivateDeviation = @IsPrivateDeviation,
                PrivateDeviationLegs = @PrivateDeviationLegs,
                IsAccommodationProvided = @IsAccommodationProvided,
                AccommodationDetails = @AccommodationDetails,
                PrivateStayDateFrom = @PrivateStayDateFrom,
                PrivateStayDateTo = @PrivateStayDateTo,
                FamilyMembers = @FamilyMembers
            WHERE TravelAuthorizationID = @TravelAuthorizationID;
            SELECT @TravelAuthorizationID;
        END;

        COMMIT TRANSACTION; -- COMMIT Main Transaction
    END TRY ------------END TRY
    BEGIN CATCH --------BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT '00000000-0000-0000-0000-000000000000' AS myNewPKErr;
    END CATCH; --------END CATCH
END;