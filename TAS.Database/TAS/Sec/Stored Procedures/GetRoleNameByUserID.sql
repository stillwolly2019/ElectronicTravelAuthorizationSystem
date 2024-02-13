

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetRoleNameByUserID]
@UserID nvarchar(150)
AS
BEGIN
SELECT DISTINCT RoleName FROM Sec.UsersRoles ur 
INNER JOIN Sec.Roles r ON r.RoleID = ur.RoleID
WHERE ur.UserID = @UserID
END

