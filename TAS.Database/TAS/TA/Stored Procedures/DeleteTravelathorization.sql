
CREATE PROCEDURE [TA].[DeleteTravelathorization]
@TravelAuthorizationID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN
UPDATE TA.TravelAuthorization
SET
	IsDeleted = 1,
	[UpdatedBy] = @CreatedBy,
	[UpdatedDate] = GETDATE()
WHERE TravelAuthorizationID= @TravelAuthorizationID
END
