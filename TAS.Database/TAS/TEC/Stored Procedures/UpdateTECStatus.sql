


CREATE PROCEDURE [TEC].[UpdateTECStatus]
@TravelAuthorizationNumber nvarchar(100),
@StatusCode nvarchar(100),
@Modified nvarchar(100),
@Comments nvarchar(max)

AS
BEGIN
	UPDATE [TEC].[TECItinerary] SET
		[StatusCode] = @StatusCode,
        [UpdatedBy] = @Modified,
		UpdatedDate = GETDATE()
	WHERE 
		TravelAuthorizationNumber = @TravelAuthorizationNumber

INSERT 
INTO [TEC].StatusChangeHistory
	(TravelAuthorizationID,StatusCode, Comments, CreatedBy)
	SELECT TravelAuthorizationID, @StatusCode, @Comments, @Modified FROM TA.TravelAuthorization WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber

IF @StatusCode = 'COM'
BEGIN
DECLARE @TravelAuthorizationID nvarchar(100)
	EXEC [TA].[UpdateTravelAuthorizationStatus] @TravelAuthorizationID, @StatusCode, @Modified, @Comments
END


END
