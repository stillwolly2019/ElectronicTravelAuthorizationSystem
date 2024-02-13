
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[ADLogOut]
@UserName nvarchar(50)
AS
BEGIN
	UPDATE 
		Sec.Users 
	SET 
		IsLoggedIn = 0
	WHERE
		Username = @UserName AND
		IsDeleted = 0
END


