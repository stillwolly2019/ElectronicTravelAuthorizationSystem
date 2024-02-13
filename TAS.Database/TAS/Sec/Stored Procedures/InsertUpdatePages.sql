-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[InsertUpdatePages]
@PageID nvarchar(100),
@PageName nvarchar(500),
@PageURL nvarchar(4000),
@ParentID nvarchar(100),
@PageOrder int,
@IsDisplayedInMenu bit,
@CreatedBy nvarchar(100)
AS
BEGIN

IF @PageID =''
BEGIN
	DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
	DECLARE @NewID UNIQUEIDENTIFIER
	INSERT INTO Sec.Pages 
		(PageName,PageURL,ParentID,PageOrder,IsDisplayedInMenu,CreatedBy)
	OUTPUT INSERTED.PageID INTO @myNewPKTable
	VALUES
		(@PageName,@PageURL,@ParentID,@PageOrder,@IsDisplayedInMenu,@CreatedBy)
	SET @NewID = (SELECT * FROM @myNewPKTable)
	INSERT INTO [Sec].[RolesPermissions] (PageID,RoleID,[Permissions])
	(SELECT @NewID,RoleID,0 FROM Sec.Roles)
END
ELSE
BEGIN
	UPDATE Sec.Pages
	SET
		PageName = @PageName,
		PageURL = @PageURL,
		ParentID = @ParentID,
		PageOrder = @PageOrder,
		IsDisplayedInMenu = @IsDisplayedInMenu,
		ModifiedBy = @CreatedBy,
		DateModified = GETDATE()
	WHERE PageID = @PageID
END

END


