


CREATE PROCEDURE [TA].[InsertUpdateTravelItinerary]

@TravelItineraryID nvarchar(100),
@TravelAuthorizationNumber nvarchar(14),
@ModeOfTravelID nvarchar(100),
@FromLocationCode nvarchar(100),
@FromLocationDate date,
@ToLocationCode nvarchar(100),
@ToLocationDate date,
@CreatedBy nvarchar(100),
@Ordering int

AS
BEGIN
BEGIN TRY ------------BEGIN TRY
BEGIN TRANSACTION;-- BEGIN Main Transaction

DECLARE @PrevToDate as date;
DECLARE @NextFromDate as date;

IF @TravelItineraryID =''
BEGIN
	 -- Check duplicat Itinerary for the same TA
      IF ((SELECT COUNT(TravelItineraryID) FROM [TA].TravelItinerary WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber AND FromLocationCode = @FromLocationCode AND FromLocationDate = @FromLocationDate AND ToLocationCode = @ToLocationCode AND ToLocationDate = @ToLocationDate and isDeleted = 0) = 0)
      BEGIN

      DECLARE @myNewPKTable TABLE (myNewPK UNIQUEIDENTIFIER)
      DECLARE @NewID UNIQUEIDENTIFIER
      
	  -- check if from date is bigger that prev to date
	  set @PrevToDate = (SELECT TOP 1 [ToLocationDate] FROM [TA].[TravelItinerary] where isDeleted=0 AND TravelAuthorizationNumber = @TravelAuthorizationNumber  order by Ordering desc);
	  
	  IF(CONVERT(date,@FromLocationDate,101) < CONVERT(date,ISNULL(@PrevToDate,'1990-01-01'),101) AND @PrevToDate IS NOT NULL)
	  BEGIN
	  SELECT 2
	  END 
	  ELSE
	  BEGIN
	  INSERT INTO [TA].TravelItinerary
           (
		   [TravelAuthorizationNumber]
           ,[ModeOfTravelID]
           ,[FromLocationCode]
           ,[FromLocationDate]
           ,[ToLocationCode]
           ,[ToLocationDate]
           ,[CreatedBy]
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
		   ,@Ordering
		   )
	  
	  SET @NewID = (SELECT * FROM @myNewPKTable)
	  INSERT INTO TEC.TECItinerary (TravelItineraryID,TravelAuthorizationNumber,NoOfDays,DSARate,RateAmount,LocalAmount, [StatusCode] ,CreatedBy)
	  SELECT @NewID, TravelAuthorizationNumber, 0,0,0,0, 'PEN',@CreatedBy FROM TA.TravelItinerary WHERE TravelItineraryID = @NewID

	  SELECT 1
	  END
	  -- END of checking   
	 END
	 ELSE
	 BEGIN
	 SELECT 0
	 END
END
ELSE
BEGIN
 -- Check duplicat Itinerary for the same TA
     IF ((SELECT COUNT(TravelItineraryID) FROM [TA].TravelItinerary WHERE TravelAuthorizationNumber = @TravelAuthorizationNumber AND FromLocationCode = @FromLocationCode AND FromLocationDate = @FromLocationDate AND ToLocationCode = @ToLocationCode AND ToLocationDate = @ToLocationDate and isDeleted = 0 and TravelItineraryID <> @TravelItineraryID) = 0)
     BEGIN

	 	  -- check if from date is bigger that prev to date
	  set @PrevToDate = (SELECT TOP 1 [ToLocationDate] FROM [TA].[TravelItinerary] where isDeleted=0 AND TravelAuthorizationNumber = @TravelAuthorizationNumber AND TravelItineraryID <> @TravelItineraryID and Ordering < @Ordering  order by Ordering desc);
	  SET @NextFromDate = (SELECT TOP 1 [FromLocationDate] FROM [TA].[TravelItinerary] where isDeleted=0 AND TravelAuthorizationNumber = @TravelAuthorizationNumber AND TravelItineraryID <> @TravelItineraryID and Ordering > @Ordering  order by Ordering ASC);
	  IF(CONVERT(date,@FromLocationDate,101) < CONVERT(date,ISNULL(@PrevToDate,'1990-01-01'),101) AND @PrevToDate IS NOT NULL)
	  BEGIN
		SELECT 2
	  END
	  ELSE IF (CONVERT(date,@ToLocationDate,101) > CONVERT(date,ISNULL(@NextFromDate,'1990-01-01'),101) AND @NextFromDate IS NOT NULL)
	  BEGIN
		SELECT 3
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
			   Ordering=@Ordering
			   where 
			   TravelItineraryID = @TravelItineraryID

		 SELECT 1
	 END
	 -- END of checking 

	 END
	 ELSE
	 BEGIN
		SELECT 0
	 END
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
		SELECT 0
END CATCH --------END CATCH
END



