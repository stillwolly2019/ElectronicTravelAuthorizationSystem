

CREATE PROCEDURE [Rpt].[TravelAuthorizationForm]

@TravelAuthorizationNumber VARCHAR(MAX)

AS

WITH DatesOfDuty AS
   (SELECT TA.TravelAuthorizationNumber,
      [TravelAuthorizationID] = TA.TravelAuthorizationID,
         [From] = MIN(t.FromLocationDate),[To] = MAX(t.ToLocationDate),
     
         [PrivateStayDatesFrom] = CONVERT(DAte,TA.PrivateStayDateFrom),
         [PrivateStayDatesTo] = CONVERT(DAte,TA.PrivateStayDateTo),
     
         [OfficialFrom] = CASE WHEN PrivateStayDateFrom IS NOT NULL THEN CASE WHEN CONVERT(Date,TA.PrivateStayDateFrom)>MIN(t.FromLocationDate) AND CONVERT(DAte,TA.PrivateStayDateTo)<MAX(t.ToLocationDate) THEN CONVERT(VARCHAR,MIN(t.FromLocationDate))+' - '+CONVERT(VARCHAR,CONVERT(DATE,DATEADD(DAY,-1,TA.PrivateStayDateFrom)))
                              WHEN CONVERT(Date,TA.PrivateStayDateTo) = MAX(t.ToLocationDate) THEN CONVERT(VARCHAR,MIN(t.FromLocationDate) )  
							   ELSE CONVERT(VARCHAR,CONVERT(DATE,DATEADD(DAY,+1,TA.PrivateStayDateTo))) END
                                         ELSE
                                               CONVERT(VARCHAR,MIN(t.FromLocationDate) )  
                                         END,

      [OfficialTo] =  CASE WHEN PrivateStayDateFrom IS NOT NULL THEN CASE WHEN CONVERT(Date,TA.PrivateStayDateFrom)>MIN(t.FromLocationDate) AND CONVERT(DAte,TA.PrivateStayDateTo)<MAX(t.ToLocationDate) THEN CONVERT(VARCHAR,CONVERT(DATE,DATEADD(DAY,+1,TA.PrivateStayDateTo))) +' - ' +CONVERT(VARCHAR,MAX(t.ToLocationDate)) 
                              WHEN CONVERT(Date,TA.PrivateStayDateTo) <> MAX(t.ToLocationDate) THEN CONVERT(VARCHAR,MAX(t.ToLocationDate) ) 
                                            ELSE CONVERT(VARCHAR,CONVERT(DATE,DATEADD(DAY,-1,TA.PrivateStayDateFrom))) END  
                                    ELSE
                                              CONVERT(VARCHAR,MAX(t.ToLocationDate) ) 
                                    END
    FROM TA.TravelAuthorization TA INNER JOIN 
       TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber 
       WHERE t.isDeleted = 0  
	   
               
    GROUP BY TA.TravelAuthorizationID,TA.PrivateStayDateFrom,TA.PrivateStayDateTo,TA.TravelAuthorizationNumber
   )

SELECT DISTINCT
      ------------------------------------------------------------------------TA Details
      [TravelAuthorizationNumber] = TA.TravelAuthorizationNumber,
      [Name] = TA.TravelersName, 
	  [Family Members]=TA.FamilyMembers,
         [Purpose of Travel] = TA.PurposeOfTravel,
         [FromDate] = DatesOfDuty.[OfficialFrom],
         [ToDate] = DatesOfDuty.[OfficialTo],
         ------------------------------------------------------------------------Travel Mode
         [Air] = CASE WHEN mode.[Description] = 'Air' THEN 20 ELSE  40 END,
         [Ferry] = CASE WHEN mode.[Description] = 'Ferry' THEN 20 ELSE  40 END,
         [Ship] = CASE WHEN mode.[Description] = 'Ship' THEN 20 ELSE  40 END,
         [Bus/Train] = CASE WHEN mode.[Description] LIKE '%Train%' OR mode.[Description] LIKE '%Bus%' THEN 20 ELSE  40 END,
         [Car] = CASE WHEN mode.[Description] = 'Car' THEN 20 ELSE  40 END,
         ------------------------------------------------------------------------Trip Schema
         [TDY] = CASE WHEN TripSchema.[Description] LIKE '%TDY%' THEN 20 ELSE  40 END,
         [Evacuation] = CASE WHEN TripSchema.[Description] = 'Evacuation' THEN 20 ELSE  40 END,
         [Escort] = CASE WHEN TripSchema.[Description] = 'Escort' THEN 20 ELSE  40 END,
         [Rest & Recuperation] = CASE WHEN TripSchema.[Description] LIKE '%Rest%' OR TripSchema.[Description] LIKE '%Recuperation%' THEN 20 ELSE  40 END,
         [Home Leave] = CASE WHEN TripSchema.[Description] LIKE '%Home%' THEN 20 ELSE  40 END,
         [Family Travel] = CASE WHEN TripSchema.[Description] LIKE '%Family%' THEN 20 ELSE  40 END,
         [Education Grant] = CASE WHEN TripSchema.[Description] = 'Education' THEN 20 ELSE  40 END,
         [Transfer] = CASE WHEN TripSchema.[Description] = 'Transfer' THEN 20 ELSE  40 END,
         [Appointment] = CASE WHEN TripSchema.[Description] = 'Appointment' THEN 20 ELSE  40 END,
         [Repatriation - Admin] = CASE WHEN TripSchema.[Description] LIKE '%Admin%' THEN 20 ELSE  40 END,
         [Repatriation - OPS] = CASE WHEN TripSchema.[Description] LIKE '%OPS%' THEN 20 ELSE  40 END,
         [Medical Travel - HI] = CASE WHEN TripSchema.[Description] LIKE '%Medical Travel%' AND TripSchema.[Description] LIKE '%HI%' THEN 20 ELSE  40 END,
         [Medical Travel - MSP] = CASE WHEN TripSchema.[Description] LIKE '%Medical Travel%' AND TripSchema.[Description] LIKE '%MSP%' THEN 20 ELSE  40 END,
         ------------------------------------------------------------------------Trip Schema
         [Security clearance-COM] = CASE WHEN TA.SecurityClearance = 1 THEN 20 ELSE  40 END,
         [Security training-COM] = CASE WHEN TA.SecurityTraining = 1 THEN 20 ELSE  40 END,
         [Security clearance-PEN] = CASE WHEN TA.SecurityClearance = 0 THEN 20 ELSE  40 END,
         [Security training-PEN] = CASE WHEN TA.SecurityTraining = 0 THEN 20 ELSE  40 END ,
         [SecClearanceReqByMission] = CASE WHEN TA.IsSecurityClearanceRequestedByMission = 1 THEN 20 ELSE  40 END,
         [SecClearanceReqByHQ] = CASE WHEN TA.IsSecurityClearanceRequestedByMission = 0 THEN 20 ELSE  40 END,

         [CityOfAccommodation] = TA.CityOfAccommodation,
         [PrivateStay-No] = CASE WHEN TA.IsPrivateStay = 0 THEN 20 ELSE  40 END,
         [PrivateStay-Yes] = CASE WHEN TA.IsPrivateStay = 1 THEN 20 ELSE  40 END,
         [PrivateStayDatesFrom] = TA.PrivateStayDateFrom,
         [PrivateStayDatesTo] = TA.PrivateStayDateTo,
         [PrivateDeviation-No] = CASE WHEN TA.IsPrivateDeviation = 0 THEN 20 ELSE  40 END,
         [PrivateDeviation-Yes] = CASE WHEN TA.IsPrivateDeviation = 1 THEN 20 ELSE  40 END,
         [PrivateDeviationLegs] = TA.PrivateDeviationLegs,
         [AccommodationProvided-No] = CASE WHEN TA.IsAccommodationProvided = 0 THEN 20 ELSE  40 END,
         [AccommodationProvided-Yes] = CASE WHEN TA.IsAccommodationProvided = 1 THEN 20 ELSE  40 END,
         [AccommodationDetails] = TA.AccommodationDetails,

         [TravelAdvanceRequested-No] = CASE WHEN TA.IsTravelAdvanceRequested = 0 THEN 20 ELSE  40 END,
         [TravelAdvanceRequested-Yes] = CASE WHEN TA.IsTravelAdvanceRequested = 1 THEN 20 ELSE  40 END,
         [TravelAdvanceCurrency] = Curr.CurrencySymbol,
         [TravelAdvanceAmount] = TA.TravelAdvanceAmount,
         [TravelAdvMethod-Bank] = CASE WHEN TA.TravelAdvanceMethod Like '%Bank%' THEN 20 ELSE  40 END,
         [TravelAdMethod-Cheque] = CASE WHEN TA.TravelAdvanceMethod Like '%Che%' THEN 20 ELSE  40 END,
         [TravelAdvMethod-Cash] = CASE WHEN TA.TravelAdvanceMethod LIKE '%Cash%' THEN 20 ELSE  40 END,

         [VisaObtained-N/A] = CASE WHEN TA.IsVisaObtained = 0 THEN 20 ELSE  40 END,
         [VisaObtained-No] = CASE WHEN TA.IsVisaObtained = 1 THEN 20 ELSE  40 END,
         [VisaObtained-Yes] = CASE WHEN TA.IsVisaObtained = 2 THEN 20 ELSE  40 END,

         [Vaccination-N/A] = CASE WHEN TA.IsVaccinationObtained = 0 THEN 20 ELSE  40 END,
         [Vaccination-No] = CASE WHEN TA.IsVaccinationObtained = 1 THEN 20 ELSE  40 END,
         [Vaccination-Yes] = CASE WHEN TA.IsVaccinationObtained = 2 THEN 20 ELSE  40 END,

         [VisaIssued] = TA.VisaIssued,
         [Confirmation] = CASE WHEN TA.TAConfirm = 1 THEN 20 ELSE  40 END
         
         
                      
                             

   FROM
       TA.TravelAuthorization TA INNER JOIN  
       TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber LEFT JOIN 
          Lkp.Lookups TripSchema ON TripSchema.LookupsID = TA.TripSchemaCode LEFT JOIN
          TEC.TECItinerary TEC ON TEC.TravelItineraryID = t.TravelItineraryID LEFT JOIN 
       TEC.TECExpenditure Ex ON TA.TravelAuthorizationNumber = Ex.TravelAuthorizationNumber LEFT JOIN 
       TEC.TECAdvances Adv ON TA.TravelAuthorizationNumber = Adv.TravelAuthorizationNumber LEFT JOIN 
          DatesOfDuty ON DatesOfDuty.TravelAuthorizationID = TA.TravelAuthorizationID LEFT JOIN 
          [Lookups].[Lkp].[City] Fromloc ON FromLoc.CityID = t.FromLocationCode LEFT JOIN 
          [Lookups].[Lkp].[City] Toloc ON ToLoc.CityID = t.ToLocationCode LEFT JOIN 
          Lkp.Lookups mode ON mode.LookupsID = TA.ModeOfTravelCode LEFT JOIN
          [Lookups].[Lkp].[Currency] Curr ON Curr.CurrencyID = TA.TravelAdvanceCurrency


    WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber

