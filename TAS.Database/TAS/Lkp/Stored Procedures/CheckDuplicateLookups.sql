





CREATE PROCEDURE [Lkp].[CheckDuplicateLookups]
@LookupsID nvarchar(100),
@LookupGroupID nvarchar(100),
@SubGroupID nvarchar(100),
@Description nvarchar(250)

AS
BEGIN

If(@LookupsID ='00000000-0000-0000-0000-000000000000')
Begin 


SELECT * FROM Lkp.Lookups WHERE LookupGroupID = @LookupGroupID  AND SubGroupID = @SubGroupID AND [Description] = @Description 
                                 --AND LookupGroupID <> @LookupGroupID
								  AND IsDeleted = 0
End
Else
Begin 

SELECT * FROM Lkp.Lookups WHERE LookupGroupID = @LookupGroupID  AND SubGroupID = @SubGroupID AND [Description] = @Description 
                                 --AND LookupGroupID <> @LookupGroupID
								  AND IsDeleted = 0 and LookupsID<>@LookupsID
End

END








