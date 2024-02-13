CREATE PROCEDURE  [Rpt].[GetWBSSubReport] 


@TravelAuthorizationID nvarchar(100)

AS


SELECT TravelAuthorizationID,[WBSCode]

FROM TA.WBS  

WHERE TravelAuthorizationID = @TravelAuthorizationID AND
	  isDeleted = 0

ORDER BY TravelAuthorizationID,[WBSCode]
