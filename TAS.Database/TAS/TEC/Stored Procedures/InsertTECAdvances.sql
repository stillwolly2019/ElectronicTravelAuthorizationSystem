CREATE PROCEDURE [TEC].[InsertTECAdvances]
@TravelAuthorizationNumber nvarchar(14),
@PayOfficeCode nvarchar(130),
@DatePaid date,
@CurrencyID nvarchar(100),
@AdvanceAmount float,
@CreatedBy nvarchar(100)
AS
BEGIN
	INSERT INTO TEC.TECAdvances 
		(TravelAuthorizationNumber,PayOfficeCodeID,DatePaid,CurrencyID,AdvanceAmount,CreatedBy)
	VALUES
		(@TravelAuthorizationNumber,@PayOfficeCode,@DatePaid,@CurrencyID,round(@AdvanceAmount,2),@CreatedBy)
		
END
