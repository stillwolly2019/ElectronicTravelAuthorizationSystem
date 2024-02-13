




CREATE PROCEDURE [Lkp].[CheckDuplicateLookupGroup]
@LookupGroupID nvarchar(100),
@LookupGroup nvarchar(100)

AS
BEGIN
SELECT * FROM Lkp.LookupsGroups WHERE LookupGroup = @LookupGroup  AND LookupGroupID <> @LookupGroupID AND IsDeleted = 0

END







