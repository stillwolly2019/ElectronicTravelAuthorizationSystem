


CREATE PROCEDURE [TA].[InsertUpdateWBS]

@WBSID nvarchar(100),
@TravelAuthorizationID nvarchar(100),
@WBSCode nvarchar(150),
@PercentageOrAmount float,
@Note nvarchar(500),
@IsPercentage bit,
@CreatedBy nvarchar(100)

AS
BEGIN
BEGIN TRY ------------BEGIN TRY
BEGIN TRANSACTION;-- BEGIN Main Transaction

IF @WBSID =''
BEGIN
       -- Check duplicat WBS for the same TA
      IF ((SELECT COUNT(WBSID) FROM [TA].[WBS] WHERE TravelAuthorizationID = @TravelAuthorizationID AND WBSCode = @WBSCode AND PercentageOrAmount = @PercentageOrAmount AND Note = @Note and isDeleted = 0) = 0)
      BEGIN

       INSERT INTO [TA].[WBS]
           (
		   [TravelAuthorizationID]
           ,[WBSCode]
           ,[PercentageOrAmount]
		   ,[Note]
           ,[IsPercentage]
           ,[CreatedBy]
		   )
            VALUES
	       (
		   @TravelAuthorizationID,
		   @WBSCode,
		   @PercentageOrAmount,
		   @Note,
		   @IsPercentage,
		   @CreatedBy
		   )

      SELECT 1
      END
	  ELSE
	  BEGIN
	  SELECT 0
	  END

END
ELSE
BEGIN

      -- Check duplicat WBS for the same TA
      IF ((SELECT COUNT(WBSID) FROM [TA].[WBS] WHERE TravelAuthorizationID = @TravelAuthorizationID AND WBSCode = @WBSCode AND PercentageOrAmount = @PercentageOrAmount AND Note = @Note and isDeleted = 0 AND WBSID <> @WBSID) = 0)
      BEGIN
       UPDATE [TA].[WBS]
   SET 
      [TravelAuthorizationID] = @TravelAuthorizationID,
      [WBSCode] = @WBSCode,
      [PercentageOrAmount] = @PercentageOrAmount,
      [Note] = @Note,
	  [IsPercentage] = @IsPercentage,
      [CreatedBy] = @CreatedBy,
	  UpdatedDate = GETDATE()
   WHERE 
      WBSID = @WBSID
	  
	   UPDATE TA.WBS SET
		IsPercentage = @IsPercentage
	WHERE
		TravelAuthorizationID = @TravelAuthorizationID
	
	  SELECT 1
      END
	  ELSE
	  BEGIN
	  SELECT 0
	  END

END

COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION;
		SELECT 0
END CATCH --------END CATCH
END



