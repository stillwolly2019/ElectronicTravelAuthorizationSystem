-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[DeleteRoleUsers]
@RoleID nvarchar(100)
AS
BEGIN
DELETE
FROM            Sec.UsersRoles
WHERE RoleID = @RoleID

END


