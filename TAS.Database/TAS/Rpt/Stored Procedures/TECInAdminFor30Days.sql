CREATE PROCEDURE [Rpt].[TECInAdminFor30Days]



AS

SELECT 
[TravelAuthorizationNumber],
[TravelersName],
Arr.ArrivalDate,
LatestStatus.[Description] 'Status',
LatestStatus.CreatedDate 'Status Date',
DATEDIFF(d,LatestStatus.CreatedDate,GETDATE()) 'No Of Days on the Status'



FROM 
TA.TravelAuthorization ta
CROSS APPLY (SELECT TOP 1 l.[Description],h.CreatedDate FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=ta.TravelAuthorizationID ORDER BY h.CreatedDate DESC) LatestStatus
OUTER APPLY (SELECT TOP 1 ArrivalDate=ToLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=ta.TravelAuthorizationNumber ORDER BY CreatedDate DESC) Arr

WHERE 
ta.isDeleted=0 AND 
LatestStatus.[Description] IN ('TEC Documents Incomplete','TEC Returned to Admin') AND
DATEDIFF(d,LatestStatus.CreatedDate,GETDATE()) = 30 

ORDER BY [TravelAuthorizationNumber]

IF @@ROWCOUNT = 0 RAISERROR('No data', 16, 1)