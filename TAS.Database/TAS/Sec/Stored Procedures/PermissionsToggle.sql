CREATE procedure [Sec].[PermissionsToggle]
	@RoleID nvarchar(100),
	@PageID nvarchar(100),
	@Read bit,
	@Edit bit,
	@Add bit,
	@Delete bit,
	@Amend bit
as
BEGIN

	DECLARE @Permissions int
	SELECT @Permissions = [Permissions] FROM [Sec].RolesPermissions WHERE RoleID=@RoleID AND PageID=@PageID
	
	-- this will toggle the 6th hex digit 
	IF @Amend=1 BEGIN SET @Permissions = @Permissions ^ 65536 END
	-- this will toggle the 4th hex digit 
	IF @read=1 BEGIN SET @Permissions = @Permissions ^ 4096 END
	-- this will toggle the 3rd hex digit
	IF @Edit=1 BEGIN SET @Permissions = @Permissions ^ 256 END
	-- this will toggle the 2nd hex digit
	IF @Add=1 BEGIN SET @permissions = @Permissions ^ 16 END
	-- this will toggle the 1st hex digit
	IF @Delete=1 BEGIN SET @Permissions = @Permissions ^ 1 END
		
	

	UPDATE 
		[Sec].RolesPermissions
	SET 
		[Permissions]=@Permissions
	WHERE 
		RoleID=@RoleID AND PageID=@PageID
END