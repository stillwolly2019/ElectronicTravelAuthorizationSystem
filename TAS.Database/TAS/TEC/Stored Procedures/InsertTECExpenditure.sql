CREATE PROCEDURE [TEC].[InsertTECExpenditure]
@TravelAuthorizationNumber nvarchar(14),
@ExpenditureDate date,
@ExpenditureDetails nvarchar(500),
@CurrencyID nvarchar(100),
@ExpenseAmount float,
@CreatedBy nvarchar(100)
AS
BEGIN
	INSERT INTO TEC.TECExpenditure
		(TravelAuthorizationNumber, ExpenditureDate, ExpenditureDetails, CurrencyID, ExpenseAmount, CreatedBy)
	VALUES
		(@TravelAuthorizationNumber, @ExpenditureDate, @ExpenditureDetails, @CurrencyID, round(@ExpenseAmount,2), @CreatedBy)
		
END
