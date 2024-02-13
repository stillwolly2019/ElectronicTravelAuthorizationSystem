CREATE PROCEDURE [TEC].[GetTECByTANumber]
@TravelAuthorizationNumber nvarchar(14)
AS
BEGIN
	SELECT * FROM [TEC].[TECAdvances] WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber AND isDeleted = 0
	SELECT * FROM [TEC].[TECExpenditure] WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber AND isDeleted = 0
	SELECT * FROM [TEC].[TECItinerary] WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber AND isDeleted = 0
END
