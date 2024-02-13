﻿CREATE PROCEDURE [Rpt].[TAIncomplete]

@UserID nvarchar(50)

AS 
WITH base as (
SELECT 
TravelAuthorizationNumber,
h.[TravelAuthorizationID],
h.CreatedDate,
[Status] = l.Description,
RejectionReasons,
h.CreatedBy
FROM TA.StatusChangeHistory h INNER JOIN
      TA.TravelAuthorization ta ON h.TravelAuthorizationID = ta.TravelAuthorizationID INNER JOIN
      Lkp.Lookups l ON l.Code = h.StatusCode INNER JOIN
      Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND LookupGroup = 'TA Status Code') ,
rnk as (select *,RANK() over (partition by TravelAuthorizationNumber order by [CreatedDate] desc) as rnk from base),
TALatest as (select * from rnk where rnk=1) 
  


SELECT DISTINCT h.[TravelAuthorizationID],h.TravelAuthorizationID,ta.TravelAuthorizationNumber, ta.TravelersName,h.CreatedDate ,h.RejectionReasons,h.Status, u.Username, u.Email,DepartureDate = Itinerary.FromLocationDate,ArrivalDate = Itinerary.ToLocationDate
FROM TALatest h INNER JOIN
     TA.TravelAuthorization ta ON h.TravelAuthorizationNumber = ta.TravelAuthorizationNumber LEFT JOIN
	 sec.Users u ON u.UserID = h.CreatedBy CROSS APPLY
	 (SELECT TravelAuthorizationNumber,ToLocationDate=MAX(ToLocationDate),FromLocationDate=MIN(FromLocationDate) FROM TA.TravelItinerary WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber GROUP BY TravelAuthorizationNumber) Itinerary 

WHERE ta.isDeleted = 0 AND 
      [Status] = 'Incomplete' AND
	  @UserID = u.UserID

ORDER BY TravelAuthorizationNumber