CREATE PROCEDURE [TA].[GetRjectionReasons]
	@TATECWorkflowStepsID nvarchar(100)	
AS

	SELECT [TATECWorkflowStepsRejectionReason]
		  ,[TATECWorkflowStepID]
		  ,[RejectionReasonID]
		  ,l.Description		  
	  FROM [TA].[TATECWorkflowStepsRejectionReasons] rs
	  INNER JOIN Lkp.Lookups l on l.LookupsID = rs.RejectionReasonID
	  WHERE
	  TATECWorkflowStepID = @TATECWorkflowStepsID AND
	  rs.IsDeleted = 0 AND
	  l.IsDeleted = 0