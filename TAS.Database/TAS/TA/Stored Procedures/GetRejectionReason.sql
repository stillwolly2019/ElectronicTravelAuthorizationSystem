-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE   [TA].[GetRejectionReason] 
	@TravelAuthorizationID nvarchar(max),
	@RejectionReasonType char(3)
AS
BEGIN

select * from Ta.RejectionReason where TravelAuthorizationID=@TravelAuthorizationID and RejectionReasonType = @RejectionReasonType and isDeleted=0


END
