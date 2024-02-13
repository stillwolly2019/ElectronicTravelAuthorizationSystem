-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[CheckDuplicateRoles]
@ID nvarchar(100) ,
	@RoleName nvarchar(500)
  
AS
BEGIN
	 
	 If(@ID='')
	 begin

	Declare @count int
	select @count=count(RoleID) from sec.Roles  
	where RoleName=@RoleName and IsDeleted=0

	if(@count>0) 
	begin
	select 1 as int 
	end

	else
	begin 
	select 0 as int
	end

	end

	else

	begin 

	Select @count=count(RoleID) from Sec.Roles where RoleID!=@ID AND RoleName=@RoleName AND IsDeleted=0
	if(@count>0) 
	begin
	select 1 as int 
	end

	else
	begin 
	select 0 as int
	end
	end
END
