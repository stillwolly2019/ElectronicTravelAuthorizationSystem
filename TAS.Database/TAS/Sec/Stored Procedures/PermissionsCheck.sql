-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[PermissionsCheck]
	@UserName nvarchar(50) ,
	@page_url nvarchar(150)
AS
BEGIN

SET XACT_ABORT ON;

BEGIN TRY ------------BEGIN TRY
    BEGIN TRANSACTION;-- BEGIN Main Transaction
		-- GET user permissions for page by UserName and page url
		select 
		case (RolesPermissions.[Permissions] & 65536) when 0 then 0 else 1 end as [Amend],
		case (RolesPermissions.[Permissions] & 4096) when 0 then 0 else 1 end as [Read],
		case (RolesPermissions.[Permissions] & 256) when 0 then 0 else 1 end as [Edit],
		case (RolesPermissions.[Permissions] & 16) when 0 then 0 else 1 end as [Add],
		case (RolesPermissions.[Permissions]& 1) when 0 then 0 else 1 end as [Delete]
		from 
		[Sec].[Pages], (
				SELECT PageID, [Permissions] FROM [Sec].RolesPermissions WHERE RoleID IN 
					(SELECT RoleID from [Sec].UsersRoles WHERE UserID = (SELECT UserID FROM Sec.Users WHERE Username = @UserName AND IsDeleted=0))
				) RolesPermissions
		WHERE 
		RolesPermissions.PageID = Sec.Pages.PageID and
		LOWER(PageURL)=@page_url   
	
	COMMIT TRANSACTION;-- COMMIT Main Transaction
	   
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION;
DECLARE
    	@ERROR_SEVERITY INT,
    	@ERROR_STATE	INT,
    	@ERROR_NUMBER	INT,
    	@ERROR_LINE		INT,
    	@ERROR_MESSAGE	NVARCHAR(4000);

    SELECT
    	@ERROR_SEVERITY = ERROR_SEVERITY(),
    	@ERROR_STATE	= ERROR_STATE(),
    	@ERROR_NUMBER	= ERROR_NUMBER(),
    	@ERROR_LINE		= ERROR_LINE(),
    	@ERROR_MESSAGE	= ERROR_MESSAGE();

    RAISERROR('Msg %d, Line %d, :%s',
    	@ERROR_SEVERITY,
    	@ERROR_STATE,
    	@ERROR_NUMBER,
    	@ERROR_LINE,
    	@ERROR_MESSAGE);
END CATCH --------END CATCH

END -- END SP