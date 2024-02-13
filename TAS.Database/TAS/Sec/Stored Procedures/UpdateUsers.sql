
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[UpdateUsers]
@UserID nvarchar(100),
@IsManager bit,
@CreatedBy nvarchar(100)
AS
BEGIN
BEGIN TRY 
    BEGIN TRANSACTION; 

			UPDATE Sec.Users
			SET
				IsManager = @IsManager,
				ModifiedBy = @CreatedBy,
				DateModified = GETDATE()
			WHERE UserID = @UserID

COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
	Begin
    	ROLLBACK TRANSACTION;
	End
END CATCH --------END CATCH
END

