CREATE PROCEDURE [Rpt].[TECIncomplete]

@UserID nvarchar(50)

AS 
WITH base as (
SELECT 
TravelAuthorizationNumber,
h.[TravelAuthorizationID],
h.CreatedDate,
[Status] = l.Description,
Comments,
h.CreatedBy
FROM TEC.StatusChangeHistory h INNER JOIN
      TA.TravelAuthorization ta ON h.TravelAuthorizationID = ta.TravelAuthorizationID INNER JOIN
      Lkp.Lookups l ON l.Code = h.StatusCode INNER JOIN
      Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND LookupGroup = 'TEC Status Code') ,
rnk as (select *,RANK() over (partition by TravelAuthorizationNumber order by [CreatedDate] desc) as rnk from base),
TECLatest as (select * from rnk where rnk=1) 
  


SELECT tec.[TravelAuthorizationID],ta.TravelAuthorizationNumber, ta.TravelersName,tec.CreatedDate ,tec.Comments,tec.Status, u.Username, u.Email,DepartureDate = Itinerary.FromLocationDate,ArrivalDate = Itinerary.ToLocationDate
FROM TECLatest tec INNER JOIN
     TA.TravelAuthorization ta ON tec.TravelAuthorizationNumber = ta.TravelAuthorizationNumber LEFT JOIN
	 sec.Users u ON u.UserID = tec.CreatedBy  CROSS APPLY
	 (SELECT TravelAuthorizationNumber,ToLocationDate=MAX(ToLocationDate),FromLocationDate=MIN(FromLocationDate) FROM TA.TravelItinerary WHERE TravelAuthorizationNumber = ta.TravelAuthorizationNumber GROUP BY TravelAuthorizationNumber) Itinerary

WHERE ta.isDeleted = 0 AND 
      [Status] = 'InComplete' AND
	  @UserID = u.UserID

ORDER BY TravelAuthorizationNumber