CREATE PROCEDURE [TEC].[UpdateTECExpenditure]
@TravelAuthorizationID nvarchar(100),
@ExpenditureNotApplicable bit,
@CreatedBy nvarchar(100)
AS
BEGIN
	update [TA].[TravelAuthorization] 
	set ExpenditureNotApplicable = @ExpenditureNotApplicable,
	UpdatedBy = @CreatedBy,
	UpdatedDate = GETDATE()
	where TravelAuthorizationID = @TravelAuthorizationID
		
END
