
CREATE PROCEDURE [TEC].[CheckTravelIteneraryTime]
@TravelAuthorizationNumber NVARCHAR(100)
AS
BEGIN
	SELECT 
		COUNT(1) TotalNotFilled,
		(SELECT COUNT(1) FROM TA.TravelItinerary WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber AND isDeleted = 0) Total
	FROM 
		TA.TravelItinerary 
	WHERE 
		TravelAuthorizationNumber = @TravelAuthorizationNumber AND 
		(FromLocationTime IS NULL OR ToLocationTime IS NULL) AND 
		isDeleted = 0
END

