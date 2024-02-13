


CREATE PROCEDURE [Lkp].[GetAllLookups]

AS
BEGIN

SELECT l.*,lg.LookupGroup,lsg.LookupGroup as SubGroupName FROM [Lkp].[Lookups] l 
Inner join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID
left join [Lkp].[LookupsGroups] lsg on lsg.LookupGroupID = l.SubGroupID
WHERE l.IsDeleted = 0 ORDER BY [Description]



END





