CREATE PROCEDURE [Rpt].[TECList]

@CreatedDateFrom date,
@CreatedDateTo date

AS

WITH DSA AS (
SELECT TravelAuthorizationNumber,	
	  [Amount USD] = SUM(DSA.RateAmount),
	  [Amount JOD] = SUM(DSA.LocalAmount),
	  [No. Of Days] = SUM(DSA.NoOfDays)

FROM 
     TEC.TECItinerary TEC LEFT OUTER JOIN
     TEC.TECItineraryDSA DSA ON DSA.TECItineraryID = TEC.TECItineraryID AND dsa.isDeleted = 0 

WHERE  TEC.isDeleted = 0
 
 GROUP BY TravelAuthorizationNumber),

Mode
AS (
SELECT DISTINCT  ta.TravelAuthorizationNumber, 
REPLACE( REPLACE (Mode,'<Description>',''),'</Description>',',') as Mode
    FROM   TA.TravelAuthorization ta
	 
	 
    OUTER APPLY ( 
         SELECT * FROM (SELECT DISTINCT [Description] FROM TA.TravelItinerary t
		 INNER JOIN Lkp.Lookups  ON LookupsID = t.ModeOfTravelID 
		 WHERE t.TravelAuthorizationNumber = ta.TravelAuthorizationNumber ) A
		
               
FOR XML Path(''))D (Mode )
)


SELECT DISTINCT
ta.TravelAuthorizationID,
ta.TravelAuthorizationNumber,
ta.CreatedDate,
TravelersName,
s.[Description] 'TEC Status',
SUBSTRING(mode.Mode, 1, (LEN(mode.Mode) - 1)) 'Mode of Travel',
DepInfo.CityDescription 'Departure Location',
DepInfo.FromLocationDate 'Departure Date',
CONVERT(VARCHAR(5),DepInfo.FromLocationTime,108) 'Departure Local Time',
FromDateTime,
ArrInfo.CityDescription 'Arrival Location',
ArrInfo.ToLocationDate 'Arrival Date',
CONVERT(VARCHAR(5),ArrInfo.ToLocationTime,108) 'Arrival Local Time',
ToDateTime,
DSA.[No. Of Days],
DSA.[Amount USD] 'Amount(USD)',
CONVERT(VARCHAR,DATEDIFF(HOUR, FromDateTime, ToDateTime)) +'.'+ CONVERT(VARCHAR,DATEDIFF(mi, FromDateTime, ToDateTime)%60)  'Total No of Hours'


FROM TA.TravelAuthorization ta 
     INNER JOIN TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber AND t.isDeleted=0
	 CROSS APPLY (SELECT [Description] FROM Lkp.Lookups WHERE LookupsID = t.ModeOfTravelID AND Lkp.Lookups.IsDeleted=0) tmode
	 CROSS APPLY (SELECT TOP 1 StatusCode FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = ta.TravelAuthorizationID ORDER BY CreatedDate DESC) LatestStatus
	 CROSS APPLY (SELECT [Description] FROM Lkp.Lookups WHERE Code = LatestStatus.StatusCode AND [Description] LIKE '%TEC%' AND Lkp.Lookups.isDeleted=0) s
	 CROSS APPLY (SELECT Fromloc.CityDescription, FromLocationDate, FromLocationTime, CAST(FromLocationDate AS DATETIME) + CAST(FromLocationTime AS DATETIME) FromDateTime
	              FROM TA.TravelItinerary INNER JOIN
				        Lookups.Lkp.City Fromloc ON Fromloc.CityID = FromLocationCode
				  WHERE TravelAuthorizationNumber = t.TravelAuthorizationNumber AND Ordering=1 AND TA.TravelItinerary.isDeleted=0) DepInfo
	 CROSS APPLY (SELECT TOP 1 Toloc.CityDescription, ToLocationDate, ToLocationTime, CAST(ToLocationDate AS DATETIME) + CAST(ToLocationTime AS DATETIME) ToDateTime  
	              FROM TA.TravelItinerary INNER JOIN
				        Lookups.Lkp.City Toloc ON Toloc.CityID = ToLocationCode
				  WHERE TravelAuthorizationNumber = t.TravelAuthorizationNumber AND TA.TravelItinerary.isDeleted=0 ORDER BY Ordering DESC) ArrInfo
	 INNER JOIN TEC.TECItinerary TEC ON t.TravelItineraryID = TEC.TravelItineraryID AND TEC.isDeleted = 0  
	 LEFT JOIN DSA ON DSA.TravelAuthorizationNumber = ta.TravelAuthorizationNumber
	 LEFT JOIN Mode ON Mode.TravelAuthorizationNumber = t.TravelAuthorizationNumber 
	   

WHERE ta.isDeleted = 0 AND
	  CONVERT(DATE, ta.CreatedDate) >= CONVERT(DATE, ISNULL(@CreatedDateFrom,ta.CreatedDate)) AND 
	  CONVERT(DATE, ta.CreatedDate) <= CONVERT(DATE, ISNULL(@CreatedDateTo,ta.CreatedDate))

ORDER BY TravelAuthorizationNumber