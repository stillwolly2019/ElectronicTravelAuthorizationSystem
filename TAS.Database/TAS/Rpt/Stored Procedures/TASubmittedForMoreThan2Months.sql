CREATE PROCEDURE [Rpt].[TASubmittedForMoreThan2Months]



AS

WITH base as (
SELECT 
[TravelAuthorizationID],
CreatedDate,
[Status] = l.Description,
RejectionReasons
FROM TA.StatusChangeHistory INNER JOIN
      Lkp.Lookups l ON l.Code = StatusCode INNER JOIN
      Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND LookupGroup = 'TA Status Code') ,
rnk as (select *,RANK() over (partition by [TravelAuthorizationID] order by [CreatedDate] desc) as rnk from base),
TALatest as (select * from rnk where rnk=1) 

SELECT 
ta.TravelAuthorizationNumber,ta.TravelersName, h.[Status], StatusDate = CONVERT(Date,h.CreatedDate), h.RejectionReasons,NoOfDaysSinceLastUpdate = DATEDIFF(d,h.CreatedDate,GETDATE())
FROM ta.TravelAuthorization ta INNER JOIN
     TALatest h ON h.TravelAuthorizationID = ta.TravelAuthorizationID 
WHERE ta.isDeleted = 0  AND h.[Status]='Submitted'AND DATEDIFF(d,h.CreatedDate,GETDATE()) >=61


IF @@ROWCOUNT = 0 RAISERROR('No data', 16, 1)