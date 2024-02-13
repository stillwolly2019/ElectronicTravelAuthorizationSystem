CREATE PROCEDURE [Rpt].[GetTASignatures]
	@TravelAuthorizationID NVARCHAR(100)
AS
	 SELECT u.FirstName +' '+ UPPER(u.LastName) AS SignedBy, CONVERT(varchar(17),[twfs].[SignedDate], 113) AS SignedDate, r.RoleName 
	 FROM  [TA].[TATECWorkflowSteps] twfs
		   inner join ta.WorkflowSteps wfs ON twfs.WorkflowStepID = wfs.WorkflowStepID
		   inner join sec.roles r ON wfs.RoleID = r.RoleID
		   inner join sec.users u ON twfs.SignedBy = u.UserID
	WHERE twfs.TravelAuthorizationID = @TravelAuthorizationID
	ORDER BY twfs.CreatedDate DESC