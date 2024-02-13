



CREATE PROCEDURE [Lkp].[SearchLookups]
@LookupGroupID nvarchar(100),
@SubGroupID nvarchar(100),
@Code nvarchar(50),
@Description nvarchar(250)

AS
BEGIN

SELECT l.*,lg.LookupGroup,lsg.LookupGroup as SubGroupName 
FROM [Lkp].[Lookups] l 
Inner join [Lkp].[LookupsGroups] lg on lg.LookupGroupID = l.LookupGroupID
left join [Lkp].[LookupsGroups] lsg on lsg.LookupGroupID = l.SubGroupID
WHERE 
l.IsDeleted = 0 
AND (l.LookupGroupID = @LookupGroupID or @LookupGroupID = '00000000-0000-0000-0000-000000000000')
AND (l.SubGroupID = @SubGroupID or @SubGroupID = '00000000-0000-0000-0000-000000000000')
AND (l.Code = @Code or @Code = '')
AND (l.[Description] = @Description or @Description = '')

ORDER BY [Description]




END






