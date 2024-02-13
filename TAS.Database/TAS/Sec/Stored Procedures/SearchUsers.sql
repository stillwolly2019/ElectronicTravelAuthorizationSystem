
CREATE PROCEDURE [Sec].[SearchUsers]-- '','','terro'
@Username nvarchar(100),
@FirstName nvarchar(100),
@LastName nvarchar(100)

AS
BEGIN
	SELECT TOP 100 Sec.Users.UserID, Sec.Users.Username, Sec.Users.FirstName, Sec.Users.LastName, Sec.Users.LastName+' '+ Sec.Users.FirstName  as FullName,
							 Sec.Users.Email, Sec.Users.DateCreated, Sec.Users.CreatedBy, 
							 Sec.Users.DateModified, Sec.Users.ModifiedBy, Sec.Users.IsDeleted , ISNULL(IsManager,0) as IsManager , ISNULL(prism.PRISM_Number,'') PRISM_Number
	FROM            Sec.Users  OUTER APPLY(SELECT [PRISM_Number] FROM [ActiveDirectoryUsers].[Sec].[UsersWithPRISMNo] where UserName =  Sec.Users.Username and DepartmentID is not null) prism
	WHERE  Sec.Users.IsDeleted = 0
	AND (username like '%' + @Username + '%' OR @Username = '')
    AND (FirstName like '%' + @FirstName + '%'  or @FirstName = '')
	AND (LastName like '%' + @LastName + '%'  or @LastName = '')
		
	ORDER BY Sec.Users.LastName, Sec.Users.FirstName
END

