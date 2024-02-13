

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE   [TA].[GetCheckListByTAID] 
	@TravelAuthorizationID nvarchar(200)
AS
BEGIN

SELECT cl.CheckListID, TravelAuthorizationID, LookupID, [Value], Note,DateCreated, CreatedBy, DateModified, ModifiedBy, isDeleted
FROM 
[TA].[CheckList] cl
WHERE 
cl.TravelAuthorizationID = @TravelAuthorizationID
AND cl.isDeleted = 0

END


