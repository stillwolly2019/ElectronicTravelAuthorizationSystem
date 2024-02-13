-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Sec].[InsertUpdateUsers]
@UserID nvarchar(100),
@Username nvarchar(50),
@FirstName nvarchar(50),
@LastName nvarchar(50),
@Email nvarchar(500),
@IsManager bit,
@CreatedBy nvarchar(100),
@PRISMNumber nvarchar(100)
AS
BEGIN
BEGIN TRY ------------BEGIN TRY
    BEGIN TRANSACTION; -- BEGIN Main Transaction
		IF @UserID =''
		BEGIN
		DECLARE @COUNT INT
		   SELECT @COUNT=COUNT(UserID) FROM Sec.Users WHERE Username = @Username AND IsDeleted = 0
			IF (@Username IS NOT NULL AND @FirstName IS NOT NULL AND @LastName IS NOT NULL AND @Email IS NOT NULL AND @COUNT=0)
			BEGIN
				DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
				DECLARE @NewID UNIQUEIDENTIFIER
	
				INSERT INTO Sec.Users 
					(Username,FirstName,LastName,Email,IsManager,CreatedBy)
				OUTPUT INSERTED.UserID INTO @myNewPKTable
				VALUES
					(@Username,UPPER(SUBSTRING(@FirstName, 1, 1)) +  LOWER(SUBSTRING(@FirstName, 2, 50)),UPPER(@LastName),@Email,@IsManager,@CreatedBy)
				SELECT * FROM @myNewPKTable

				 insert into ActiveDirectoryUsers.Sec.UsersWithPRISMNo 
				 ( UserName,PRISM_Number,Email ) values(@Username,@PRISMNumber,@Email)

			END
			ELSE
			BEGIN
				SELECT '00000000-0000-0000-0000-000000000000' AS myNewPK
			END
		END
		ELSE
		BEGIN
			UPDATE Sec.Users
			SET
				Username=@Username,
				FirstName=UPPER(SUBSTRING(@FirstName, 1, 1)) +  LOWER(SUBSTRING(@FirstName, 2, 50)),
				LastName=UPPER(@LastName),
				Email=@Email,
				IsManager = @IsManager,
				ModifiedBy = @CreatedBy,
				DateModified = GETDATE()
			WHERE UserID = @UserID
			SELECT @UserID
		END
COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
	Begin
    	ROLLBACK TRANSACTION;
		
		SELECT '00000000-0000-0000-0000-000000000001' AS myNewPKErr
	End
END CATCH --------END CATCH
END



