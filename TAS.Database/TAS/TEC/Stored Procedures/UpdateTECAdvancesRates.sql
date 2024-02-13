CREATE PROCEDURE [TEC].[UpdateTECAdvancesRates]
@TECAdvancesID nvarchar(100),
@Rate float,
@RateAmount float,
@LocalAmount float,
@ModifiedBy nvarchar(100)
AS
BEGIN
	UPDATE TEC.TECAdvances SET
		Rate = @Rate,
		RateAmount = @RateAmount,
		LocalAmount = @LocalAmount,
		UpdatedBy = @ModifiedBy,
		UpdatedDate = GETDATE()
	WHERE
		TECAdvancesID = @TECAdvancesID
		
END
