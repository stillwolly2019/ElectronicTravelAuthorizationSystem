CREATE PROCEDURE [Rpt].[PendingTASubscription]

AS

SELECT DISTINCT

[Subject] = 'ACTION: Travel Authorization with TA Pending Status',
[Comment] = 'Dear Colleague/s

There are ' + CONVERT(VARCHAR,COUNT(DISTINCT ta.[TravelAuthorizationID])) + ' Travel Authorizations with status TA Pending and departure date has passed. Based on SOP, status should have been changed to TA Documents Complete before the Departure Date. 

"4. TA Pending notifications
- This is used to notify Admin of all pending TAs that need to be followed up and changed to TA documents Compete before the departure date."


Please review and change to the correct status.

This is a system generated email
For all inquiries, please send your email to IOM AMMAN Helpdesk AmmanHelpdesk@iom.int

'


FROM 
TA.TravelAuthorization ta
CROSS APPLY (SELECT TOP (1) l.[Description] FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=ta.TravelAuthorizationID ORDER BY h.CreatedDate DESC) LatestStatus
CROSS APPLY (SELECT TOP (1) DepartureDate=FromLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=ta.TravelAuthorizationNumber AND isDeleted = 0 ORDER BY CreatedDate ASC) Dep
INNER JOIN sec.Users u ON u.UserID = ta.UserID 

WHERE 
ta.isDeleted=0 AND 
LatestStatus.[Description] = 'TA Pending' AND
Dep.DepartureDate < DATEADD(d,-1,GETDATE())