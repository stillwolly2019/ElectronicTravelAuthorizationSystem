CREATE PROCEDURE [TEC].[InsertUpdateTECItineraryDSA]
@TECItineraryDSAID nvarchar(100),
@TECItineraryID nvarchar(100),
@NoOfDays float,
@DSARate float,
@Percentage float,
@RateAmount float,
@ExchangeRate float,
@LocalAmount float,
@CreatedBy nvarchar(100)
AS

IF @TECItineraryDSAID = '00000000-0000-0000-0000-000000000000'
	BEGIN
		INSERT INTO TEC.TECItineraryDSA
			(TECItineraryID, NoOfDays, DSARate, Percentage, RateAmount, ExchangeRate, LocalAmount, CreatedBy)
		VALUES
			(@TECItineraryID, @NoOfDays, @DSARate, @Percentage, @RateAmount, ROUND(@ExchangeRate,3), @LocalAmount, @CreatedBy)
	END
ELSE
	BEGIN
		UPDATE TEC.TECItineraryDSA SET
			TECItineraryID = @TECItineraryID, 
			NoOfDays = @NoOfDays, 
			DSARate = @DSARate, 
			Percentage = @Percentage, 
			RateAmount = @RateAmount, 
			ExchangeRate = @ExchangeRate, 
			LocalAmount = @LocalAmount, 
			UpdatedBy = @CreatedBy,	
			UpdatedDate = GETDATE()
		WHERE
			TECItineraryDSAID = @TECItineraryDSAID
	END

--UPDATE TEC.TECItineraryDSA SET
--	DSARate = @DSARate,
--	RateAmount = @DSARate * (Percentage / 100) * NoOfDays,
--	LocalAmount = @DSARate * (Percentage / 100) * NoOfDays * @ExchangeRate
--WHERE
--	TECItineraryID = @TECItineraryID

UPDATE [TEC].[TECItinerary] SET
	NoOfDays = (SELECT ISNULL(SUM(NoOfDays),0.0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
	NoOfNights = (SELECT Sum(NoOfDays * (Percentage / 100)) AS NoOfNights FROM [TEC].[TECItineraryDSA] WHERE TECItineraryID = @TECItineraryID AND isDeleted=0),
	DSARate = (SELECT TOP 1 ISNULL(DSARate,0.0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
	RateAmount = (SELECT ISNULL(SUM(RateAmount),0.0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
	ExchangeRate = (SELECT TOP 1 ISNULL(ExchangeRate,0.0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
	LocalAmount = (SELECT ISNULL(SUM(LocalAmount),0.0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
	AllDSARates = (SELECT distinct STUFF((SELECT ' | ' + convert(nvarchar,ISNULL(DSARate,0.0)) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0 FOR XML PATH('')), 2, 1, '') DSARate FROM TEC.TECItineraryDSA  WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0)
WHERE
	TECItineraryID = @TECItineraryID
