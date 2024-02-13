
CREATE PROCEDURE [TA].[GetTravelAuthorizationByTravelAuthorizationID]
@TravelAuthorizationID nvarchar(100)
AS
BEGIN

IF OBJECT_ID('tempdb.dbo.#TT', 'U') IS NOT NULL
  DROP TABLE #TT; 

SELECT TOP 1 * INTO #TT FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = @TravelAuthorizationID ORDER BY CreatedDate DESC

SELECT 
	T.TravelAuthorizationID,t.TravelAuthorizationNumber,UserID,TravelersName,FirstName,MiddleName,LastName,PurposeOfTravel,TripSchemaCode,ModeOfTravelCode,
	SecurityClearance,SecurityTraining,#tt.StatusCode,l.[Description] AS TAStatus,CityOfAccommodation,IsPrivateStay,PrivateStayDates,IsPrivateDeviation,
	PrivateDeviationLegs,IsAccommodationProvided,AccommodationDetails,IsTravelAdvanceRequested,TravelAdvanceCurrency,TravelAdvanceAmount,
	TravelAdvanceMethod,IsVisaObtained,VisaIssued,IsVaccinationObtained,IsSecurityClearanceRequestedByMission, ISNULL(TAConfirm,0) as TAConfirm
	,T.CreatedDate,T.CreatedBy,UpdatedDate,UpdatedBy,T.isDeleted,IsTecComplete,
	 REPLACE(CONVERT(nvarchar(11), T.CreatedDate,106),' ','-') AS DateCreated,
	 L.LongDescription, CONVERT(varchar,PrivateStayDateFrom,103) AS PrivateStayDateFrom,
	 CONVERT(varchar,PrivateStayDateTo, 103) AS PrivateStayDateTo, ISNULL(T.IsNotForDSA,0) as IsNotForDSA , ISNULL(ArrivalDate.ToLocationDate,'1900-01-01') as ToLocationDate
	 ,ISNULL(ExpenditureNotApplicable,0) as ExpenditureNotApplicable ,ISNULL(AdvancesNotApplicable,0) as AdvancesNotApplicable,FamilyMembers,DocumentNumber
	
FROM 
	[TA].[TravelAuthorization] T LEFT OUTER JOIN
	#TT ON T.TravelAuthorizationID = #TT.TravelAuthorizationID INNER JOIN
	Lkp.Lookups L ON #TT.StatusCode = L.Code
	OUTER APPLY (SELECT ti.TravelAuthorizationNumber,MIN(ti.FromLocationDate) as  FromLocationDate, MAX(ti.ToLocationDate) AS ToLocationDate FROM Ta.TravelItinerary ti WHERE ti.TravelAuthorizationNumber= t.TravelAuthorizationNumber AND ti.isDeleted = 0 GROUP BY  ti.TravelAuthorizationNumber) ArrivalDate

WHERE 
	T.isDeleted = 0 
	AND
	L.IsDeleted = 0
	AND T.TravelAuthorizationID = @TravelAuthorizationID

END