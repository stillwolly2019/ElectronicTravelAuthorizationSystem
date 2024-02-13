CREATE PROCEDURE [TEC].[UpdateTECItineraryDSA]
@TravelItineraryID nvarchar(100),
@NoOfDays float,
@DSARate float,
@RateAmount float,
@LocalAmount float,
@ModifiedBy nvarchar(100)
AS
BEGIN
IF NOT EXISTS(SELECT 1 FROM TEC.TECItinerary WHERE TravelItineraryID = @TravelItineraryID AND isDeleted = 0)
BEGIN
	INSERT INTO TEC.TECItinerary (TravelItineraryID,TravelAuthorizationNumber,NoOfDays,DSARate,RateAmount,LocalAmount, [StatusCode] ,CreatedBy)
	SELECT @TravelItineraryID, TravelAuthorizationNumber, @NoOfDays,@DSARate,@RateAmount,@LocalAmount, '5c71c3b3-2044-4afe-a1bf-1d94e6a622f3',@ModifiedBy FROM TA.TravelItinerary WHERE TravelItineraryID = @TravelItineraryID
END
	UPDATE TEC.TECItinerary SET 
		NoOfDays = @NoOfDays,
		DSARate = @DSARate,
		RateAmount = @RateAmount,
		LocalAmount = @LocalAmount,
		UpdatedBy = @ModifiedBy,
		UpdatedDate = GETDATE()
	WHERE TravelItineraryID = @TravelItineraryID
END
