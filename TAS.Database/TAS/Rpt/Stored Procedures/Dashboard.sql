CREATE PROCEDURE [Rpt].[Dashboard]



AS

   SELECT 
      [Travel Authorizations] = COUNT(DISTINCT TA.TravelAuthorizationID),
	  [Travel Expense Claims] = (SELECT COUNT(TECItineraryID) FROM TEC.TECItinerary TEC WHERE TEC.isDeleted = 0 )
   FROM TA.TravelAuthorization TA 
   WHERE TA.isDeleted = 0 
        