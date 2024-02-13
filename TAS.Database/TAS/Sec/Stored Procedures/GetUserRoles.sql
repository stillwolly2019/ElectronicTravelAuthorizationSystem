-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetUserRoles]
@UserID nvarchar(100)
AS
BEGIN
SELECT        *
FROM            Sec.UsersRoles
WHERE UserID = @UserID AND IsDeleted = 0

END


