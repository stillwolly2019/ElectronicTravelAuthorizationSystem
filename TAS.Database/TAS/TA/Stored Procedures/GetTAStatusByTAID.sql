
CREATE PROCEDURE [TA].[GetTAStatusByTAID]

@TravelAuthorizationID nvarchar(100)

AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 * FROM [TA].[StatusChangeHistory] WHERE TravelAuthorizationID = @TravelAuthorizationID ORDER BY CreatedDate DESC

END
