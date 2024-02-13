
CREATE PROCEDURE [TA].[GetTATECStatusByTAID]

@TravelAuthorizationID nvarchar(100)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 TATECStatus as StatusCode, SignedBy 
	FROM [TA].[TATECWorkflowSteps] 	
	WHERE TravelAuthorizationID = @TravelAuthorizationID AND
	TATECStatus IS NOT NULL
	ORDER BY CreatedDate DESC

END