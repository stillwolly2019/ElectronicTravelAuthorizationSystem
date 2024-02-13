-- Procedure
CREATE PROCEDURE [TEC].[GetTECItineraryDSAByTECItineraryID]
@TravelItineraryID nvarchar(100)
AS

SELECT * FROM TEC.TECItineraryDSA WHERE TECItineraryID = (SELECT TECItineraryID FROM TEC.TECItinerary WHERE TravelItineraryID = @TravelItineraryID AND isDeleted=0) AND isDeleted=0