
CREATE PROCEDURE [TA].[DuplicateTA]

@TravelAuthorizationID nvarchar(100),
@UserID nvarchar(100),
@PRISMMissionCode nvarchar(20),
@CreatedBy nvarchar(100)

AS
BEGIN
BEGIN TRY ------------BEGIN TRY
BEGIN TRANSACTION;-- BEGIN Main Transaction

DECLARE @TravelAuthorizationNumber nvarchar(100);
DECLARE @FirstName nvarchar(100);
DECLARE @LastName nvarchar(100);


SET @TravelAuthorizationNumber = (SELECT TravelAuthorizationNumber FROM [TA].[TravelAuthorization] WHERE TravelAuthorizationID= @TravelAuthorizationID)
SET @FirstName = (SELECT FirstName FROM [Sec].[Users] WHERE UserID = @UserID)
SET @LastName = (SELECT LastName FROM [Sec].[Users] WHERE UserID = @UserID)

DECLARE @NewTravelAuthorizationNumber as nvarchar(14);
DECLARE @TANumber as int = (select top 1 SUBSTRING([TravelAuthorizationNumber],6,4) from [TA].[TravelAuthorization]   order by CreatedDate desc)
DECLARE @CurrentYear as int = RIGHT(year(getdate()),2)

IF((select count(*) from [TA].[TravelAuthorization] where SUBSTRING([TravelAuthorizationNumber],11,2) = @CurrentYear) > 0)
BEGIN
SET @NewTravelAuthorizationNumber = @PRISMMissionCode + '/' + REPLACE(STR(@TANumber + 1, 4), SPACE(1), '0') + '/' + RIGHT(year(getdate()),2)
END
ELSE
BEGIN
SET @NewTravelAuthorizationNumber = @PRISMMissionCode + '/' + '1701' + '/' + RIGHT(year(getdate()),2)
END

DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
				DECLARE @NewID UNIQUEIDENTIFIER

-- Duplicate TA

INSERT INTO [TA].[TravelAuthorization]
           ([TravelAuthorizationNumber]
           ,[UserID]
           ,[TravelersName]
           ,[FirstName]
           ,[LastName]
           ,[PurposeOfTravel]
           ,[TripSchemaCode]
           ,[ModeOfTravelCode]
           ,[SecurityClearance]
           ,[SecurityTraining]
           ,[StatusCode]
           ,[CityOfAccommodation]
           ,[IsPrivateStay]
           ,[PrivateStayDates]
           ,[PrivateStayDateFrom]
           ,[PrivateStayDateTo]
           ,[IsPrivateDeviation]
           ,[PrivateDeviationLegs]
           ,[IsAccommodationProvided]
           ,[AccommodationDetails]
           ,[IsTravelAdvanceRequested]
           ,[TravelAdvanceCurrency]
           ,[TravelAdvanceAmount]
           ,[TravelAdvanceMethod]
           ,[IsVisaObtained]
           ,[VisaIssued]
           ,[IsVaccinationObtained]
           ,[IsSecurityClearanceRequestedByMission]
           ,[TAConfirm]
           ,[IsNotForDSA]
           ,[CreatedBy])

		   OUTPUT INSERTED.TravelAuthorizationID INTO @myNewPKTable
			
     SELECT @NewTravelAuthorizationNumber
           ,@UserID
           ,@FirstName + ' ' + @LastName
           ,@FirstName
           ,@LastName
           ,[PurposeOfTravel]
           ,[TripSchemaCode]
           ,[ModeOfTravelCode]
           ,[SecurityClearance]
           ,[SecurityTraining]
           ,[StatusCode]
           ,[CityOfAccommodation]
           ,[IsPrivateStay]
           ,[PrivateStayDates]
           ,[PrivateStayDateFrom]
           ,[PrivateStayDateTo]
           ,[IsPrivateDeviation]
           ,[PrivateDeviationLegs]
           ,[IsAccommodationProvided]
           ,[AccommodationDetails]
           ,[IsTravelAdvanceRequested]
           ,[TravelAdvanceCurrency]
           ,[TravelAdvanceAmount]
           ,[TravelAdvanceMethod]
           ,[IsVisaObtained]
           ,[VisaIssued]
           ,[IsVaccinationObtained]
           ,[IsSecurityClearanceRequestedByMission]
           ,[TAConfirm]
           ,[IsNotForDSA]
           ,@CreatedBy
       FROM [TA].[TravelAuthorization]
	   Where TravelAuthorizationID = @TravelAuthorizationID and isDeleted = 0;

-- Duplicate WBS

DECLARE @TAID as nvarchar(100) = (select * from @myNewPKTable);

INSERT INTO [TA].[WBS]
           ([TravelAuthorizationID]
           ,[WBSCode]
           ,[PercentageOrAmount]
           ,[Note]
           ,[IsPercentage]
           ,[CreatedBy])

	 (SELECT @TAID
           ,[WBSCode]
           ,[PercentageOrAmount]
           ,[Note]
           ,[IsPercentage]
           ,@CreatedBy
     FROM [TA].[WBS] Where TravelAuthorizationID = @TravelAuthorizationID and isDeleted = 0)

-- Duplicate Itinerary

INSERT INTO [TA].[TravelItinerary]
           ([TravelAuthorizationNumber]
           ,[ModeOfTravelID]
           ,[FromLocationCode]
           ,[FromLocationDate]
           ,[ToLocationCode]
           ,[ToLocationDate]
           ,[CreatedBy]
           ,[Ordering])

     (SELECT @NewTravelAuthorizationNumber
      ,[ModeOfTravelID]
      ,[FromLocationCode]
      ,[FromLocationDate]
      ,[ToLocationCode]
      ,[ToLocationDate]
      ,@CreatedBy
      ,[Ordering]
  FROM [TA].[TravelItinerary] WHERE  [TravelAuthorizationNumber] = @TravelAuthorizationNumber and isDeleted = 0)

-- Status History
  
  INSERT INTO TA.StatusChangeHistory 
  (TravelAuthorizationID, StatusCode,  CreatedBy)
  Values
  (@TAID,'PEN',@CreatedBy)
  



COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION;
END CATCH --------END CATCH
END

