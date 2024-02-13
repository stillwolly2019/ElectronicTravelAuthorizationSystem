

CREATE PROCEDURE [TA].[UpdateAdvanceAndSecurity]

@TravelAuthorizationID nvarchar(100),
@SecurityClearance bit,
@SecurityTraining bit,
@IsTravelAdvanceRequested bit,
@TravelAdvanceCurrency nvarchar(100),
@TravelAdvanceAmount float,
@TravelAdvanceMethod nvarchar(50),
@IsVisaObtained	int,
@VisaIssued nvarchar(500),
@IsVaccinationObtained int,
@IsSecurityClearanceRequestedByMission	bit,
@TAConfirm	bit,
@IsNotForDSA bit,
@ModifiedBy nvarchar(100)

AS
BEGIN
BEGIN TRY 
BEGIN TRANSACTION

UPDATE [TA].[TravelAuthorization]
	SET
	[SecurityClearance] = @SecurityClearance,
    [SecurityTraining] = @SecurityTraining,
	IsTravelAdvanceRequested=@IsTravelAdvanceRequested,
    TravelAdvanceCurrency=@TravelAdvanceCurrency,
    TravelAdvanceAmount=@TravelAdvanceAmount,
    TravelAdvanceMethod=@TravelAdvanceMethod,
    IsVisaObtained=@IsVisaObtained,
    VisaIssued=@VisaIssued,
    IsVaccinationObtained=@IsVaccinationObtained,
    IsSecurityClearanceRequestedByMission=@IsSecurityClearanceRequestedByMission,
    TAConfirm=@TAConfirm,
    IsNotForDSA = @IsNotForDSA ,
	[UpdatedBy] = @ModifiedBy,
    [UpdatedDate] = Getdate()
	
	WHERE TravelAuthorizationID = @TravelAuthorizationID

COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION;
END CATCH --------END CATCH
END




