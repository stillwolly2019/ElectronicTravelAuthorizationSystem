CREATE FUNCTION [TA].[ufn_GetNextWorkflowStepRecord]
(
	@CurrentTATECWorkflowStepsID nvarchar(100)	
)
RETURNS @returntable TABLE
(
	WorkflowStepID uniqueidentifier,
	UserID uniqueidentifier,
	RoleID uniqueidentifier
)
AS
BEGIN
	-- get the current step informatin
	DECLARE @WorkflowStepID nvarchar(100)
	DECLARE @RoleID nvarchar(100)

	DECLARE @TravelAuthorizationID nvarchar(100)
	DECLARE @CurrentRoleName nvarchar(100)	
	DECLARE @Scope nvarchar(50)
	DECLARE @ApprovedWorkflowStepID nvarchar(100)	
	DECLARE @NextApprovedWorkflowStepID nvarchar(100)	
	DECLARE @ApprovedUserID nvarchar(100) = NULL	
	DECLARE @CurrentRoleID nvarchar(100) = NULL

	-- get the current TATEC workflow step information
	SELECT @TravelAuthorizationID = TravelAuthorizationID, @CurrentRoleName= r.RoleName, @Scope = wfs.Scope,
		   @ApprovedWorkflowStepID = ApprovedWorkflowStepID, @NextApprovedWorkflowStepID = NextApprovedWorkflowStepID
	FROM TA.TATECWorkflowSteps twfs
	INNER JOIN TA.WorkflowSteps wfs on twfs.WorkflowStepID = wfs.WorkflowStepID
	INNER JOIN Sec.Roles r on wfs.RoleID = r.RoleID
	WHERE twfs.TATECWorkflowStepsID = @CurrentTATECWorkflowStepsID

	DECLARE @IsPMRequired bit, @IsCOMRequired bit, @IsRMORequired bit, @IsNotForDSA bit
	SELECT @IsPMRequired = ISNULL(IsPMRequired, 1), @IsCOMRequired = ISNULL(IsCOMRequired, 1), 
	@IsRMORequired = ISNULL(IsRMORequired, 1), @IsNotForDSA = ISNULL(IsNotForDSA, 1)
	FROM TA.TravelAuthorization 
	WHERE TravelAuthorizationID = @TravelAuthorizationID

	IF @Scope = 'TA'
	BEGIN
		IF @CurrentRoleName= 'Staff Member'
		BEGIN
			-- supervisor area
			Select @ApprovedUserID = SupervisorUserID FROM TA.TravelAuthorization WHERE TravelAuthorizationID = @TravelAuthorizationID	
			SET @WorkflowStepID	= @ApprovedWorkflowStepID
			SET @RoleID = NULL
		END
		ELSE IF @CurrentRoleName= 'Supervisor'
		BEGIN
			-- PM area
			IF @IsPMRequired = 1
			BEGIN						
				Select @ApprovedUserID = PMUserID FROM TA.TravelAuthorization WHERE TravelAuthorizationID = @TravelAuthorizationID
				SET @WorkflowStepID	= @ApprovedWorkflowStepID
				SET @RoleID = NULL
			END
			ELSE
			BEGIN
				-- Admin area
				SET @ApprovedUserID = NULL
				SET @WorkflowStepID	= @NextApprovedWorkflowStepID
				SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @NextApprovedWorkflowStepID
			END
		END
		ELSE IF @CurrentRoleName= 'Project Manager'
		BEGIN
			-- Admin area
			SET @ApprovedUserID = NULL
			SET @WorkflowStepID	= @ApprovedWorkflowStepID
			SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @ApprovedWorkflowStepID
		END
		ELSE IF @CurrentRoleName= 'Admin'
		BEGIN	
			-- RMO Area
			IF @IsRMORequired = 1
			BEGIN
				SET @ApprovedUserID = NULL
				SET @WorkflowStepID	= @ApprovedWorkflowStepID
				SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @ApprovedWorkflowStepID
			END
			ELSE
			BEGIN
				-- SRMO area
				SET @ApprovedUserID = NULL
				SET @WorkflowStepID	= @NextApprovedWorkflowStepID
				SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @NextApprovedWorkflowStepID
			END
		END
		ELSE IF @CurrentRoleName= 'RMO'
		BEGIN
			-- SRMO area
			SET @ApprovedUserID = NULL
			SET @WorkflowStepID	= @ApprovedWorkflowStepID
			SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @ApprovedWorkflowStepID
		END
		ELSE IF @CurrentRoleName= 'SRMO'
		BEGIN
			-- Cheif of Mission area
			IF @IsCOMRequired = 1
			BEGIN
				Select @ApprovedUserID = COMUserID FROM TA.TravelAuthorization WHERE TravelAuthorizationID = @TravelAuthorizationID	
				SET @WorkflowStepID	= @ApprovedWorkflowStepID
				SET @RoleID = NULL				
			END
			ELSE IF @IsNotForDSA = 0
			BEGIN
				-- Finance
				SET @ApprovedUserID = NULL
				SET @WorkflowStepID	= @NextApprovedWorkflowStepID
				SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @NextApprovedWorkflowStepID
			END		
		END
		ELSE IF @CurrentRoleName= 'Chief of Mission'
		BEGIN
			IF @IsNotForDSA = 0
			BEGIN
				-- Finance
				SET @ApprovedUserID = NULL
				SET @WorkflowStepID	= @ApprovedWorkflowStepID
				SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @ApprovedWorkflowStepID 
			END
		END
		ELSE IF @CurrentRoleName= 'Finance'
		BEGIN			
			-- System Administrator
			SET @ApprovedUserID = NULL
			SET @WorkflowStepID	= @ApprovedWorkflowStepID
			SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @ApprovedWorkflowStepID 
			
		END		
	END
	ELSE IF @Scope = 'TEC'
	BEGIN
	IF @CurrentRoleName = 'System Administrator'
	BEGIN
			Select @ApprovedUserID = UserID FROM TA.TravelAuthorization WHERE TravelAuthorizationID = @TravelAuthorizationID	
			SET @WorkflowStepID	= @ApprovedWorkflowStepID
			SELECT @RoleID = NULL
	END
	ELSE
	BEGIN
		SET @ApprovedUserID = NULL
		SET @WorkflowStepID	= @ApprovedWorkflowStepID
		SELECT @RoleID = RoleID FROM TA.WorkflowSteps WHERE WorkflowStepID = @ApprovedWorkflowStepID 
	END
	END


	INSERT @returntable
	SELECT @WorkflowStepID, @ApprovedUserID, @RoleID
	RETURN

END