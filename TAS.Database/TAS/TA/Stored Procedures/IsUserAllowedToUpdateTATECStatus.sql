CREATE PROCEDURE [TA].[IsUserAllowedToUpdateTATECStatus]
	@TravelAuthorizationID nvarchar(100),
	@UserID nvarchar(100)
AS
BEGIN
	DECLARE @CurrentWorkflowStepID nvarchar(100)
	DECLARE @CurrentTATECWorkflowStepsID nvarchar(100)
	DECLARE @CurrentTATECStatus NVARCHAR(50)

	SELECT TOP 1 @CurrentWorkflowStepID  = WorkflowStepID , @CurrentTATECWorkflowStepsID = TATECWorkflowStepsID, @CurrentTATECStatus = TATECStatus
	FROM TA.TATECWorkflowSteps 
	WHERE TravelAuthorizationID = @TravelAuthorizationID 
	ORDER BY CreatedDate DESC

	IF [TA].[ufn_IsUserAllowedToUpdateTATECStatus](@UserID, @CurrentTATECWorkflowStepsID, @CurrentTATECStatus) = 0							
		SELECT 0
	ELSE
		SELECT 1
END