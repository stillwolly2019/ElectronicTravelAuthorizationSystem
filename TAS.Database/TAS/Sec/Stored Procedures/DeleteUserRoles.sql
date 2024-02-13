-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[DeleteUserRoles]
@UserID nvarchar(100)
AS
BEGIN
DELETE
FROM            Sec.UsersRoles
WHERE UserID = @UserID

END


