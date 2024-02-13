

CREATE PROCEDURE [TA].[UpdateWBS]
@WBSID nvarchar(100),
@WBSCode nvarchar(150),
@PercentageOrAmount float,
@Note nvarchar(500),
@CreatedBy nvarchar(100)

AS
BEGIN

UPDATE TA.WBS
SET
    WBSCode = @WBSCode, -- nvarchar
    PercentageOrAmount = @PercentageOrAmount, -- float
    Note = @Note, -- nvarchar
    UpdatedDate = GETDATE(), -- datetime
    UpdatedBy = @CreatedBy -- uniqueidentifier
WHERE WBSID = @WBSID

END
