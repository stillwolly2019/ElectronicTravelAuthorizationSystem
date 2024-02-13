

CREATE PROCEDURE [TA].[GetWBSByTravelAuthorizationID]
@TravelAuthorizationID nvarchar(100)
AS
BEGIN

select [WBSID]
      ,[TravelAuthorizationID]
      , UPPER([WBSCode]) as [WBSCode]
      ,[PercentageOrAmount]
      ,[Note]
      ,[IsPercentage]
      ,[CreatedDate]
      ,[CreatedBy]
      ,[UpdatedDate]
      ,[UpdatedBy]
      ,[isDeleted] from [TA].WBS 
where isDeleted = 0 and TravelAuthorizationID = @TravelAuthorizationID
	
END


