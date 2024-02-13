create PROCEDURE [Rpt].[ReminderTECPendingForMonth]

@UserID nvarchar(50)


AS

SELECT 
[TravelAuthorizationID],
[TravelAuthorizationNumber],
[TravelersName],
LatestStatus.[Description] 'Status',
Arr.ArrivalDate,
u.FirstName  'Name',
u.Email,
ta.UserID

      
FROM 
TA.TravelAuthorization ta
CROSS APPLY (SELECT TOP 1 l.[Description] FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=ta.TravelAuthorizationID ORDER BY h.CreatedDate DESC) LatestStatus
OUTER APPLY (SELECT TOP 1 ArrivalDate=ToLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=ta.TravelAuthorizationNumber ORDER BY CreatedDate DESC) Arr
INNER JOIN sec.Users u ON u.UserID = ta.UserID 

WHERE 
ta.isDeleted=0 AND 
LatestStatus.[Description] = 'TA Documents Complete' AND
DATEDIFF(d,Arr.ArrivalDate,GETDATE()) = 31 AND
@UserID = u.UserID
      