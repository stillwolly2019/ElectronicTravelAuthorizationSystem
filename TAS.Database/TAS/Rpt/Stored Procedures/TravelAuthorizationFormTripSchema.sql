CREATE PROCEDURE [Rpt].[TravelAuthorizationFormTripSchema]

@TravelAuthorizationNumber VARCHAR(MAX)

AS

WITH DatesOfDuty AS
   (SELECT 
      [TravelAuthorizationID] = TA.TravelAuthorizationID,
      [From] = MIN(FromLocationDate),
      [To] = MAX(ToLocationDate)
     
    FROM TA.TravelAuthorization TA INNER JOIN 
       TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber 
    GROUP BY TA.TravelAuthorizationID
   )

SELECT DISTINCT
      [TravelAuthorizationNumber] = TA.TravelAuthorizationNumber,
      [Name] = TA.TravelersName,
	  [Purpose of Travel] = TA.PurposeOfTravel,
	  [FromDate] = DatesOfDuty.[From],
	  [ToDate] = DatesOfDuty.[To],
	  ------------------------------------------------------------------------
	  [TDY] = CASE WHEN TripSchema.[Description] = 'TDY' THEN 20 ELSE  40 END,
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
	  [Medical Travel - MSP] = CASE WHEN TripSchema.[Description] LIKE '%Medical Travel%' AND TripSchema.[Description] LIKE '%MSP%' THEN 20 ELSE  40 END
	  ------------------------------------------------------------------------
	                
	               
				  

   FROM
       TA.TravelAuthorization TA INNER JOIN 
       TA.WBS wbs ON wbs.TravelAuthorizationID = TA.[TravelAuthorizationID] INNER JOIN 
       TA.TravelItinerary t ON TA.TravelAuthorizationNumber = t.TravelAuthorizationNumber LEFT JOIN 
	   Lkp.Lookups TripSchema ON TripSchema.LookupsID = TA.TripSchemaCode LEFT JOIN
	   TEC.TECItinerary TEC ON TEC.TravelItineraryID = t.TravelItineraryID LEFT JOIN 
       TEC.TECExpenditure Ex ON TA.TravelAuthorizationNumber = Ex.TravelAuthorizationNumber LEFT JOIN 
       TEC.TECAdvances Adv ON TA.TravelAuthorizationNumber = Adv.TravelAuthorizationNumber LEFT JOIN 
	   DatesOfDuty ON DatesOfDuty.TravelAuthorizationID = TA.TravelAuthorizationID

    WHERE @TravelAuthorizationNumber = TA.TravelAuthorizationNumber