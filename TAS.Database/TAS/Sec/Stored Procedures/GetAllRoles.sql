-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetAllRoles]
AS
BEGIN
SELECT        *
FROM            Sec.Roles
WHERE IsDeleted = 0
ORDER BY	RoleName

END


