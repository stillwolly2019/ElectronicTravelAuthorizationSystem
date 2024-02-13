-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE Rpt.EmployeeDashboardTA 
	
	@UserID nvarchar(50)

AS
BEGIN
	
	WITH LatestTAs AS(
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
LatestStatus.[Description] LIKE '%TA%' and ta.userID=@UserID )

,TADocumentsSubmitted AS(SELECT Total = COUNT([TravelAuthorizationID]),Status = 'Submitted' , Sort = 1 FROM LatestTAs WHERE [Status] = 'TA Documents Submitted')
,TAPending AS(SELECT Total = COUNT([TravelAuthorizationID]),Status = 'Pending' , Sort = 2 FROM LatestTAs WHERE [Status] = 'TA Pending')      
,TADocumentsIncomplete AS(SELECT Total = COUNT([TravelAuthorizationID]),Status = 'Incomplete' , Sort = 3 FROM LatestTAs WHERE [Status] = 'TA Documents Incomplete')
,TADocumentsComplete AS(SELECT Total = COUNT([TravelAuthorizationID]),Status = 'Complete' , Sort = 4 FROM LatestTAs WHERE [Status] = 'TA Documents Complete')
,TACanceled AS(SELECT Total = COUNT([TravelAuthorizationID]),Status = 'Canceled' , Sort = 5 FROM LatestTAs WHERE [Status] = 'TA Canceled')


SELECT * FROM TADocumentsSubmitted
UNION
SELECT * FROM TAPending
UNION 
SELECT * FROM TADocumentsIncomplete
UNION
SELECT * FROM TADocumentsComplete
UNION
SELECT * FROM TACanceled

ORDER BY SORT




END
