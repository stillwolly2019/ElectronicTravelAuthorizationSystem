CREATE PROCEDURE [Rpt].[AllTAsWithStatuses]



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
  

SELECT h.[TravelAuthorizationID],
[TravelAuthorizationNumber],
[TravelersName],
[Status] = h.[Status],
[Date] = h.CreatedDate,
h.RejectionReasons,
DepartureDate.DepartureDate,
ArrivalDate.ArrivalDate

FROM ta.TravelAuthorization ta INNER JOIN
     TALatest h ON h.TravelAuthorizationID = ta.TravelAuthorizationID OUTER APPLY
	 (SELECT DepartureDate = MIN([FromLocationDate]) FROM [TA].[TravelItinerary] WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber) DepartureDate OUTER APPLY
	 (SELECT ArrivalDate = MAX([FromLocationDate]) FROM [TA].[TravelItinerary] WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber) ArrivalDate

WHERE ta.isDeleted = 0 
