

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Lkp].[InsertUpdateLookupsGroups]

@LookupGroupID nvarchar(100),
@LookupGroup nvarchar(100),
@CreatedBy nvarchar(100)

AS
BEGIN

IF @LookupGroupID =''
BEGIN
	Insert into [Lkp].[LookupsGroups] (LookupGroup,CreatedBy) values (@LookupGroup,@CreatedBy)
END
ELSE
BEGIN
	UPDATE Lkp.LookupsGroups
	SET
		LookupGroup = @LookupGroup,
		ModifiedBy = @CreatedBy,
		DateModified = GETDATE()
	WHERE LookupGroupID = @LookupGroupID
END

END








