CREATE procedure [Sec].[GetRolePages]
	@RoleID nvarchar(100),
	@ParentID nvarchar(100)
AS

BEGIN
	SET XACT_ABORT ON;
	SELECT
		Pages.PageID, Pages.PageName,
		(CASE ISNULL(ParentID, '00000000-0000-0000-0000-000000000000') WHEN '00000000-0000-0000-0000-000000000000' THEN '' ELSE ' -----------> ' END)AS PageSeparator,
		CASE (Sec.RolesPermissions.[Permissions] & 65536) WHEN 0 THEN 0 ELSE 1 END AS [Amend],
		CASE (Sec.RolesPermissions.[Permissions] & 4096) WHEN 0 THEN 0 ELSE 1 END AS [Read],
		CASE (Sec.RolesPermissions.[Permissions] & 256) WHEN 0 THEN 0 ELSE 1 END AS [Edit],
		CASE (Sec.RolesPermissions.[Permissions] & 16) WHEN 0 THEN 0 ELSE 1 END As [Add],
		CASE (Sec.RolesPermissions.[Permissions]& 1) WHEN 0 THEN 0 ELSE 1 END AS [Delete]
		
	FROM
		(SELECT * FROM [Sec].Pages WHERE @ParentID='00000000-0000-0000-0000-000000000000' OR @ParentID IS NULL OR PageID=@ParentID OR ParentID=@ParentID)	Pages 
			LEFT OUTER JOIN
			Sec.RolesPermissions ON Pages.PageID = RolesPermissions.PageID
		WHERE
		(@RoleID IS NULL OR @RoleID='00000000-0000-0000-0000-000000000000') OR RoleID=@RoleID AND Pages.IsDeleted=0
		ORDER BY
		CASE WHEN ParentID = '00000000-0000-0000-0000-000000000000' THEN Pages.PageID
		ELSE ParentID END, ParentID, PageOrder, PageName	

END 