-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[DeleteRoles]
@RoleID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN

	UPDATE Sec.Roles
	SET
		IsDeleted = 1,
		ModifiedBy = @CreatedBy,
		DateModified = GETDATE()
	WHERE RoleID = @RoleID
END
