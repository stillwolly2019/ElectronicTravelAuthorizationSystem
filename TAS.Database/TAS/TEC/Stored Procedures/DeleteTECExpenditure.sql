CREATE PROCEDURE [TEC].[DeleteTECExpenditure]
@TECExpenditureID nvarchar(100),
@ModifiedBy nvarchar(100)
AS
BEGIN
	UPDATE TEC.TECExpenditure SET
		isDeleted = 1,
		UpdatedBy = @ModifiedBy,
		UpdatedDate = GETDATE()
	WHERE
		TECExpenditureID = @TECExpenditureID
		
END
