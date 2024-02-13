



CREATE PROCEDURE [Lkp].[DeleteLookups]
@LookupsID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN
UPDATE lkp.Lookups SET 
	IsDeleted = 1,
	ModifiedBy = @CreatedBy,
	DateModified = GETDATE()
WHERE LookupsID= @LookupsID
END










