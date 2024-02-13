


-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [Lkp].[InsertUpdateLookups]

@LookupsID nvarchar(100),
@LookupGroupID nvarchar(100),
@SubGroupID nvarchar(100),
@Code nvarchar(50),
@Description nvarchar(250),
@LongDescription nvarchar(4000),
@CreatedBy nvarchar(100)

AS
BEGIN

IF @LookupsID =''
BEGIN
	Insert into [Lkp].[Lookups] (LookupGroupID,SubGroupID,Code,[Description],LongDescription,CreatedBy) values (@LookupGroupID,@SubGroupID,@Code,@Description,@LongDescription,@CreatedBy)
END
ELSE
BEGIN
	UPDATE Lkp.Lookups
	SET
		LookupGroupID = @LookupGroupID,
		SubGroupID=@SubGroupID,
		Code=@Code,
		[Description]=@Description,
		LongDescription=@LongDescription,
		ModifiedBy = @CreatedBy,
		DateModified = GETDATE()
	WHERE LookupsID = @LookupsID
END

END









