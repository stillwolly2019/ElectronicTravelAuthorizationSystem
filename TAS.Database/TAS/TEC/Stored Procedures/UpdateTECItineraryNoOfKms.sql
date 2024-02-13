CREATE PROCEDURE [TEC].[UpdateTECItineraryNoOfKms]
@TravelItineraryID nvarchar(100),
@NoOfKms float,
@ModifiedBy nvarchar(100)
AS
BEGIN
IF NOT EXISTS(SELECT 1 FROM TEC.TECItinerary WHERE TravelItineraryID = @TravelItineraryID AND isDeleted = 0)
BEGIN
	INSERT INTO TEC.TECItinerary (TravelItineraryID,TravelAuthorizationNumber, [StatusCode],CreatedBy)
	SELECT @TravelItineraryID, TravelAuthorizationNumber, 'PEN', @ModifiedBy FROM TA.TravelItinerary WHERE TravelItineraryID = @TravelItineraryID
END
	UPDATE TEC.TECItinerary SET 
		NoOfKms = @NoOfKms,
		UpdatedBy = @ModifiedBy,
		UpdatedDate = GETDATE()
	WHERE TravelItineraryID = @TravelItineraryID
END
