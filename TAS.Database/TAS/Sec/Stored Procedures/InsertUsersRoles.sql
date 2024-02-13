-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[InsertUsersRoles]
@UserID nvarchar(100),
@RoleID nvarchar(100)
AS
BEGIN
INSERT INTO Sec.UsersRoles (UserID,RoleID) VALUES (@UserID,@RoleID)

END


