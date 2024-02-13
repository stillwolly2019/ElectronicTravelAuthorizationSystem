
CREATE PROCEDURE [TEC].[GetTECItineraryExchangeRateByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(100)
AS
BEGIN
	
SELECT ExchangeRate FROM [TEC].[TECItinerary] where TravelAuthorizationNumber = @TravelAuthorizationNumber order by ExchangeRate desc

END
