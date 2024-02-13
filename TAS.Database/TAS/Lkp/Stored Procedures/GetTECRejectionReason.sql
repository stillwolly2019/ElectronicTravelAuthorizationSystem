
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Lkp].[GetTECRejectionReason]
	
AS
BEGIN

SELECT LookupsID, Code, [Description] FROM [Lkp].Lookups l 
left join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID  WHERE lg.LookupGroup='TEC Rejection Reason' and l.IsDeleted = 0 ORDER BY [Description]
END

