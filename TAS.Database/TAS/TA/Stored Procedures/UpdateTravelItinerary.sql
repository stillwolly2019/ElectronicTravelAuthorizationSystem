
CREATE PROCEDURE [TA].[UpdateTravelItinerary]

@TravelItineraryID nvarchar(100),
@ModeOfTravelID nvarchar(100),
@FromLocationCode nvarchar(100),
@FromLocationDate date,
@ToLocationCode nvarchar(100),
@ToLocationDate date,
@CreatedBy nvarchar(100)

AS
BEGIN

UPDATE TA.TravelItinerary
SET
    ModeOfTravelID = @ModeOfTravelID, -- uniqueidentifier
    FromLocationCode = @FromLocationCode, -- uniqueidentifier
    FromLocationDate = @FromLocationDate, -- date
    ToLocationCode = @ToLocationCode, -- uniqueidentifier
    ToLocationDate = @ToLocationDate, -- date
    UpdatedDate = GETDATE(), -- datetime
    UpdatedBy = @CreatedBy -- uniqueidentifier
WHERE TravelItineraryID = @TravelItineraryID
END
