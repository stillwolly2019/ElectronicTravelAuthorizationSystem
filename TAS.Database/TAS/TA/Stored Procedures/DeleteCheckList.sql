
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[DeleteCheckList] 
	@TravelAuthorizationID nvarchar(200)

AS
BEGIN

DELETE FROM TA.CheckList WHERE TravelAuthorizationID = @TravelAuthorizationID
	 
END

