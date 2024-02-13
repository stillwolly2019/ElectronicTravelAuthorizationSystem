


CREATE PROCEDURE [Lkp].[DeleteLookupsGroups]
@LookupGroupID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN
UPDATE lkp.LookupsGroups SET 
	IsDeleted = 1,
	ModifiedBy = @CreatedBy,
	DateModified = GETDATE()
WHERE LookupGroupID= @LookupGroupID
END









