CREATE PROCEDURE [Rpt].[HistorySearchLog]

@TravelAuthorizationNo nvarchar(500),
@Status nvarchar(50),
@DateFrom date,
@DateTo date,
@Report nvarchar(10)

AS

WITH Base AS (
SELECT DISTINCT t.TravelAuthorizationNumber,
       [Status]= h.[Description],
       [RejectionReasons],
       h.[CreatedDate],
       [CreatedBy] = u.Username,
	   [Report] = 'TA'
FROM TA.TravelAuthorization t 
OUTER APPLY (SELECT l.[Description],h.CreatedDate,RejectionReasons FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=t.TravelAuthorizationID AND isDeleted=0) h
CROSS APPLY (SELECT TOP 1 DepartureDate=FromLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=t.TravelAuthorizationNumber AND  isDeleted=0 ORDER BY CreatedDate ASC) Dep
CROSS APPLY (SELECT TOP 1 ArrivalDate=ToLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=t.TravelAuthorizationNumber ORDER BY CreatedDate DESC) Arr
CROSS APPLY (SELECT [Description] FROM Lkp.Lookups WHERE LookupsID = t.TripSchemaCode) tsc
CROSS APPLY (SELECT [Description] FROM Lkp.Lookups WHERE LookupsID = t.ModeOfTravelCode) mode
INNER JOIN TA.WBS wbs ON wbs.TravelAuthorizationID = t.TravelAuthorizationID AND wbs.isDeleted=0
INNER JOIN sec.Users u ON u.UserID = t.UserID 
						CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.FromLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=t.TravelAuthorizationNumber )
		locationfrom
		CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.ToLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=t.TravelAuthorizationNumber )locationto


WHERE t.isDeleted = 0 and h.[Description] LIKE '%TA%'


	   UNION

SELECT DISTINCT t.TravelAuthorizationNumber,
       [Status]= h.[Description],
       [RejectionReasons],
       h.[CreatedDate],
       [CreatedBy] = u.Username,
	   [Report] = 'TEC'
FROM TA.TravelAuthorization t 
OUTER APPLY (SELECT l.[Description],h.CreatedDate,RejectionReasons FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=t.TravelAuthorizationID AND isDeleted=0) h
CROSS APPLY (SELECT TOP 1 DepartureDate=FromLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=t.TravelAuthorizationNumber AND  isDeleted=0 ORDER BY CreatedDate ASC) Dep
CROSS APPLY (SELECT TOP 1 ArrivalDate=ToLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=t.TravelAuthorizationNumber ORDER BY CreatedDate DESC) Arr
CROSS APPLY (SELECT [Description] FROM Lkp.Lookups WHERE LookupsID = t.TripSchemaCode) tsc
CROSS APPLY (SELECT [Description] FROM Lkp.Lookups WHERE LookupsID = t.ModeOfTravelCode) mode
INNER JOIN TA.WBS wbs ON wbs.TravelAuthorizationID = t.TravelAuthorizationID AND wbs.isDeleted=0
INNER JOIN sec.Users u ON u.UserID = t.UserID 
		CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.FromLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=t.TravelAuthorizationNumber )
		locationfrom
		CROSS APPLY(
		SELECT CityDescription FROM Lookups.Lkp.City  c
		INNER JOIN ta.TravelItinerary ti ON ti.ToLocationCode= c.CityID
		WHERE ti.TravelAuthorizationNumber=t.TravelAuthorizationNumber )locationto
WHERE t.isDeleted = 0 and h.[Description] LIKE '%TEC%'
	)

SELECT * FROM Base
WHERE (@TravelAuthorizationNo = TravelAuthorizationNumber OR @TravelAuthorizationNo ='All') AND
      (@Status = [Status] OR @Status ='All') AND
	  (@Report = Report OR @Report ='All') AND
	  CONVERT(DATE, [CreatedDate]) >= CONVERT(DATE, ISNULL(@DateFrom,[CreatedDate]))
		AND CONVERT(DATE, [CreatedDate]) <= CONVERT(DATE, ISNULL(@DateTo,[CreatedDate]))


ORDER BY  [CreatedDate], TravelAuthorizationNumber, [Status]