
CREATE PROCEDURE [Rpt].[TravelExpenseClaimCheckList]


@TravelAuthorizationNumber VARCHAR(MAX)

AS


      SELECT DISTINCT
      [Travel Authorization No.] = TA.[TravelAuthorizationNumber],
	  TravelersName = ta.TravelersName,
	  chl.[Description],
	  ch.Value ,ch.Note

	 

  FROM TA.TravelAuthorization TA 
       LEFT JOIN TA.CheckList ch ON ch.TravelAuthorizationID = ta.TravelAuthorizationID
	   CROSS APPLY (SELECT l.[Description] FROM Lkp.Lookups l INNER JOIN 
	                            Lkp.LookupsGroups lg ON lg.LookupGroupID = l.LookupGroupID AND lg.LookupGroup='Check List'
	                WHERE l.LookupsID = ch.LookupID) chl
	   INNER JOIN Sec.Users U ON U.UserID = TA.UserID

  WHERE ta.isDeleted = 0  AND
        @TravelAuthorizationNumber = TA.TravelAuthorizationNumber


