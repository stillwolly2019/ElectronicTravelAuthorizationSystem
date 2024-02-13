CREATE PROCEDURE [Rpt].[PendingTA]

AS

SELECT DISTINCT
ta.[TravelAuthorizationID],
ta.[TravelAuthorizationNumber],
ta.[TravelersName],
LatestStatus.[Description] [Status],
Dep.DepartureDate,
u.FirstName Name,
ta.UserID

      
FROM 
TA.TravelAuthorization ta
CROSS APPLY (SELECT TOP (1) l.[Description] FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=ta.TravelAuthorizationID ORDER BY h.CreatedDate DESC) LatestStatus
CROSS APPLY (SELECT TOP (1) DepartureDate=FromLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=ta.TravelAuthorizationNumber AND isDeleted = 0 ORDER BY CreatedDate ASC) Dep
INNER JOIN sec.Users u ON u.UserID = ta.UserID 

WHERE 
ta.isDeleted=0 AND 
LatestStatus.[Description] = 'TA Pending' AND
Dep.DepartureDate < DATEADD(d,-1,GETDATE())

ORDER BY Dep.DepartureDate 

IF @@ROWCOUNT = 0 RAISERROR('No data', 16, 1)