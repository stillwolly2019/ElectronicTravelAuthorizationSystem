CREATE PROCEDURE [TEC].[DeleteTECAdvances]
@TECAdvancesID nvarchar(100),
@ModifiedBy nvarchar(100)
AS
BEGIN
	UPDATE TEC.TECAdvances SET
		isDeleted = 1,
		UpdatedBy = @ModifiedBy,
		UpdatedDate = GETDATE()
	WHERE
		TECAdvancesID = @TECAdvancesID
		
END
