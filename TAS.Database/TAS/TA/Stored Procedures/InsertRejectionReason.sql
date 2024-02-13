-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[InsertRejectionReason] 
	@TravelAuthorizationID nvarchar(max),
	@RejectionReasonID nvarchar(max),
	@RejectionReasonType char(3),
	@CreatedBy nvarchar(max)

AS
BEGIN
	
	insert into Ta.RejectionReason(TravelAuthorizationID,RejectionReasonID,RejectionReasonType,DateCreated,CreatedBy,isDeleted)
	values(@TravelAuthorizationID,@RejectionReasonID,@RejectionReasonType,GETDATE(),@CreatedBy,0)

END
