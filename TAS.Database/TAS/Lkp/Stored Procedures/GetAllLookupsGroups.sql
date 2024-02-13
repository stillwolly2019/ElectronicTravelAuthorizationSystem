


CREATE PROCEDURE [Lkp].[GetAllLookupsGroups]

AS
BEGIN

SELECT * FROM [Lkp].[LookupsGroups] WHERE IsDeleted = 0 ORDER BY [LookupGroup]



END





