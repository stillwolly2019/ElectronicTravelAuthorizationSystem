CREATE TABLE [TEC].[TECItinerary] (
    [TECItineraryID]            UNIQUEIDENTIFIER CONSTRAINT [DF_TECItinerary_TECItineraryID] DEFAULT (newid()) NOT NULL,
    [TravelItineraryID]         UNIQUEIDENTIFIER NULL,
    [TravelAuthorizationNumber] NVARCHAR (14)    NULL,
    [DSARate]                   FLOAT (53)       NULL,
    [RateAmount]                FLOAT (53)       NULL,
    [ExchangeRate]              FLOAT (53)       NULL,
    [LocalAmount]               FLOAT (53)       NULL,
    [NoOfKms]                   FLOAT (53)       NULL,
    [CreatedDate]               DATETIME         CONSTRAINT [DF_TECItinerary_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [UpdatedDate]               DATETIME         NULL,
    [UpdatedBy]                 UNIQUEIDENTIFIER NULL,
    [isDeleted]                 BIT              CONSTRAINT [DF_TECItinerary_isDeleted] DEFAULT ((0)) NULL,
    [StatusCode]                NVARCHAR (50)    NULL,
    [NoOfDays]                  FLOAT (53)       NULL,
    [AllDSARates]               NVARCHAR (100)   NULL,
    [NoOfNights]                FLOAT (53)       NULL,
    CONSTRAINT [PK_TECItinerary] PRIMARY KEY CLUSTERED ([TECItineraryID] ASC) WITH (FILLFACTOR = 80)
);



