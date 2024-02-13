CREATE FUNCTION [TA].[ufn_IsUserAllowedToUpdateTATECStatus]
(
	@UserID nvarchar(100),
	@CurrentTATECWorkflowStepsID nvarchar(100),
	@CurrentTATECStatus nvarchar(50)
)
RETURNS bit
AS
BEGIN	
	-- If the stats is SET then do nothing 
	IF @CurrentTATECStatus = 'SET'					
		RETURN 0
	
	-- if userid is not the same userid assigned to, or not
	DECLARE @AssignedUserID nvarchar(100)
	DECLARE @AssignedRoleID nvarchar(100)
	SELECT @AssignedUserID = UserID FROM TA.TATECWorkflowSteps WHERE TATECWorkflowStepsID = @CurrentTATECWorkflowStepsID

	IF @AssignedUserID IS NOT NULL
	BEGIN	
		IF @AssignedUserID <> @UserID		
			RETURN 0		
	END
	ELSE		
		IF NOT EXISTS(SELECT RoleID FROM TA.TATECWorkflowSteps 
		WHERE TATECWorkflowStepsID = @CurrentTATECWorkflowStepsID 
		AND RoleID IN (SELECT RoleID FROM Sec.UsersRoles WHERE UserID = @UserID))		
			RETURN 0			

	RETURN 1
END