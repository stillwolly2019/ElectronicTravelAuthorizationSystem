-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetUserMenu] --'cd24c636-d8df-4382-b74d-5f94d78c18f7','../Admin/Pages.aspx'
	@UserID nvarchar(100)
AS
BEGIN

	SELECT DISTINCT
		Pages.PageID, 
		Pages.PageName, 
		Pages.PageURL, 
		Pages.ParentID, 
		Pages.PageOrder
	FROM 
		[Sec].Pages, (
				SELECT PageID, [Permissions] FROM [Sec].RolesPermissions WHERE RoleID IN 
					(SELECT RoleID FROM [Sec].UsersRoles WHERE UserID=@UserID)
				) RolesPages
	WHERE 
		RolesPages.PageID=Pages.PageID AND 
		(RolesPages.[Permissions] & 4096)>0 AND
		IsDisplayedInMenu=1 AND 
		Pages.IsDeleted = 0
	ORDER BY
		PageOrder, PageName
END