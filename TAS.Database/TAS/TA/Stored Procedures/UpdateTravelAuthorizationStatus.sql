


CREATE PROCEDURE [TA].[UpdateTravelAuthorizationStatus]
@TravelAuthorizationID nvarchar(100),
@StatusCode nvarchar(3),
@CreatedBy nvarchar(100),
@Comments nvarchar(max)

AS
BEGIN

UPDATE [TA].[TravelAuthorization]
	SET
		 [StatusCode] = @StatusCode,
         [UpdatedBy] = @CreatedBy,
		 UpdatedDate = GETDATE()

	WHERE TravelAuthorizationID = @TravelAuthorizationID

INSERT 
INTO [TA].[StatusChangeHistory] 
	(TravelAuthorizationID,StatusCode, RejectionReasons, CreatedBy)
VALUES
	(@TravelAuthorizationID,@StatusCode, @Comments, @CreatedBy)


END
