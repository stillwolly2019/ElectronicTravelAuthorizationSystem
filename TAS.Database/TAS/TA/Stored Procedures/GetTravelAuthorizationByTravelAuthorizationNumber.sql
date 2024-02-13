

-- Procedure
CREATE PROCEDURE [TA].[GetTravelAuthorizationByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(100)
AS
BEGIN

	SELECT 
	TravelAuthorizationID , TA.TravelAuthorization .TravelAuthorizationNumber,TA.TravelAuthorization.UserID,TravelersName,PurposeOfTravel, TripSchemaCode,ModeOfTravelCode,SecurityClearance,
	SecurityTraining, TAStatus.StatusCode,CreatedDate,TA.TravelAuthorization.CreatedBy,UpdatedDate, TA.TravelAuthorization.FirstName AS TravelerFirstName, TA.TravelAuthorization.LastName AS TravelerLastName,
	UpdatedBy, TA.TravelAuthorization .isDeleted,CityOfAccommodation,IsPrivateStay,PrivateStayDates,IsPrivateDeviation,PrivateDeviationLegs,IsAccommodationProvided, 
	AccommodationDetails,IsTravelAdvanceRequested,TravelAdvanceCurrency,TravelAdvanceAmount,TravelAdvanceMethod,IsVisaObtained,	VisaIssued,
	IsVaccinationObtained,IsSecurityClearanceRequestedByMission,TAConfirm,
	 
	 CASE WHEN TA.TravelAuthorization.SecurityClearance = 1 THEN 'YES' ELSE 'NO' END AS SecurityClearanceNeeded
	,CASE WHEN TA.TravelAuthorization.SecurityTraining = 1 THEN 'YES' ELSE 'NO' END AS SecurityTrainingCompleted,
	MOT.Description AS ModeOfTravel, TS.Description AS TravelSchema, Sec.Users.FirstName, Sec.Users.LastName
	FROM 
		TA.TravelAuthorization 
		INNER JOIN Lkp.Lookups AS MOT ON TA.TravelAuthorization.ModeOfTravelCode = MOT.LookupsID 
		INNER JOIN Lkp.Lookups AS TS ON TA.TravelAuthorization.TripSchemaCode = TS.LookupsID
		INNER JOIN Sec.Users ON TA.TravelAuthorization.UserID = Sec.Users.UserID
		CROSS APPLY (SELECT TOP 1 StatusCode FROM TA.StatusChangeHistory WHERE TravelAuthorizationID = TA.TravelAuthorization.TravelAuthorizationID  ORDER BY CreatedDate DESC) TAStatus
	WHERE 
		TA.TravelAuthorization.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TA.TravelAuthorization.isDeleted = 0 
END
