-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[DeleteRejectionReason] 
	@TravelAuthorizationID nvarchar(max)

AS
BEGIN
	update Ta.RejectionReason
	set isDeleted=1 
	where TravelAuthorizationID=@TravelAuthorizationID
	 
END
