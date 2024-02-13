CREATE PROCEDURE [Sec].[SearchUsersByName]
@SearchText nvarchar(100),
@UserName nvarchar(100), 
@AllUsers bit = 0

AS

	IF @AllUsers = 0
	BEGIN

		SELECT UserID, FirstName +' '+ LastName as DisplayName FROM [Sec].Users
		Where
		(UserName LIKE '' + @SearchText + '%'OR 
		(replace(FirstName + ' '+ LastName, N'''' , '') LIKE N'' + replace(@SearchText, N'''' , '') + '%'))  AND
		UserName <> @UserName
	END
	ELSE
	BEGIN
		SELECT UserID, FirstName +' '+ LastName as DisplayName FROM [Sec].Users
		Where
		(UserName LIKE '' + @SearchText + '%'OR 
		(replace(FirstName + ' '+ LastName, N'''' , '') LIKE N'' + replace(@SearchText, N'''' , '') + '%')) 
	END