

CREATE PROCEDURE [TA].[DeleteTravelItinerary]
@TravelItineraryID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN
UPDATE TA.TravelItinerary SET 
	IsDeleted = 1,
	[UpdatedBy] = @CreatedBy,
	[UpdatedDate] = GETDATE()
WHERE TravelItineraryID= @TravelItineraryID
END



