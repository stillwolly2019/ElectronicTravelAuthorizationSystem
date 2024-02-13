CREATE PROCEDURE [TEC].[GetTECExpenditureByTravelAuthorizationNumber]
@TravelAuthorizationNumber nvarchar(100)
AS
BEGIN
	
SELECT	
		TEC.TECExpenditure.TECExpenditureID, TEC.TECExpenditure.TravelAuthorizationNumber, REPLACE(CONVERT(nvarchar(11), TEC.TECExpenditure.ExpenditureDate,106),' ','-') AS ExpenditureDate, 
		TEC.TECExpenditure.ExpenseAmount, ISNULL(TEC.TECExpenditure.Rate,0) AS Rate, ISNULL(TEC.TECExpenditure.RateAmount,0) AS RateAmount, 
		ISNULL(TEC.TECExpenditure.LocalAmount,0) AS LocalAmount, LkpCurr.CurrencyName, TEC.TECExpenditure.CurrencyID, TEC.TECExpenditure.TECExpenditureID, TEC.TECExpenditure.ExpenditureDetails
	FROM    
		TEC.TECExpenditure 

		INNER JOIN Lookups.Lkp.Currency LkpCurr ON TEC.TECExpenditure.CurrencyID = LkpCurr.CurrencyID
	WHERE
		TEC.TECExpenditure.TravelAuthorizationNumber = @TravelAuthorizationNumber
		AND TEC.TECExpenditure.isDeleted = 0
		
    order by CONVERT(DATE,ExpenditureDate) 
END
