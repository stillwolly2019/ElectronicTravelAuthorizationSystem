

CREATE PROCEDURE [TA].[InsertTADocumentNumber]
	
  @TravelAuthorizationID nvarchar(100),
  @DocumentNumber nvarchar(100),
  @CreatedBy nvarchar(100)

AS
BEGIN

  Update [TA].[TravelAuthorization] set 
  DocumentNumber = @DocumentNumber, 
  UpdatedBy = @CreatedBy, 
  UpdatedDate = GETDATE() 
  where TravelAuthorizationID = @TravelAuthorizationID

END
