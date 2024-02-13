-- Procedure
CREATE PROCEDURE [TEC].[InsertEmptyTECItineraryDSA]
@TravelItineraryID nvarchar(100),
@ModifiedBy nvarchar(100)
AS
BEGIN
IF NOT EXISTS(SELECT 1 FROM TEC.TECItinerary WHERE TravelItineraryID = @TravelItineraryID AND isDeleted = 0)
	BEGIN
		INSERT INTO TEC.TECItinerary
		 (TravelItineraryID,TravelAuthorizationNumber,NoOfDays,DSARate,RateAmount,LocalAmount, StatusCode ,CreatedBy)
		SELECT @TravelItineraryID, TravelAuthorizationNumber, 0,0,0,0, 'PEN',@ModifiedBy 
		FROM TA.TravelItinerary WHERE TravelItineraryID = @TravelItineraryID
	END
END


