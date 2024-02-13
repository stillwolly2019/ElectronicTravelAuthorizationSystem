
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Lkp].[GetCheckList] 
	--@TravelAuthorizationID nvarchar(200)
AS
BEGIN

SELECT l.LookupsID,l.Code, l.[Description], '' AS Note FROM [Lkp].Lookups l 
LEFT JOIN [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID
WHERE lg.LookupGroup='Check List' and l.IsDeleted = 0 
ORDER BY [Description]

END

