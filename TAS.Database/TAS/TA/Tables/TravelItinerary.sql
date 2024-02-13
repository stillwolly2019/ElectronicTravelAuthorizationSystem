CREATE TABLE [TA].[TravelItinerary] (
    [TravelItineraryID]         UNIQUEIDENTIFIER CONSTRAINT [DF_TravelItinerary_TravelItineraryID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationNumber] NVARCHAR (14)    NULL,
    [ModeOfTravelID]            UNIQUEIDENTIFIER NULL,
    [FromLocationCode]          UNIQUEIDENTIFIER NULL,
    [FromLocationDate]          DATE             NULL,
    [FromLocationTime]          TIME (7)         NULL,
    [ToLocationCode]            UNIQUEIDENTIFIER NULL,
    [ToLocationDate]            DATE             NULL,
    [ToLocationTime]            TIME (7)         NULL,
    [CreatedDate]               DATETIME         CONSTRAINT [DF_TravelItinerary_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [UpdatedDate]               DATETIME         NULL,
    [UpdatedBy]                 UNIQUEIDENTIFIER NULL,
    [isDeleted]                 BIT              CONSTRAINT [DF_TravelItinerary_isDeleted] DEFAULT ((0)) NULL,
    [Ordering]                  INT              NULL,
    CONSTRAINT [PK_TravelItinerary] PRIMARY KEY CLUSTERED ([TravelItineraryID] ASC) WITH (FILLFACTOR = 80)
);



