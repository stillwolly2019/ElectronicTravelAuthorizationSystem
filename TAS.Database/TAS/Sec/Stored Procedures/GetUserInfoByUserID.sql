
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[GetUserInfoByUserID]
	@UserID nvarchar(100)
AS
BEGIN
	
		SELECT UserID,
		 Username,
		 FirstName, 
		 LastName, 
		 Email
        FROM sec.Users u WHERE u.UserID = @UserID

END

