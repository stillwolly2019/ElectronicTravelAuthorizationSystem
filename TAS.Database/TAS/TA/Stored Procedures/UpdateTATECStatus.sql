CREATE PROCEDURE [TA].[UpdateTATECStatus]
	@TravelAuthorizationID nvarchar(100),
	@UserID nvarchar(100),		
	@IsApproved bit,
	@RejectionReasons nvarchar(500) = null

AS
BEGIN TRY ------------BEGIN TRY
BEGIN TRANSACTION; -- BEGIN Main Transaction

	DECLARE @CurrentWorkflowStepID nvarchar(100)
	DECLARE @CurrentTATECWorkflowStepsID nvarchar(100)
	DECLARE @CurrentTATECStatus NVARCHAR(50)
	-- get the last step
	SELECT TOP 1 @CurrentWorkflowStepID  = WorkflowStepID , @CurrentTATECWorkflowStepsID = TATECWorkflowStepsID, @CurrentTATECStatus = TATECStatus
	FROM TA.TATECWorkflowSteps 
	WHERE TravelAuthorizationID = @TravelAuthorizationID 
	ORDER BY CreatedDate DESC

	IF [TA].[ufn_IsUserAllowedToUpdateTATECStatus](@UserID, @CurrentTATECWorkflowStepsID, @CurrentTATECStatus) = 0
	BEGIN						
		COMMIT
		SELECT 0
		RETURN
	END

	IF @IsApproved = 1
		BEGIN

			DECLARE @ApprovedTATECStatus nvarchar(50)
			DECLARE @ApprovedWorkflowStepID nvarchar(100)
			-- get approved status, and the next approved step
			SELECT @ApprovedTATECStatus = TATECApprovedStatus, @ApprovedWorkflowStepID = ApprovedWorkflowStepID
			FROM TA.WorkflowSteps
			WHERE WorkflowStepID  = @CurrentWorkflowStepID 

			-- update the current (last) step to the approved status with siginture
			UPDATE TA.TATECWorkflowSteps
			SET 
				SignedBy = @UserID,
				SignedDate = GETDATE(),
				TATECStatus = @ApprovedTATECStatus 
			WHERE
				TATECWorkflowStepsID = @CurrentTATECWorkflowStepsID

			-- update the history table
			UPDATE TA.TATECWorkflowStepsHistory
			SET 
				SignedBy = @UserID,
				SignedDate = GETDATE(),
				TATECStatus = @ApprovedTATECStatus 
			WHERE
				TATECWorkflowStepsHistoryID = @CurrentTATECWorkflowStepsID

			DECLARE @NextWorkflowStepID nvarchar(100), @NextRoleID nvarchar(100), @NextUserID nvarchar(100)
			Select @NextWorkflowStepID = WorkflowStepID, @NextUserID = UserID, @NextRoleID = RoleID FROM [TA].[ufn_GetNextWorkflowStepRecord](@CurrentTATECWorkflowStepsID)
			IF @NextWorkflowStepID IS NOT NULL 
			BEGIN
				DECLARE @NewTATECWorkflowStepsID UNIQUEIDENTIFIER  
				SET @NewTATECWorkflowStepsID = NEWID()

				INSERT INTO [TA].[TATECWorkflowSteps]
					   (
						TATECWorkflowStepsID
					   ,[TravelAuthorizationID]
					   ,[WorkflowStepID]
					   ,[UserID]
					   ,[RoleID]
					   ,[CreatedBy]
					   )
				 VALUES
					   (
						@NewTATECWorkflowStepsID
					   ,@TravelAuthorizationID
					   ,@NextWorkflowStepID
					   ,@NextUserID
					   ,@NextRoleID
					   ,@UserID
					   )

				INSERT INTO [TA].[TATECWorkflowStepsHistory]
					   (TATECWorkflowStepsHistoryID
					   ,[TravelAuthorizationID]
					   ,[WorkflowStepID]
					   ,[UserID]
					   ,[RoleID]
					   ,[CreatedBy]
					   )
				 VALUES
					   (
						@NewTATECWorkflowStepsID
					   ,@TravelAuthorizationID
					   ,@NextWorkflowStepID
					   ,@NextUserID
					   ,@NextRoleID
					   ,@UserID
					   )
			END
			ELSE 
			BEGIN
				DECLARE @IsNotForDSA bit
			    SELECT @IsNotForDSA = ISNULL(IsNotForDSA, 1) FROM TA.TravelAuthorization WHERE TravelAuthorizationID = @TravelAuthorizationID
				IF @IsNotForDSA = 1
				BEGIN
					UPDATE TA.TATECWorkflowSteps
					SET 
						TATECStatus = 'NDSA'
					WHERE
					TATECWorkflowStepsID = @CurrentTATECWorkflowStepsID
					
					UPDATE TA.TATECWorkflowStepsHistory
					SET 
						TATECStatus = 'NDSA'
					WHERE
						TATECWorkflowStepsHistoryID = @CurrentTATECWorkflowStepsID
				END
			END
		END
		ELSE
		BEGIN
			
			DECLARE @RejectedTATECStatus nvarchar(50), @StaffID nvarchar(100)
			DECLARE @RejectedWorkflowStepID nvarchar(100)

			SELECT @RejectedTATECStatus  = TATECRejectedStatus, @RejectedWorkflowStepID = RejectionWorkflowStepID
			FROM TA.WorkflowSteps
			WHERE WorkflowStepID  = @CurrentWorkflowStepID 

			-- get the userid where the rejection path should go
			SELECT  @StaffID = UserID
			FROM [TA].[TATECWorkflowSteps]
			WHERE 
			TravelAuthorizationID = @TravelAuthorizationID AND WorkflowStepID = @RejectedWorkflowStepID		
			-- keep history of the rejections 
			UPDATE TA.TATECWorkflowStepsHistory
			SET 
				RejectedBy = @UserID,
				RejectedDate = GETDATE(),
				TATECStatus = @RejectedTATECStatus,
				CreatedBy = @UserID
			WHERE
				TATECWorkflowStepsHistoryID = @CurrentTATECWorkflowStepsID

			INSERT INTO [TA].[TATECWorkflowStepsRejectionReasons]
					   (
					    [TATECWorkflowStepID]
					   ,[RejectionReasonID]					   
					   ,[CreatedBy])
				 SELECT					   
					    @CurrentTATECWorkflowStepsID
					   ,value
					   ,@UserID
					FROM [dbo].[ufn_Split] (@RejectionReasons,',')


			-- get all the next approved steps untill the current step.
		   ;WITH
		   CTEWrokflow (WorflowStepID, RoleID, ApprovedWorkflowStepID)
		   AS
		   (
			SELECT [WorkflowStepID], [RoleID] ,ApprovedWorkflowStepID
			FROM [TravelAuthorization].[TA].[WorkflowSteps]
			WHERE WorkflowStepID=@RejectedWorkflowStepID
			UNION ALL
			SELECT w.[WorkflowStepID], w.[RoleID],w.ApprovedWorkflowStepID 
			FROM [TravelAuthorization].[TA].[WorkflowSteps] w			
			INNER JOIN CTEWrokflow cw on cw.ApprovedWorkflowStepID = w.[WorkflowStepID]
			--WHERE cw.WorflowStepID <> @CurrentWorkflowStepID
  		   )

		   -- delete all previuos signitures 
		   DELETE from TA.TATECWorkflowSteps where WorkflowStepID in (select WorflowStepID from CTEWrokflow)
			-- create new record of the rejected path 
			Declare @TempTATECWorkflowStepsID uniqueidentifier
			Set @TempTATECWorkflowStepsID = NEWID()

			INSERT INTO [TA].[TATECWorkflowSteps]
			   (
			    TATECWorkflowStepsID
			   ,[TravelAuthorizationID]
			   ,[WorkflowStepID]
			   ,[UserID]
			   ,[RoleID]
			   ,[TATECStatus]
			   ,[SignedBy]
			   ,[SignedDate]
			   ,[RejectedBy]
			   ,[RejectedDate]
			   ,[CreatedBy]
			   )
		 VALUES
			   (
			    @TempTATECWorkflowStepsID
			   ,@TravelAuthorizationID
			   ,@RejectedWorkflowStepID
			   ,@StaffID
			   ,NULL
			   ,@RejectedTATECStatus
			   ,NULL
			   ,NULL
			   ,NULL
			   ,NULL
			   ,@StaffID
			   )

		
		INSERT INTO [TA].[TATECWorkflowStepsHistory]
			    (
			    TATECWorkflowStepsHistoryID
			   ,[TravelAuthorizationID]
			   ,[WorkflowStepID]
			   ,[UserID]
			   ,[RoleID]
			   ,[TATECStatus]
			   ,[SignedBy]
			   ,[SignedDate]
			   ,[RejectedBy]
			   ,[RejectedDate]
			   ,[CreatedBy]
			   )
		 VALUES
			   (
			    @TempTATECWorkflowStepsID
			   ,@TravelAuthorizationID
			   ,@RejectedWorkflowStepID
			   ,@StaffID
			   ,NULL
			   ,@RejectedTATECStatus
			   ,NULL
			   ,NULL
			   ,NULL
			   ,NULL
			   ,@StaffID
			   )


		END

	Select 1
	COMMIT TRANSACTION; -- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    SELECT -1
END CATCH; --------END CATCH