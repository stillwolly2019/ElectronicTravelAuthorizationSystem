CREATE PROCEDURE [TEC].[UpdateTECAdvances]
@TravelAuthorizationID nvarchar(100),
@AdvancesNotApplicable bit,
@CreatedBy nvarchar(100)
AS
BEGIN
	update [TA].[TravelAuthorization] 
	set AdvancesNotApplicable = @AdvancesNotApplicable,
	UpdatedBy = @CreatedBy,
	UpdatedDate = GETDATE()
	where TravelAuthorizationID = @TravelAuthorizationID
		
END
