
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE   [TA].[GetCheckList] 
	@TravelAuthorizationID nvarchar(200),
	@lookupID nvarchar(200)
AS
BEGIN

SELECT cl.CheckListID, TravelAuthorizationID, LookupID, [Value], Note,DateCreated, CreatedBy, DateModified, ModifiedBy, isDeleted
FROM 
[TA].[CheckList] cl
WHERE 
cl.TravelAuthorizationID = @TravelAuthorizationID
AND cl.LookupID = @lookupID
AND cl.isDeleted = 0

END

