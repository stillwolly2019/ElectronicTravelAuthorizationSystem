CREATE TABLE [TEC].[TECItineraryDSA] (
    [TECItineraryDSAID] UNIQUEIDENTIFIER CONSTRAINT [DF__TECItiner__TECIt__1209AD79] DEFAULT (newid()) NOT NULL,
    [TECItineraryID]    UNIQUEIDENTIFIER NULL,
    [NoOfDays]          FLOAT (53)       NULL,
    [DSARate]           FLOAT (53)       NULL,
    [Percentage]        FLOAT (53)       NULL,
    [RateAmount]        FLOAT (53)       NULL,
    [ExchangeRate]      FLOAT (53)       NULL,
    [LocalAmount]       FLOAT (53)       NULL,
    [CreatedDate]       DATETIME         CONSTRAINT [DF__TECItiner__Creat__12FDD1B2] DEFAULT (getdate()) NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [UpdatedDate]       DATETIME         NULL,
    [UpdatedBy]         UNIQUEIDENTIFIER NULL,
    [isDeleted]         BIT              CONSTRAINT [DF__TECItiner__isDel__13F1F5EB] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TECItineraryDSA] PRIMARY KEY CLUSTERED ([TECItineraryDSAID] ASC) WITH (FILLFACTOR = 80)
);



