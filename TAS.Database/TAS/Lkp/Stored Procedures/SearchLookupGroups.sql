




CREATE PROCEDURE [Lkp].[SearchLookupGroups]

@LookupGroup nvarchar(250)

AS
BEGIN

SELECT * FROM [Lkp].[LookupsGroups] 
WHERE IsDeleted = 0 
and (LookupGroup = @LookupGroup or @LookupGroup = '')
ORDER BY [LookupGroup]


END



