-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[Get_User_Pages]
	@UserName nvarchar(50) 
AS
BEGIN
	SELECT 
		[Sec].Pages.PageID, [Sec].Pages.PageName, [Sec].Pages.PageURL, [Sec].Pages.ParentID,Sec.Pages.PageOrder,
		case (RolesPermissions.[Permissions] & 65536) when 0 then 0 else 1 end as [Amend],
		case (RolesPermissions.[Permissions] & 4096) when 0 then 0 else 1 end as [Read],
		case (RolesPermissions.[Permissions] & 256) when 0 then 0 else 1 end as [Edit],
		case (RolesPermissions.[Permissions] & 16) when 0 then 0 else 1 end as [Add],
		case (RolesPermissions.[Permissions]& 1) when 0 then 0 else 1 end as [Delete]
	FROM 
		[sec].pages, (
				select PageID, [Permissions] FROM [Sec].RolesPermissions WHERE RoleID IN 
					(SELECT RoleID from [Sec].UsersRoles WHERE UserID = (SELECT UserID FROM Sec.Users WHERE Username = @UserName AND IsDeleted=0))
				) RolesPermissions
	WHERE 
		RolesPermissions.PageID = Sec.Pages.PageID
	ORDER BY
		PageOrder, PageName
END


