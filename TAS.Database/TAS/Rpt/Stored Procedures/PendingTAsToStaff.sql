CREATE PROCEDURE [Rpt].[PendingTAsToStaff]

@UserID nvarchar(50)

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
  


SELECT h.[TravelAuthorizationID],u.Username,u.Email,ta.TravelAuthorizationNumber, ta.TravelersName,h.CreatedDate ,h.RejectionReasons,h.[Status],DepartureDate = Itinerary.FromLocationDate,ArrivalDate = Itinerary.ToLocationDate
FROM TALatest h INNER JOIN
     TA.TravelAuthorization ta ON h.TravelAuthorizationID = ta.TravelAuthorizationID INNER JOIN
	 sec.Users u ON u.UserID = ta.UserID CROSS APPLY
	 (SELECT TravelAuthorizationNumber,ToLocationDate=MAX(ToLocationDate),FromLocationDate=MIN(FromLocationDate) FROM TA.TravelItinerary WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber GROUP BY TravelAuthorizationNumber) Itinerary 

WHERE ta.isDeleted = 0 AND 
	  DATEDIFF(d,Itinerary.ToLocationDate,GETDATE()) >=21 AND
	  [Status] = 'Pending'  AND
	  @UserID = u.UserID
