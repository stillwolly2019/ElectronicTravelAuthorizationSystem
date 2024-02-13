CREATE PROCEDURE [TEC].[DeleteTECItineraryDSA]
@TECItineraryDSAID nvarchar(100),
@TECItineraryID nvarchar(100),
@CreatedBy nvarchar(100)
AS
UPDATE TEC.TECItineraryDSA SET
	isDeleted = 1,
	UpdatedBy = @CreatedBy,	
	UpdatedDate = GETDATE()
WHERE
	TECItineraryDSAID = @TECItineraryDSAID

--UPDATE [TEC].[TECItinerary] SET
--	NoOfDays = (SELECT ISNULL(SUM(NoOfDays),0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
--	DSARate = (SELECT TOP 1 ISNULL(DSARate,0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
--	RateAmount = (SELECT ISNULL(SUM(RateAmount),0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
--	ExchangeRate = (SELECT TOP 1 ISNULL(ExchangeRate,0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0),
--	LocalAmount = (SELECT ISNULL(SUM(LocalAmount),0) FROM TEC.TECItineraryDSA WHERE TECItineraryID = @TECItineraryID AND isDeleted = 0)
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