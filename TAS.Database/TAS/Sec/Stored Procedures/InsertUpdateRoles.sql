-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[InsertUpdateRoles]
@RoleID nvarchar(100),
@RoleName nvarchar(500),
@IsAdmin bit,
@IsFinance bit,
@CreatedBy nvarchar(100)
AS
BEGIN

IF @RoleID =''
BEGIN
	DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
	DECLARE @NewID UNIQUEIDENTIFIER
	INSERT INTO Sec.Roles 
		(RoleName,IsAdmin,IsFinance,CreatedBy)
	OUTPUT INSERTED.RoleID INTO @myNewPKTable
	VALUES
		(@RoleName,@IsAdmin,@IsFinance, @CreatedBy)
	SET @NewID = (SELECT * FROM @myNewPKTable)
	INSERT INTO [Sec].[RolesPermissions] (PageID,RoleID,[Permissions])
	(SELECT PageID,@NewID,0 FROM Sec.Pages)
END
ELSE
BEGIN
	UPDATE Sec.Roles
	SET
		RoleName = @RoleName,
		IsAdmin = @IsAdmin,
		IsFinance = @IsFinance,
		ModifiedBy = @CreatedBy,
		DateModified = GETDATE()
	WHERE RoleID = @RoleID
END

END


