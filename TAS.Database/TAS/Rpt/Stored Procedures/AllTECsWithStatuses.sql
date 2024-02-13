CREATE PROCEDURE [Rpt].[AllTECsWithStatuses]


AS 

WITH base as (
SELECT 
[TravelAuthorizationID],
h.CreatedDate,
[Status] = l.Description,
Comments,
h.CreatedBy,
NoOfDays = CONVERT(VARCHAR,DATEDIFF(d,h.CreatedDate,GETDATE())) + ' Days'
FROM TEC.StatusChangeHistory h INNER JOIN
      Lkp.Lookups l ON l.Code = StatusCode INNER JOIN
      Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND LookupGroup = 'TEC Status Code') ,
rnk as (select *,RANK() over (partition by [TravelAuthorizationID] order by [CreatedDate] desc) as rnk from base),
TECLatest as (select * from rnk where rnk=1) 
  

SELECT h.TravelAuthorizationID,ta.TravelAuthorizationNumber,ta.TravelersName,
h.CreatedDate,
h.[Status] ,
Comments,
DepartureDate.DepartureDate,
ArrivalDate.ArrivalDate

 FROM TECLatest h INNER JOIN
      TA.TravelAuthorization ta ON h.TravelAuthorizationID = ta.TravelAuthorizationID OUTER APPLY
	 (SELECT DepartureDate = MIN([FromLocationDate]) FROM [TA].[TravelItinerary] WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber) DepartureDate OUTER APPLY
	 (SELECT ArrivalDate = MAX([FromLocationDate]) FROM [TA].[TravelItinerary] WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber) ArrivalDate
WHERE ta.isDeleted = 0 


