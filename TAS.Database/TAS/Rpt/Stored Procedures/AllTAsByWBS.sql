CREATE PROCEDURE [Rpt].[AllTAsByWBS]



AS


SELECT DISTINCT WBS = [WBSCode],
                ta.TravelAuthorizationNumber
      
FROM   TA.WBS INNER JOIN
       TA.TravelAuthorization ta ON ta.TravelAuthorizationID = WBS.TravelAuthorizationID AND ta.isDeleted = 0

WHERE wbs.isDeleted = 0 

ORDER BY [WBSCode],ta.TravelAuthorizationNumber