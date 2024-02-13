CREATE PROCEDURE [TA].[UpdateTravelAuthorizationCOMRole]
	@TravelAuthorizationID nvarchar(100),
	@IsCOMRequired bit,
	@COMUserID nvarchar(100)
AS
	UPDATE TA.TravelAuthorization
	SET
	IsCOMRequired = @IsCOMRequired,
	COMUserID = @COMUserID
	WHERE
	TravelAuthorizationID = @TravelAuthorizationID