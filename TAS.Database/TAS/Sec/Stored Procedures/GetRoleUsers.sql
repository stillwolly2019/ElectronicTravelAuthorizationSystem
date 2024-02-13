-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetRoleUsers]
@RoleID nvarchar(100)
AS
BEGIN
SELECT        *
FROM            Sec.UsersRoles
WHERE RoleID = @RoleID AND IsDeleted = 0

END


