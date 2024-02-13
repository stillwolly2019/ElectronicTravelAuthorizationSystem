-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[DeletePages]
@PageID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN

	UPDATE Sec.Pages
	SET
		IsDeleted = 1,
		ModifiedBy = @CreatedBy,
		DateModified = GETDATE()
	WHERE PageID = @PageID
END
