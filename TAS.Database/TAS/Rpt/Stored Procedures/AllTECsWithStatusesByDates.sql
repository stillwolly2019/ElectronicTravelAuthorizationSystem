CREATE PROCEDURE [Rpt].[AllTECsWithStatusesByDates]
@TravelAuthorizationNo nvarchar(500),
@Status nvarchar(50),
@DateFrom date,
@DateTo date

AS 

SELECT ta.[TravelAuthorizationID],ta.TravelAuthorizationNumber,ta.TravelersName,
LatestStatus.CreatedDate,
LatestStatus.[Description] 'Status' ,
LatestStatus.RejectionReasons,DepartureDate = Arr.DepartureDate,ArrivalDate = Arr.ArrivalDate

FROM 
TA.TravelAuthorization ta
CROSS APPLY (SELECT TOP 1 l.[Description], h.CreatedDate,h.RejectionReasons FROM TA.StatusChangeHistory h INNER JOIN Lkp.Lookups l ON l.Code = h.StatusCode WHERE h.TravelAuthorizationID=ta.TravelAuthorizationID AND l.IsDeleted = 0 ORDER BY h.CreatedDate DESC) LatestStatus
OUTER APPLY (SELECT TOP 1 ArrivalDate=ToLocationDate, DepartureDate= FromLocationDate FROM TA.TravelItinerary WHERE TravelAuthorizationNumber=ta.TravelAuthorizationNumber ORDER BY CreatedDate DESC) Arr
INNER JOIN sec.Users u ON u.UserID = ta.UserID 


WHERE ta.isDeleted = 0 AND
LatestStatus.[Description] LIKE '%TEC%' AND
(@TravelAuthorizationNo = ta.TravelAuthorizationNumber OR @TravelAuthorizationNo IS NULL) AND
(@Status = LatestStatus.[Description] OR @Status IS NULL) AND
CONVERT(DATE, LatestStatus.CreatedDate) >= CONVERT(DATE, ISNULL(@DateFrom,LatestStatus.CreatedDate))
		AND CONVERT(DATE, LatestStatus.CreatedDate) <= CONVERT(DATE, ISNULL(@DateTo,LatestStatus.CreatedDate))

ORDER BY ta.TravelAuthorizationNumber