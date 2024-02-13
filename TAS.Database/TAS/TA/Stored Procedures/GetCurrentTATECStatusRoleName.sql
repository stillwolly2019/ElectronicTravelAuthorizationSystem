CREATE PROCEDURE [TA].[GetCurrentTATECStatusRoleName]
	@TravelAuthorizationID nvarchar(100)
AS
	DECLARE @RoleName NVARCHAR(50)

	SELECT TOP 1 @RoleName = r.RoleName
	FROM TA.TATECWorkflowSteps twfs
	INNER JOIN TA.WorkflowSteps wfs on twfs.WorkflowStepID = wfs.WorkflowStepID
	INNER JOIN Sec.Roles r on wfs.RoleID = r.RoleID
	WHERE TravelAuthorizationID = @TravelAuthorizationID 
	ORDER BY CreatedDate DESC

	SELECT @RoleName