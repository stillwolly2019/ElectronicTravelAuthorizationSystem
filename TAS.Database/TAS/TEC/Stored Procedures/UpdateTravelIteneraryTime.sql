CREATE PROCEDURE [TEC].[UpdateTravelIteneraryTime]
@TravelItineraryID nvarchar(100),
@Time time(7),
@DepArr nvarchar(4),
@ModifiedBy nvarchar(100)
AS
BEGIN
IF NOT EXISTS(SELECT 1 FROM TEC.TECItinerary WHERE TravelItineraryID = @TravelItineraryID AND isDeleted = 0)
BEGIN
	INSERT INTO TEC.TECItinerary (TravelItineraryID,TravelAuthorizationNumber, [StatusCode],CreatedBy)
	SELECT @TravelItineraryID, TravelAuthorizationNumber, 'PEN', @ModifiedBy FROM TA.TravelItinerary WHERE TravelItineraryID = @TravelItineraryID
END
	IF @DepArr = 'Dep.'
		BEGIN
			UPDATE TA.TravelItinerary SET
				FromLocationTime = @Time,
				UpdatedBy = @ModifiedBy,
				UpdatedDate = GETDATE()
			WHERE 
				TravelItineraryID = @TravelItineraryID
		END
	ELSE
		BEGIN
			UPDATE TA.TravelItinerary SET
				ToLocationTime = @Time,
				UpdatedBy = @ModifiedBy,
				UpdatedDate = GETDATE()
			WHERE 
				TravelItineraryID = @TravelItineraryID
		END

END
