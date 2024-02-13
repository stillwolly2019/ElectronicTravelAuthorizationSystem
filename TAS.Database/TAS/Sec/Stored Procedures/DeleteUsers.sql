-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[DeleteUsers]
@UserID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN
UPDATE Sec.Users SET 
	IsDeleted = 1,
	ModifiedBy = @CreatedBy,
	DateModified = GETDATE()
WHERE UserID= @UserID
END


