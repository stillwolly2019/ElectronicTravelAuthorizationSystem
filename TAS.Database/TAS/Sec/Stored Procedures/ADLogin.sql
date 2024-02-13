CREATE PROCEDURE [Sec].[ADLogin] 
	@UserName nvarchar(50)
AS
BEGIN

	
SET XACT_ABORT ON;

BEGIN TRY ------------BEGIN TRY
    BEGIN TRANSACTION;-- BEGIN Main Transaction
	SELECT
		u.UserID,
		ISNULL(u.UserName,'') AS UserName,
		ISNULL(u.FirstName,'') AS FirstName,
		ISNULL(u.LastName,'') AS LastName,
		ISNULL(u.Email,'') AS Email ,
		ISNULL(asu.DepartmentID,'00000000-0000-0000-0000-000000000000') as DepartmentID,
		ISNULL(asu.UnitID,'00000000-0000-0000-0000-000000000000') as UnitID,
		ISNULL(asu.SubUnitID,'00000000-0000-0000-0000-000000000000') as SubUnitID,
		ISNULL (u.IsManager,0) as IsManager,
		ISNULL(IsLoggedIn,0) as IsLoggedIn
	FROM
		[Sec].Users u

		CROSS apply (
		select DepartmentID,UnitID,SubUnitID from  ActiveDirectoryUsers.Sec.UsersWithPRISMNo where username = @UserName
		
		)asu

			--left JOIN ActiveDirectoryUsers.Sec.Users asu ON u.Username = asu.UserName
						
	WHERE
		u.UserName=@UserName AND u.IsDeleted =0

		
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
