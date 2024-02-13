CREATE PROCEDURE [Sec].[InsertNewUsers]
AS
BEGIN
	INSERT INTO Sec.Users (Username, [Password], FirstName, LastName, Email, CreatedBy)
		SELECT 
			ADU.UserName, '', ADU.FirstName, ADU.LastName, ADU.Email, '00000000-0000-0000-0000-000000000000' 
		FROM	
			[ActiveDirectoryUsers].Sec.Users ADU LEFT OUTER JOIN
			Sec.Users U ON U.UserName = ADU.UserName WHERE U.UserID IS NULL and ADU.Country='Jordan'

	INSERT INTO Sec.UsersRoles (UserID, RoleID)
		SELECT Sec.Users.UserID ,'6b0ae980-4429-4f77-bf48-a72d872b9a65' FROM Sec.Users LEFT OUTER JOIN
		Sec.UsersRoles ON Sec.Users.UserID = Sec.UsersRoles.UserID
		WHERE Sec.UsersRoles.UserID IS NULL
END

