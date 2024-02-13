CREATE PROCEDURE [Rpt].[AllTAsWithStatusesByDates]
@TravelAuthorizationNo nvarchar(500),
@Status nvarchar(50),
@DateFrom date,
@DateTo date

AS 

WITH base as (
SELECT 
[TravelAuthorizationID],
h.CreatedDate,
[Status] = l.Description,
RejectionReasons,
h.CreatedBy,
NoOfDays = CONVERT(VARCHAR,DATEDIFF(d,h.CreatedDate,GETDATE())) + ' Days'
FROM TA.StatusChangeHistory h INNER JOIN
      Lkp.Lookups l ON l.Code = StatusCode INNER JOIN
      Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND LookupGroup = 'TA Status Code') ,
rnk as (select *,RANK() over (partition by [TravelAuthorizationID] order by [CreatedDate] desc) as rnk from base),
TALatest as (select * from rnk where rnk=1) 
  

SELECT DISTINCT h.[TravelAuthorizationID],ta.TravelAuthorizationNumber,ta.TravelersName,
h.CreatedDate,
h.[Status] ,
RejectionReasons,DepartureDate = Itinerary.FromLocationDate,ArrivalDate = Itinerary.ToLocationDate

 FROM TALatest h INNER JOIN
              TA.TravelAuthorization ta ON h.TravelAuthorizationID = ta.TravelAuthorizationID  CROSS APPLY
	 (SELECT TravelAuthorizationNumber,ToLocationDate=MAX(ToLocationDate),FromLocationDate=MIN(FromLocationDate) FROM TA.TravelItinerary WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber AND isDeleted=0 GROUP BY TravelAuthorizationNumber) Itinerary
	 	INNER JOIN Sec.Users Employee ON Employee.UserID = ta.UserID
								CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.FromLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=ta.TravelAuthorizationNumber )
		locationfrom
		CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.ToLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=ta.TravelAuthorizationNumber )locationto
WHERE ta.isDeleted = 0 AND
(@TravelAuthorizationNo = ta.TravelAuthorizationNumber OR @TravelAuthorizationNo IS NULL) AND
(@Status = [Status] OR @Status IS NULL) AND
CONVERT(DATE, h.CreatedDate) >= CONVERT(DATE, ISNULL(@DateFrom,h.CreatedDate))
		AND CONVERT(DATE, h.CreatedDate) <= CONVERT(DATE, ISNULL(@DateTo,h.CreatedDate))

ORDER BY ta.TravelAuthorizationNumber