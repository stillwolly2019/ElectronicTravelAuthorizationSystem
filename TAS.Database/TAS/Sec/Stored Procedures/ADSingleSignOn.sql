CREATE PROCEDURE [Sec].[ADSingleSignOn]
@UserName nvarchar(50)
AS
BEGIN
	UPDATE 
		Sec.Users 
	SET 
		IsLoggedIn = 1
	WHERE
		Username = @UserName AND
		IsDeleted = 0
END
