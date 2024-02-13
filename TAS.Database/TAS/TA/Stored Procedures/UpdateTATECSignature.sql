CREATE PROCEDURE [TA].[UpdateTATECSignature]
	@TravelAuthorizationID nvarchar(100),
	@UserID nvarchar(100),
	@RoleID nvarchar(100),
	@Scope nvarchar(10),
	@IsApproved bit

AS
IF @IsApproved = 1
	BEGIN

		declare @Status nvarchar(50)
		declare @WorkflowStepID nvarchar(100)
		declare @ApprovedWorkflowStepID nvarchar(100)

		Select @Status = TATECApprovedStatus , @WorkflowStepID = WorkflowStepID, @ApprovedWorkflowStepID = ApprovedWorkflowStepID
		From TA.WorkflowSteps
		WHERE RoleID = @RoleID AND
		Scope = @Scope

		UPDATE TA.TATECWorkflowSteps
		SET 
			SignedBy = @UserID,
			SignedDate = GETDATE(),
			TATECStatus = @Status
		WHERE
			UserID = @UserID

		DECLARE @IsSignedByRole bit
		DECLARE @ApprovedRoleID nvarchar(100)		
		DECLARE @ApprovedStatus nvarchar(50)

		SELECT @ApprovedRoleID = RoleID, @ApprovedStatus = TATECApprovedStatus, @IsSignedByRole = IsSignedByRole from TA.WorkflowSteps 
		WHERE WorkflowStepID = @ApprovedWorkflowStepID

		INSERT INTO [TA].[TATECWorkflowSteps]
			   ([TravelAuthorizationID]
			   ,[WorkflowStepID]
			   ,[UserID]
			   ,[RoleID]
			   ,[TATECStatus]
			   )
		 VALUES
			   (
			    @TravelAuthorizationID
			   ,@ApprovedWorkflowStepID
			   ,''
			   ,@ApprovedRoleID
			   ,@ApprovedStatus
			   )




	END
RETURN 0