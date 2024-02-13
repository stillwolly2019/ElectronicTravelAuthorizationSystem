
CREATE PROCEDURE [TEC].[GetTECItineraryByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(100)
AS
BEGIN
	
IF OBJECT_ID('tempdb.dbo.#TT', 'U') IS NOT NULL
DROP TABLE #TT; 

SELECT TOP 1 * INTO #TT FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = (SELECT TravelAuthorizationID FROM TA.TravelAuthorization WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber) ORDER BY CreatedDate DESC

	SELECT 
	TAItinerary.TravelItineraryID 
	, 'Dep.' AS DepArr
	,TAItinerary.TravelAuthorizationNumber,REPLACE(CONVERT(nvarchar(11)
	,TAItinerary.FromLocationDate,106),' ','-') AS FromLocationDate 
	,CONVERT(VARCHAR(5), TAItinerary.FromLocationTime, 8) AS FromLocationTime
	,CityFrom.CityDescription AS FromCity
	,MOT.Description AS ModeOfTravelName, 0 AS NoOfDays, 0 AS DSARate, '0' as AllDSARates
	,0 AS RateAmount
	,0 AS LocalAmount
	,0 AS NoOfKms
	,ISNULL(#TT.StatusCode,'PEN') AS StatusCode
	,statusCode.LookupsID as StatusId
	,CONVERT(datetime,TAItinerary.FromLocationDate,103)
	,ISNULL(TEC.TECItinerary.TECItineraryID,'00000000-0000-0000-0000-000000000000') AS TECItineraryID
	,TAItinerary.Ordering
	,ISNULL(TEC.TECItinerary.ExchangeRate,0) AS ExchangeRate

	FROM [TA].[TravelItinerary] TAItinerary

	INNER JOIN Lookups.Lkp.City CityFrom ON CityFrom.CityID = TAItinerary.FromLocationCode
	INNER JOIN Lookups.Lkp.City CityTo ON CityTo.CityID = TAItinerary.ToLocationCode
	INNER JOIN Lkp.Lookups MOT ON MOT.LookupsID = TAItinerary.ModeOfTravelID
	LEFT OUTER JOIN TEC.TECItinerary ON TAItinerary.TravelItineraryID = TEC.TECItinerary.TravelItineraryID
	LEFT OUTER JOIN	#TT ON (SELECT TravelAuthorizationID FROM TA.TravelAuthorization WHERE TravelAuthorizationNumber = TAItinerary.TravelAuthorizationNumber) = #TT.TravelAuthorizationID 
	OUTER APPLY (select LookupsID from lkp.Lookups where Code=TEC.TECItinerary.StatusCode  and isDeleted=0 and LookupGroupID='4C9BDCF2-2FCC-432C-9D02-B509299F0193') statusCode

	WHERE 
	TAItinerary.TravelAuthorizationNumber = @TravelAuthorizationNumber AND TAItinerary.isDeleted = 0

	UNION ALL

	SELECT 
	TAItinerary.TravelItineraryID
	,'Arr.' AS DepArr
	,TAItinerary.TravelAuthorizationNumber
	,REPLACE(CONVERT(nvarchar(11), TAItinerary.ToLocationDate,106),' ','-') FromLocationDate
	,CONVERT(VARCHAR(5), TAItinerary.ToLocationTime, 8) AS FromLocationTime
	,CityTo.CityDescription AS FromCity
	,MOT.Description AS ModeOfTravelName
	,ISNULL(TECiDSA.NoOfNights,0.0) AS NoOfDays
	,ISNULL(TEC.TECItinerary.DSARate,0.0) AS DSARate
	,ISNULL(AllDSARates,ISNULL(TEC.TECItinerary.DSARate,0.0)) as AllDSARates
	,ISNULL(TEC.TECItinerary.RateAmount,0.0) AS RateAmount
	,ISNULL(TEC.TECItinerary.LocalAmount,0.0) AS LocalAmount
	,ISNULL(TEC.TECItinerary.NoOfKms,0.0) AS NoOfKms
	,ISNULL(TEC.TECItinerary.StatusCode,'PEN') AS StatusCode
	,statusCode.LookupsID as StatusId
	,CONVERT(datetime,TAItinerary.FromLocationDate,103)
	,ISNULL(TEC.TECItinerary.TECItineraryID,'00000000-0000-0000-0000-000000000000') AS TECItineraryID
	,TAItinerary.Ordering
	,ISNULL(TEC.TECItinerary.ExchangeRate,0) AS ExchangeRate

	FROM [TA].[TravelItinerary] TAItinerary

	INNER JOIN Lookups.Lkp.City CityFrom ON CityFrom.CityID = TAItinerary.FromLocationCode
	INNER JOIN Lookups.Lkp.City CityTo ON CityTo.CityID = TAItinerary.ToLocationCode
	INNER JOIN Lkp.Lookups MOT ON MOT.LookupsID = TAItinerary.ModeOfTravelID
	LEFT OUTER JOIN TEC.TECItinerary ON TAItinerary.TravelItineraryID = TEC.TECItinerary.TravelItineraryID
	LEFT OUTER JOIN	#TT ON (SELECT TravelAuthorizationID FROM TA.TravelAuthorization WHERE TravelAuthorizationNumber = TAItinerary.TravelAuthorizationNumber) = #TT.TravelAuthorizationID
	OUTER APPLY (select LookupsID from lkp.Lookups where Code=TEC.TECItinerary.StatusCode  and isDeleted=0 and LookupGroupID='4C9BDCF2-2FCC-432C-9D02-B509299F0193') statusCode
	OUTER APPLY (SELECT Sum(TECDSA.NoOfDays * (TECDSA.Percentage / 100)) AS NoOfNights FROM [TEC].[TECItineraryDSA] TECDSA WHERE TECDSA.TECItineraryID = TEC.TECItinerary.TECItineraryID AND TECDSA.isDeleted=0) TECiDSA
	
	WHERE 
	TAItinerary.TravelAuthorizationNumber = @TravelAuthorizationNumber
	AND TAItinerary.isDeleted = 0
	ORDER BY TAItinerary.Ordering, CONVERT(datetime,TAItinerary.FromLocationDate,103) ASC, DepArr DESC, FromLocationTime ASC
		
IF OBJECT_ID('tempdb.dbo.#TT', 'U') IS NOT NULL
DROP TABLE #TT; 

END
