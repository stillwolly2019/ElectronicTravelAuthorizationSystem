
CREATE PROCEDURE [TA].[GetArrivalDateByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(14)

AS
BEGIN

SELECT ti.TravelAuthorizationNumber,MIN(ti.FromLocationDate) as  FromLocationDate, MAX(ti.ToLocationDate) AS ToLocationDate
FROM 
	Ta.TravelItinerary ti
WHERE 
	ti.TravelAuthorizationNumber= @TravelAuthorizationNumber
	AND 
	ti.isDeleted = 0

GROUP BY  ti.TravelAuthorizationNumber

END

