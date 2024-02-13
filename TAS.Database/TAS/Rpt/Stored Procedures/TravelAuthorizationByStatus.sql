CREATE PROCEDURE [Rpt].[TravelAuthorizationByStatus]

AS

WITH Total AS (SELECT Total = COUNT(TravelAuthorizationID), [Status] = 'Total',Sort=1 FROM TA.TravelAuthorization WHERE isDeleted = 0),
     Pending AS (SELECT Total = COUNT(t.TravelAuthorizationID), [Status] = 'Pending',Sort=2 FROM TA.TravelAuthorization t LEFT JOIN Lkp.Lookups l ON l.Code = t.StatusCode WHERE l.[Description]='Pending' AND t.isDeleted = 0),
	 Incomplete AS (SELECT Total = COUNT(t.TravelAuthorizationID), [Status] = 'Incomplete',Sort=3 FROM TA.TravelAuthorization t LEFT JOIN Lkp.Lookups l ON l.Code = t.StatusCode WHERE l.[Description]='Incomplete' AND t.isDeleted = 0),
	 Cancelled AS (SELECT Total = COUNT(t.TravelAuthorizationID), [Status] = 'Cancelled',Sort=4 FROM TA.TravelAuthorization t LEFT JOIN Lkp.Lookups l ON l.Code = t.StatusCode WHERE l.[Description]='Cancelled' AND t.isDeleted = 0),
	 Submitted AS (SELECT Total = COUNT(t.TravelAuthorizationID), [Status] = 'Submitted',Sort=5 FROM TA.TravelAuthorization t LEFT JOIN Lkp.Lookups l ON l.Code = t.StatusCode WHERE l.[Description]='Submitted' AND t.isDeleted = 0),
	 Completed AS (SELECT Total = COUNT(t.TravelAuthorizationID), [Status] = 'Completed',Sort=6 FROM TA.TravelAuthorization t LEFT JOIN Lkp.Lookups l ON l.Code = t.StatusCode WHERE l.[Description]='Completed' AND t.isDeleted = 0)

SELECT * FROM Total
UNION 
SELECT * FROM Pending
UNION 
SELECT * FROM Incomplete
UNION 
SELECT * FROM Cancelled
UNION 
SELECT * FROM Submitted
UNION 
SELECT * FROM Completed


ORDER BY Sort