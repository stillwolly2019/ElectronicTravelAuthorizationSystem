CREATE PROCEDURE [TEC].[UpdateTECExpenditureRates]
@TECExpenditureID nvarchar(100),
@Rate float,
@RateAmount float,
@LocalAmount float,
@ModifiedBy nvarchar(100)
AS
BEGIN
	UPDATE TEC.TECExpenditure SET
		Rate = @Rate,
		RateAmount = @RateAmount,
		LocalAmount = @LocalAmount,
		UpdatedBy = @ModifiedBy,
		UpdatedDate = GETDATE()
	WHERE
		TECExpenditureID = @TECExpenditureID
		
END
