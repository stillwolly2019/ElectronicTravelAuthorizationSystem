CREATE PROCEDURE [Rpt].[TAsList]

@Status nvarchar(500),
@CreatedDateFrom date,
@CreatedDateTo date

AS


SELECT DISTINCT
t.TravelAuthorizationNumber,
t.TravelersName,
Dep.DepartureDate,
Arr.ArrivalDate,
tsc.[Description] 'Trip Schema Code',
mode.[Description] 'ModeOfTravelCode',
t.CityOfAccommodation,
CASE WHEN t.IsTravelAdvanceRequested = 1 THEN 'Yes' ELSE 'No' END 'Travel Advance Requested',
wbs.WBSCode 'WBS',
wbs.PercentageOrAmount,
LatestStatus.RejectionReasons 'Comments',
LatestStatus.CreatedDate 'Status Date',
LatestStatus.[Description] 'Status',
u.FirstName + ' ' + u.LastName 'Created By',
t.CreatedDate,
CASE WHEN t.IsNotForDSA=1 THEN 'Yes' ELSE 'No' END 'Not For DSA'


FROM TA.TravelAuthorization t 
OUTER APPLY (SELECT TOP 1 l.[Description],h.CreatedDate,RejectionReasons FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=t.TravelAuthorizationID AND isDeleted=0 ORDER BY h.CreatedDate DESC) LatestStatus
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


WHERE t.isDeleted = 0 AND
(@Status = LatestStatus.[Description] OR @Status IS NULL) AND
CONVERT(DATE, t.CreatedDate) >= CONVERT(DATE, ISNULL(@CreatedDateFrom,t.CreatedDate)) AND 
CONVERT(DATE, t.CreatedDate) <= CONVERT(DATE, ISNULL(@CreatedDateTo,t.CreatedDate))

ORDER BY t.TravelAuthorizationNumber
