
CREATE PROCEDURE [TA].[GetTAStepByTravelAuthorizationID] 

@TravelAuthorizationID nvarchar(100)

AS
BEGIN

	SET NOCOUNT ON;

    DECLARE @Step as nvarchar(10);

	set @Step = (SELECT CASE 
	WHEN Travel.TAConfirm is NULL THEN 'Step 2' 
	WHEN WBS.NoOfWBS = 0 THEN 'Step 3' 
	WHEN Itinerary.NoOFItinerary = 0 THEN 'Step 4' 
	WHEN TECItinerary.NoOfTECItinerary = 0 THEN 'Step 5' 
	WHEN TECExpenditure.NoOfExpenditure = 0 AND ISNULL(Travel.ExpenditureNotApplicable,0) = 0 THEN 'Step 6' 
	WHEN TECAdvances.NoOfAdvances = 0 AND ISNULL(Travel.AdvancesNotApplicable,0) = 0 THEN 'Step 7' 
	WHEN CheckList.NoOfCheckList = 0 THEN 'Step 8' 
	ELSE
	'Step 9'
	END As Step

	FROM [TA].TravelAuthorization Travel 
	CROSS APPLY (Select Count([TravelItineraryID]) as NoOFItinerary from [TA].[TravelItinerary] where [TravelAuthorizationNumber] = Travel.[TravelAuthorizationNumber] and isDeleted = 0) Itinerary
	CROSS APPLY (Select Count([WBSID]) as NoOfWBS from [TA].[WBS] where [TravelAuthorizationID] = Travel.[TravelAuthorizationID] and isDeleted = 0) WBS
	CROSS APPLY (Select Count([TravelItineraryID]) as NoOfTECItinerary from [TA].[TravelItinerary] where [TravelAuthorizationNumber] = Travel.[TravelAuthorizationNumber] and isDeleted = 0 and FromLocationTime is not null) TECItinerary
	
	CROSS APPLY (Select Count([TECExpenditureID]) as NoOfExpenditure from [TEC].TECExpenditure where [TravelAuthorizationNumber] = Travel.[TravelAuthorizationNumber] and isDeleted = 0) TECExpenditure
	CROSS APPLY (Select Count([TECAdvancesID]) as NoOfAdvances from [TEC].TECAdvances where [TravelAuthorizationNumber] = Travel.[TravelAuthorizationNumber] and isDeleted = 0) TECAdvances
	
	CROSS APPLY (Select Count([CheckListID]) as NoOfCheckList from [TA].CheckList where [TravelAuthorizationID] = Travel.[TravelAuthorizationID] and isDeleted = 0) CheckList
	WHERE Travel.[TravelAuthorizationID] = @TravelAuthorizationID and isDeleted = 0
	)
	SELECT @Step
END
