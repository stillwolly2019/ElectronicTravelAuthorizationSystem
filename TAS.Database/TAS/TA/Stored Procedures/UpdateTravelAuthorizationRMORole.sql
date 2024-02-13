CREATE PROCEDURE [TA].[UpdateTravelAuthorizationRMORole]
	@TravelAuthorizationID nvarchar(100),
	@IsRMORequired bit
AS
	UPDATE TA.TravelAuthorization
	SET
	IsRMORequired = @IsRMORequired	
	WHERE
	TravelAuthorizationID = @TravelAuthorizationID