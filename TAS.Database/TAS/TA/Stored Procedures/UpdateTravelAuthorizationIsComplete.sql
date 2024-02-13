

CREATE PROCEDURE [TA].[UpdateTravelAuthorizationIsComplete]
@TravelAuthorizationID nvarchar(100),
@IsTecComplete bit,
@CreatedBy nvarchar(100)

AS
BEGIN

UPDATE [TA].[TravelAuthorization]
	SET
		 [IsTecComplete] = @IsTecComplete,
         [UpdatedBy] = @CreatedBy,
		 UpdatedDate = GETDATE()

	WHERE TravelAuthorizationID = @TravelAuthorizationID

END

