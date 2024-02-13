

CREATE PROCEDURE [TEC].[GetTECAdvancesByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(100)
AS
BEGIN
	

	SELECT	
		TECAdvancesID,TravelAuthorizationNumber, PayOfficeCode, PayOfficeCodeID ,POC.[Description] AS  PayOfficeCodeDesc,REPLACE(CONVERT(nvarchar(11), DatePaid,106),' ','-') AS DatePaid, TEC.TECAdvances.CurrencyID, 
		ISNULL(AdvanceAmount,0) AS AdvanceAmount, ISNULL(Rate,0) AS Rate, ISNULL(RateAmount,0) AS RateAmount, ISNULL(LocalAmount,0) AS LocalAmount,
		LkpCurr.CurrencyName
	FROM
		TEC.TECAdvances
		INNER JOIN Lookups.Lkp.Currency LkpCurr ON TEC.TECAdvances.CurrencyID = LkpCurr.CurrencyID
		INNER JOIN Lkp.Lookups POC ON POC.LookupsID = TEC.TECAdvances.PayOfficeCodeID
	WHERE
		TEC.TECAdvances.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TEC.TECAdvances.isDeleted = 0
END

