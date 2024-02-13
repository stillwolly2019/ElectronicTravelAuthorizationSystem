CREATE PROCEDURE [Rpt].[TravelAuthorizationFormWBS]

@TravelAuthorizationNumber VARCHAR(MAX)

AS


SELECT
      [Name] = TA.TravelersName,
	  [TravelAuthorizationNumber] = TA.TravelAuthorizationNumber,
      [Count] = ROW_NUMBER() OVER(ORDER BY wbs.[WBSID] DESC),
      WBS = [WBSCode],
      [Percentage] = ISNULL(CASE WHEN wbs.IsPercentage = 1 THEN '%' ELSE '' END,' '),
      [Amount] =  PercentageOrAmount,
	  [Notes] = wbs.Note,
	  [Perc] = CASE WHEN wbs.IsPercentage = 1 THEN 20 ELSE  40 END,
	  [Amm] = CASE WHEN wbs.IsPercentage <> 1 THEN 20 ELSE  40 END

   FROM TA.TravelAuthorization TA INNER JOIN 
        TA.WBS wbs ON wbs.TravelAuthorizationID = TA.[TravelAuthorizationID] 

      WHERE wbs.isDeleted = 0 AND
         @TravelAuthorizationNumber = TA.TravelAuthorizationNumber



 