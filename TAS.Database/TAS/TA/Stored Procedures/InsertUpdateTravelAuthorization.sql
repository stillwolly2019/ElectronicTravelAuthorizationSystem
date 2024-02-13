

CREATE PROCEDURE [TA].[InsertUpdateTravelAuthorization]
@TravelAuthorizationID nvarchar(100),
@PRISMMissionCode nvarchar(20),
@UserID nvarchar(100),
--@TravelersName nvarchar(2000),
@FirstName nvarchar(500),
@MiddleName nvarchar(500),
@LastName nvarchar(500),
@PurposeOfTravel nvarchar(100),
@TripSchemaCode nvarchar(100),
@ModeOfTravelCode nvarchar(100),
@SecurityClearance bit,
@SecurityTraining bit,
@StatusCode nvarchar(3),
@CreatedBy nvarchar(100),
@CityOfAccommodation	nvarchar(500),
@IsPrivateStay	bit,
@PrivateStayDates	nvarchar(500),
@IsPrivateDeviation	bit,
@PrivateDeviationLegs	nvarchar(500),
@IsAccommodationProvided	bit,
@AccommodationDetails	nvarchar(500),
@IsTravelAdvanceRequested	bit,
@TravelAdvanceCurrency	nvarchar(100),
@TravelAdvanceAmount	float,
@TravelAdvanceMethod	nvarchar(50),
@IsVisaObtained	int,
@VisaIssued nvarchar(500),
@IsVaccinationObtained	int,
@IsSecurityClearanceRequestedByMission	bit,
@TAConfirm	bit,
@PrivateStayDateFrom datetime,
@PrivateStayDateTo datetime,
@IsNotForDSA bit

AS
BEGIN
BEGIN TRY ------------BEGIN TRY
    BEGIN TRANSACTION;-- BEGIN Main Transaction

IF @TravelAuthorizationID =''
BEGIN

declare @TravelAuthorizationNumber as nvarchar(14);

declare @TANumber as int = (select top 1 SUBSTRING([TravelAuthorizationNumber],6,4) from [TA].[TravelAuthorization]   order by CreatedDate desc)

declare @CurrentYear as int = RIGHT(year(getdate()),2)

IF((select count(*) from [TA].[TravelAuthorization] where SUBSTRING([TravelAuthorizationNumber],11,2) = @CurrentYear) > 0)
BEGIN
SET @TravelAuthorizationNumber = @PRISMMissionCode + '/' + REPLACE(STR(@TANumber + 1, 4), SPACE(1), '0') + '/' + RIGHT(year(getdate()),2)
END
ELSE
BEGIN
SET @TravelAuthorizationNumber = @PRISMMissionCode + '/' + '1701' + '/' + RIGHT(year(getdate()),2)
END


DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
				DECLARE @NewID UNIQUEIDENTIFIER


INSERT INTO [TA].[TravelAuthorization]
           (
		   [TravelAuthorizationNumber]
           ,[UserID]
           ,[TravelersName]
		   ,[FirstName]
		   ,[MiddleName]
		   ,[LastName]
           ,[PurposeOfTravel]
           ,[TripSchemaCode]
           ,[ModeOfTravelCode]
           ,[SecurityClearance]
           ,[SecurityTraining]
		   ,[StatusCode]
           ,[CreatedBy]
		   ,CityOfAccommodation
		   ,IsPrivateStay
		   ,PrivateStayDates
		   ,IsPrivateDeviation
		   ,PrivateDeviationLegs
		   ,IsAccommodationProvided
		   ,AccommodationDetails
		   ,IsTravelAdvanceRequested
		   ,TravelAdvanceCurrency
		   ,TravelAdvanceAmount
		   ,TravelAdvanceMethod
		   ,IsVisaObtained
		   ,VisaIssued
		   ,IsVaccinationObtained
		   ,IsSecurityClearanceRequestedByMission
		   ,TAConfirm
		   ,PrivateStayDateFrom
		   ,PrivateStayDateTo
		   ,IsNotForDSA
		   )
	 OUTPUT INSERTED.TravelAuthorizationID INTO @myNewPKTable
     VALUES
           (
		   @TravelAuthorizationNumber,
		   @UserID,
		   @FirstName + ' ' + @MiddleName + ' ' + @LastName,
		   @FirstName,
		   @MiddleName,
		   @LastName,
		   @PurposeOfTravel,
		   @TripSchemaCode,
		   @ModeOfTravelCode,
		   @SecurityClearance,
		   @SecurityTraining,
		   @StatusCode,
		   @CreatedBy,
		   @CityOfAccommodation,
		   @IsPrivateStay,
		   @PrivateStayDates,
		   @IsPrivateDeviation,
		   @PrivateDeviationLegs,
		   @IsAccommodationProvided,
		   @AccommodationDetails,
		   @IsTravelAdvanceRequested,
		   @TravelAdvanceCurrency,
		   @TravelAdvanceAmount,
		   @TravelAdvanceMethod,
		   @IsVisaObtained,
		   @VisaIssued,
		   @IsVaccinationObtained,
		   @IsSecurityClearanceRequestedByMission,
		   @TAConfirm,
		   @PrivateStayDateFrom,
		   @PrivateStayDateTo,
		   @IsNotForDSA
		   )

		   INSERT INTO TA.StatusChangeHistory 
		   (TravelAuthorizationID, StatusCode,  CreatedBy)
		   SELECT *,'PEN',  @CreatedBy FROM @myNewPKTable

		   

		   SELECT * FROM @myNewPKTable
END
ELSE
BEGIN
UPDATE [TA].[TravelAuthorization]
	SET
		 
         [TravelersName] = @FirstName + ' ' + @MiddleName + ' ' + @LastName,
		 [FirstName] =  @FirstName,
		 [MiddleName] = @MiddleName,
		 [LastName] = @LastName,
         [PurposeOfTravel] = @PurposeOfTravel,
         [TripSchemaCode] = @TripSchemaCode,
         [ModeOfTravelCode] = @ModeOfTravelCode , 
         [SecurityClearance] = @SecurityClearance,
         [SecurityTraining] = @SecurityTraining,
		 [StatusCode] = @StatusCode,
         [UpdatedBy] = @CreatedBy,
		 UpdatedDate = GETDATE(),
		 CityOfAccommodation = @CityOfAccommodation,
		 IsPrivateStay=@IsPrivateStay,
		 PrivateStayDates=@PrivateStayDates,
		 IsPrivateDeviation=@IsPrivateDeviation,
		 PrivateDeviationLegs=@PrivateDeviationLegs,
		 IsAccommodationProvided=@IsAccommodationProvided,
		 AccommodationDetails=@AccommodationDetails,
		 IsTravelAdvanceRequested=@IsTravelAdvanceRequested,
		 TravelAdvanceCurrency=@TravelAdvanceCurrency,
		 TravelAdvanceAmount=@TravelAdvanceAmount,
		 TravelAdvanceMethod=@TravelAdvanceMethod,
		 IsVisaObtained=@IsVisaObtained,
		 VisaIssued=@VisaIssued,
		 IsVaccinationObtained=@IsVaccinationObtained,
		 IsSecurityClearanceRequestedByMission=@IsSecurityClearanceRequestedByMission,
		 TAConfirm=@TAConfirm,
		 PrivateStayDateFrom=@PrivateStayDateFrom,
		 PrivateStayDateTo=@PrivateStayDateTo,
		 IsNotForDSA = @IsNotForDSA
	WHERE TravelAuthorizationID = @TravelAuthorizationID
	select @TravelAuthorizationID
END

COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION;
		SELECT '00000000-0000-0000-0000-000000000000' AS myNewPKErr
END CATCH --------END CATCH
END