
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [TA].[InsertCheckList] 
	@TravelAuthorizationID nvarchar(200),
	@LookupID nvarchar(200),
	@Value int,
	@Note nvarchar(350),
	@CreatedBy nvarchar(200)

AS
BEGIN

INSERT INTO TA.CheckList
(
    TravelAuthorizationID,
    LookupID,
    [Value],
	Note,
    DateCreated,
    CreatedBy,
    isDeleted
)
VALUES
(
    @TravelAuthorizationID, -- TravelAuthorizationID - uniqueidentifier
    @LookupID, -- LookupID - uniqueidentifier
    @Value, -- Value - int
	@Note,
    GETDATE(), -- DateCreated - datetime
    @CreatedBy, -- CreatedBy - uniqueidentifier
    0 -- isDeleted - bit
)

END

