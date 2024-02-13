
-- Procedure
CREATE PROCEDURE [TA].[GetTravelAuthorizationByTravelAuthorizationNumber_Old]
@TravelAuthorizationNumber nvarchar(100)
AS
BEGIN

Declare @TAravelAuthorizationId  nvarchar(max)
SELECT @TAravelAuthorizationId=TravelAuthorizationID  FROM TA.TravelAuthorization 
	WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber


	SELECT 
	TravelAuthorizationID , TA.TravelAuthorization .TravelAuthorizationNumber,TA.TravelAuthorization.UserID,TravelersName,PurposeOfTravel, TripSchemaCode,ModeOfTravelCode,SecurityClearance,
	SecurityTraining,(SELECT TOP 1 StatusCode FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = @TAravelAuthorizationId  ORDER BY CreatedDate DESC)as StatusCode
	,CreatedDate,TA.TravelAuthorization.CreatedBy,UpdatedDate, TA.TravelAuthorization.FirstName AS TravelerFirstName, TA.TravelAuthorization.LastName AS TravelerLastName,
	UpdatedBy, TA.TravelAuthorization .isDeleted,CityOfAccommodation,IsPrivateStay,PrivateStayDates,IsPrivateDeviation,PrivateDeviationLegs,IsAccommodationProvided, 
	AccommodationDetails,IsTravelAdvanceRequested,TravelAdvanceCurrency,TravelAdvanceAmount,TravelAdvanceMethod,IsVisaObtained,	VisaIssued,
	IsVaccinationObtained,IsSecurityClearanceRequestedByMission,TAConfirm,
	 
	 CASE WHEN TA.TravelAuthorization.SecurityClearance = 1 THEN 'YES' ELSE 'NO' END AS SecurityClearanceNeeded
	, CASE WHEN TA.TravelAuthorization.SecurityTraining = 1 THEN 'YES' ELSE 'NO' END AS SecurityTrainingCompleted,
	MOT.Description AS ModeOfTravel, TS.Description AS TravelSchema, Sec.Users.FirstName, Sec.Users.LastName
	FROM 
		TA.TravelAuthorization 
		INNER JOIN Lkp.Lookups AS MOT ON TA.TravelAuthorization.ModeOfTravelCode = MOT.LookupsID 
		INNER JOIN Lkp.Lookups AS TS ON TA.TravelAuthorization.TripSchemaCode = TS.LookupsID
		INNER JOIN Sec.Users ON TA.TravelAuthorization.UserID = Sec.Users.UserID
	WHERE 
		TA.TravelAuthorization.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TA.TravelAuthorization.isDeleted = 0 




	--------------------------------------------
	
IF OBJECT_ID('tempdb.dbo.#TT', 'U') IS NOT NULL
  DROP TABLE #TT; 

SELECT TOP 1 * INTO #TT FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = 
(SELECT TravelAuthorizationID FROM TA.TravelAuthorization WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber) ORDER BY CreatedDate DESC

	SELECT TAItinerary.TravelItineraryID , 'Dep.' AS DepArr,
	 TAItinerary.TravelAuthorizationNumber,REPLACE(CONVERT(nvarchar(11), TAItinerary.FromLocationDate,106),' ','-') AS FromLocationDate ,
	 CONVERT(VARCHAR(5), TAItinerary.FromLocationTime, 8) AS FromLocationTime,
	CityFrom.CityDescription AS FromCity, MOT.Description AS ModeOfTravelName, 0 AS NoOfDays, 0 AS DSARate, '0' as AllDSARates,
	0 AS RateAmount, 0 AS LocalAmount, 0 AS NoOfKms, ISNULL(#TT.StatusCode,'PEN') AS StatusCode,statusCode.LookupsID as StatusId,
	CONVERT(datetime,TAItinerary.FromLocationDate,103), ISNULL(TEC.TECItinerary.TECItineraryID,'00000000-0000-0000-0000-000000000000') AS TECItineraryID,
	TAItinerary.Ordering
	FROM 
		[TA].[TravelItinerary] TAItinerary
		INNER JOIN Lookups.Lkp.City CityFrom ON CityFrom.CityID = TAItinerary.FromLocationCode
		INNER JOIN Lookups.Lkp.City CityTo ON CityTo.CityID = TAItinerary.ToLocationCode
		INNER JOIN Lkp.Lookups MOT ON MOT.LookupsID = TAItinerary.ModeOfTravelID
		LEFT OUTER JOIN TEC.TECItinerary ON TAItinerary.TravelItineraryID = TEC.TECItinerary.TravelItineraryID
		--left outer join lkp.Lookups statusCode on statusCode.Code=TEC.TECItinerary.StatusCode
		LEFT OUTER JOIN	#TT ON (SELECT TravelAuthorizationID FROM TA.TravelAuthorization WHERE TravelAuthorizationNumber = TAItinerary.TravelAuthorizationNumber) = #TT.TravelAuthorizationID
cross apply (
	select LookupsID from lkp.Lookups where Code=TEC.TECItinerary.StatusCode  and isDeleted=0 
	and LookupGroupID='4C9BDCF2-2FCC-432C-9D02-B509299F0193'
	
	) statusCode

	WHERE 
		TAItinerary.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TAItinerary.isDeleted = 0
	UNION ALL
	SELECT TAItinerary.TravelItineraryID, 'Arr.' AS DepArr,
	TAItinerary.TravelAuthorizationNumber,REPLACE(CONVERT(nvarchar(11), TAItinerary.ToLocationDate,106),' ','-') FromLocationDate,
	CONVERT(VARCHAR(5), TAItinerary.ToLocationTime, 8) AS FromLocationTime,
	 CityTo.CityDescription AS FromCity, MOT.Description AS ModeOfTravelName, ISNULL(TECiDSA.NoOfNights,0.0) AS NoOfDays,
	 ISNULL(TEC.TECItinerary.DSARate,0.0) AS DSARate, 
	 ISNULL(AllDSARates,ISNULL(TEC.TECItinerary.DSARate,0.0)) as AllDSARates,
	 ISNULL(TEC.TECItinerary.RateAmount,0.0) AS RateAmount,
	 ISNULL(TEC.TECItinerary.LocalAmount,0.0) AS LocalAmount, ISNULL(TEC.TECItinerary.NoOfKms,0.0) AS NoOfKms,
	 ISNULL(TEC.TECItinerary.StatusCode,'PEN') AS StatusCode,statusCode.LookupsID as StatusId,
	 CONVERT(datetime,TAItinerary.FromLocationDate,103),
	 ISNULL(TEC.TECItinerary.TECItineraryID,'00000000-0000-0000-0000-000000000000') AS TECItineraryID,TAItinerary.Ordering
	FROM 
		[TA].[TravelItinerary] TAItinerary
		INNER JOIN Lookups.Lkp.City CityFrom ON CityFrom.CityID = TAItinerary.FromLocationCode
		INNER JOIN Lookups.Lkp.City CityTo ON CityTo.CityID = TAItinerary.ToLocationCode
		INNER JOIN Lkp.Lookups MOT ON MOT.LookupsID = TAItinerary.ModeOfTravelID
		LEFT OUTER JOIN TEC.TECItinerary ON TAItinerary.TravelItineraryID = TEC.TECItinerary.TravelItineraryID
		--left outer join lkp.Lookups statusCode on statusCode.Code=TEC.TECItinerary.StatusCode 
		LEFT OUTER JOIN	#TT ON 
		(SELECT TravelAuthorizationID FROM TA.TravelAuthorization 
		WHERE TravelAuthorizationNumber = TAItinerary.TravelAuthorizationNumber) = #TT.TravelAuthorizationID
	cross apply (
	select LookupsID from lkp.Lookups where Code=TEC.TECItinerary.StatusCode  and isDeleted=0 
	and LookupGroupID='4C9BDCF2-2FCC-432C-9D02-B509299F0193'
	) statusCode
	OUTER APPLY (
	SELECT Sum(TECDSA.NoOfDays * (TECDSA.Percentage / 100)) AS NoOfNights FROM [TEC].[TECItineraryDSA] TECDSA WHERE TECDSA.TECItineraryID = TEC.TECItinerary.TECItineraryID
	AND TECDSA.isDeleted=0
	) TECiDSA
	WHERE 
		TAItinerary.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TAItinerary.isDeleted = 0
	ORDER BY TAItinerary.Ordering, CONVERT(datetime,TAItinerary.FromLocationDate,103) ASC, DepArr DESC, FromLocationTime ASC
---------------------------------------------------------
	SELECT * FROM [TA].[WBS] 
	WHERE 
		TravelAuthorizationID = (
									SELECT TravelAuthorizationID 
									FROM TA.TravelAuthorization 
									WHERE 
										TA.TravelAuthorization.TravelAuthorizationNumber = @TravelAuthorizationNumber 
										AND isDeleted = 0
								) 
		AND isDeleted = 0
---------------------------------------------------------

	SELECT	
		TEC.TECExpenditure.TECExpenditureID, TEC.TECExpenditure.TravelAuthorizationNumber, REPLACE(CONVERT(nvarchar(11), TEC.TECExpenditure.ExpenditureDate,106),' ','-') AS ExpenditureDate, 
		TEC.TECExpenditure.ExpenseAmount, ISNULL(TEC.TECExpenditure.Rate,0) AS Rate, ISNULL(TEC.TECExpenditure.RateAmount,0) AS RateAmount, 
		ISNULL(TEC.TECExpenditure.LocalAmount,0) AS LocalAmount, LkpCurr.CurrencyName, TEC.TECExpenditure.CurrencyID, TEC.TECExpenditure.TECExpenditureID, TEC.TECExpenditure.ExpenditureDetails
	FROM    
		TEC.TECExpenditure 

		INNER JOIN Lookups.Lkp.Currency LkpCurr ON TEC.TECExpenditure.CurrencyID = LkpCurr.CurrencyID
	WHERE
		TEC.TECExpenditure.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TEC.TECExpenditure.isDeleted = 0
				

---------------------------------------------------------

	SELECT	
		TECAdvancesID,TravelAuthorizationNumber, PayOfficeCode, PayOfficeCodeID ,POC.[Description] AS  PayOfficeCodeDesc,REPLACE(CONVERT(nvarchar(11), DatePaid,106),' ','-') AS DatePaid, TEC.TECAdvances.CurrencyID, 
		ISNULL(AdvanceAmount,0) AS AdvanceAmount, ISNULL(Rate,0) AS Rate, ISNULL(RateAmount,0) AS RateAmount, ISNULL(LocalAmount,0) AS LocalAmount,
		LkpCurr.CurrencyName
	FROM
		TEC.TECAdvances
		INNER JOIN Lookups.Lkp.Currency LkpCurr ON TEC.TECAdvances.CurrencyID = LkpCurr.CurrencyID
		INNER JOIN Lkp.Lookups POC ON POC.LookupsID = TEC.TECAdvances.PayOfficeCodeID
	WHERE
		TEC.TECAdvances.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TEC.TECAdvances.isDeleted = 0
END

