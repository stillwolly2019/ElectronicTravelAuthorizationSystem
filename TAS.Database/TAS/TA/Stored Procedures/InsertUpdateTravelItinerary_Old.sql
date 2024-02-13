

CREATE PROCEDURE [TA].[InsertUpdateTravelItinerary_Old]

@TravelItineraryID nvarchar(100),
@TravelAuthorizationNumber nvarchar(14),
@ModeOfTravelID nvarchar(100),
@FromLocationCode nvarchar(100),
@FromLocationDate date,
@ToLocationCode nvarchar(100),
@ToLocationDate date,
@CreatedBy nvarchar(100),
@isDeleted bit,
@Ordering int,
@isPrivateDeviation bit,
@pdIsDeleted bit

AS
BEGIN
BEGIN TRY ------------BEGIN TRY
BEGIN TRANSACTION;-- BEGIN Main Transaction


IF @TravelItineraryID =''
BEGIN
	
DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
DECLARE @NewID UNIQUEIDENTIFIER

INSERT INTO [TA].TravelItinerary
           (
		   [TravelAuthorizationNumber]
           ,[ModeOfTravelID]
           ,[FromLocationCode]
           ,[FromLocationDate]
           ,[ToLocationCode]
           ,[ToLocationDate]
           ,[CreatedBy]
		   ,isDeleted
		   ,Ordering
		   )
	 OUTPUT INSERTED.TravelItineraryID INTO @myNewPKTable
     VALUES
           (
           @TravelAuthorizationNumber,
           @ModeOfTravelID,
           @FromLocationCode ,
           @FromLocationDate,
           @ToLocationCode ,
           @ToLocationDate ,
           @CreatedBy
		   ,@isDeleted
		   ,@Ordering
		   )
	SET @NewID = (SELECT * FROM @myNewPKTable)
	INSERT INTO TEC.TECItinerary (TravelItineraryID,TravelAuthorizationNumber,NoOfDays,DSARate,RateAmount,LocalAmount, [StatusCode] ,CreatedBy)
	SELECT @NewID, TravelAuthorizationNumber, 0,0,0,0, 'PEN',@CreatedBy FROM TA.TravelItinerary WHERE TravelItineraryID = @NewID

	if(@isPrivateDeviation=1)
	begin

	INSERT INTO TA.PrivateDeviation(TravelItineraryID,TravelAuthorizationNumber,CreatedBy,DateCreated,isDeleted)
	SELECT @NewID, @TravelAuthorizationNumber,@CreatedBy,GETDATE(),@isDeleted FROM TA.TravelItinerary WHERE TravelItineraryID = @NewID
	end



END
ELSE
BEGIN
UPDATE [TA].TravelItinerary
SET
		 
           [TravelAuthorizationNumber] = @TravelAuthorizationNumber,
           [ModeOfTravelID] = @ModeOfTravelID,
           [FromLocationCode] = @FromLocationCode,
           [FromLocationDate] = @FromLocationDate,
           [ToLocationCode] = @ToLocationCode,
           [ToLocationDate] = @ToLocationDate,
           [CreatedBy] = @CreatedBy,
		   UpdatedDate = GETDATE(),
		   isDeleted=@isDeleted,
		   Ordering=@Ordering
		   where 
		   TravelItineraryID = @TravelItineraryID


		   If((select Count(TravelItineraryID) from TA.PrivateDeviation where  TravelItineraryID=@TravelItineraryID )>0)
		   Begin 
		   update TA.PrivateDeviation 
		   set TravelItineraryID=@TravelItineraryID ,TravelAuthorizationNumber=@TravelAuthorizationNumber,ModifiedBy=@CreatedBy,DateModified=getDAte(),isDeleted=@pdIsDeleted
		   where TravelItineraryID=@TravelItineraryID
		   end
		 
		   else 
		   Begin
		   
		   if(@isPrivateDeviation=1)
		   begin
		    
		    INSERT INTO TA.PrivateDeviation(TravelItineraryID,TravelAuthorizationNumber,CreatedBy,DateCreated,isDeleted)
	       SELECT @TravelItineraryID, @TravelAuthorizationNumber,@CreatedBy,GETDATE(),@isDeleted FROM TA.TravelItinerary WHERE TravelItineraryID = @TravelItineraryID
	       end
		   end





END


IF OBJECT_ID('tempdb..#TT') IS NOT NULL DROP TABLE #TT
SELECT TravelItineraryID,ROW_NUMBER() OVER(PARTITION BY TravelAuthorizationNumber ORDER BY CreatedDate ASC) RN INTO #TT FROM TA.TravelItinerary WHERE isDeleted = 0

UPDATE ti 
SET ti.Ordering = tr.RN
FROM TA.TravelItinerary ti INNER JOIN
#TT tr ON ti.TravelItineraryID = tr.TravelItineraryID
WHERE ti.TravelAuthorizationNumber = @TravelAuthorizationNumber

COMMIT TRANSACTION;-- COMMIT Main Transaction
END TRY ------------END TRY
BEGIN CATCH --------BEGIN CATCH
    IF @@TRANCOUNT > 0
    	ROLLBACK TRANSACTION;
END CATCH --------END CATCH
END


