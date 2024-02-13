-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetDepUsers]
	@RoleID nvarchar(100),

@Department NVARCHAR(30) 
AS
BEGIN
	
		SELECT distinct u.[FirstName] + ' ' + u.[LastName] AS FullName , u.UserID, r.RoleName 
							FROM Sec.Users u
							INNER JOIN sec.UsersRoles ur ON ur.UserID = u.UserID
							INNER JOIN Sec.Roles r ON r.RoleID = ur.RoleID
							INNER JOIN ActiveDirectoryUsers.Sec.Users asu ON u.Username = asu.UserName
							
						WHERE 
							 asu.Country = 'Jordan'  AND asu.Department  like  @Department +'%' and 
						(r.RoleName  = 'Manager' or r.RoleName='System Administrator') -- AND u.UserID = t.UserID   
						--ORDER BY u.Username

END
