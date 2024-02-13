

CREATE PROCEDURE [TA].[DeleteWBS]
@WBSID nvarchar(100),
@CreatedBy nvarchar(100)
AS
BEGIN
UPDATE TA.WBS SET 
	IsDeleted = 1,
	[UpdatedBy] = @CreatedBy,
	[UpdatedDate] = GETDATE()
WHERE WBSID= @WBSID
END



