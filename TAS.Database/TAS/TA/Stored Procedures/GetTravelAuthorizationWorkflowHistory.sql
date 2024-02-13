CREATE PROCEDURE [TA].[GetTravelAuthorizationWorkflowHistory]
	@TravelAuthorizationID nvarchar(100)
AS

	SELECT [TATECWorkflowStepsHistoryID]
	  ,ta.[TravelAuthorizationNumber]
      ,ta.[TravelAuthorizationID]
	  ,ta.TravelersName
		  ,CASE
		   WHEN  su.LastName IS NOT NULL
				THEN UPPER(su.LastName) + ' ' + su.FirstName				
		   WHEN ru.LastName IS NOT NULL
				THEN UPPER(ru.LastName) + ' ' + ru.FirstName
			ELSE  UPPER(pu.LastName) + ' ' + pu.FirstName
		   END as FullName
      ,r.RoleName as FullRoleName
      ,l.Description as StatusDescription
	  ,l.Code as StatusCode
        ,CONVERT(varchar(17),ISNULL([SignedDate], [RejectedDate]), 113)   as DoneDate 
		,Case
		WHEN datediff(minute,twfs.[CreatedDate],ISNULL([SignedDate], [RejectedDate])) <=60
			THEN CAST(datediff(minute,twfs.[CreatedDate],ISNULL([SignedDate], [RejectedDate])) as nvarchar(10)) + ' Minutes'
		WHEN datediff(Hour,twfs.[CreatedDate],ISNULL([SignedDate], [RejectedDate])) <=24
			THEN  CAST(datediff(Hour,twfs.[CreatedDate],ISNULL([SignedDate], [RejectedDate])) as nvarchar(10)) + ' Hours'
		WHEN datediff(day,twfs.[CreatedDate],ISNULL([SignedDate], [RejectedDate])) <=30
			THEN  CAST(datediff(day,twfs.[CreatedDate],ISNULL([SignedDate], [RejectedDate])) as nvarchar(10)) + ' Days'
		ELSE ''
		END as CompDate
      ,CONVERT(varchar(17),twfs.[CreatedDate], 113) AS CreatedDate
	  ,twfs.RejectedBy
	  ,twfs.SignedBy
  FROM [TA].[TATECWorkflowStepsHistory] twfs
    inner join ta.WorkflowSteps wfs on  wfs.WorkflowStepID = twfs.WorkflowStepID
	inner join TA.TravelAuthorization ta on ta.TravelAuthorizationID = twfs.TravelAuthorizationID
  inner join sec.roles r on wfs.RoleID = r.RoleID
  left join sec.Users su on twfs.SignedBy = su.UserID
  left join sec.Users ru on twfs.RejectedBy = ru.UserID
  LEFT JOIN sec.Users pu on twfs.UserID = pu.UserID
  outer apply
  (
	   SELECT Code, Description from 
	   lkp.Lookups WHERE Code = twfs.[TATECStatus]
	   AND IsDeleted = 0
   ) as l
  WHERE Ta.TravelAuthorizationID = @TravelAuthorizationID AND
  r.IsDeleted = 0 
  order by twfs.CreatedDate desc