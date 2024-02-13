CREATE PROCEDURE [Rpt].[SubscriptionIncompleteTAs]



AS 
WITH base as (
SELECT 
[TravelAuthorizationID],
CreatedDate,
[Status] = l.Description,
RejectionReasons
FROM TA.StatusChangeHistory INNER JOIN
      Lkp.Lookups l ON l.Code = StatusCode INNER JOIN
      Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND LookupGroup = 'TEC Status Code') ,
rnk as (select *,RANK() over (partition by [TravelAuthorizationID] order by [CreatedDate] desc) as rnk from base),
TALatest as (select * from rnk where rnk=1) 
  


SELECT Ta.TravelAuthorizationNumber,u.Username,u.Email,Subject='Incomplete TA',[Status],t.RejectionReasons,ta.UserID,
      Comment='Your TA ( ' + Ta.TravelAuthorizationNumber+ ' ) is incomplete. Please see comments from admin [Comments]. Please submit your TA once requirements are completed.

 

Best regards,

TA Admin

This is a system generated report
'
FROM TALatest t INNER JOIN
     TA.TravelAuthorization ta ON t.TravelAuthorizationID = ta.TravelAuthorizationID INNER JOIN
	 sec.Users u ON u.UserID = ta.UserID CROSS APPLY
	 (SELECT TravelAuthorizationNumber,ToLocationDate=MAX(ToLocationDate),FromLocationDate=MIN(FromLocationDate) FROM TA.TravelItinerary WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber GROUP BY TravelAuthorizationNumber) Itinerary 

WHERE ta.isDeleted = 0 AND 
	  [Status] = 'Incomplete' 
