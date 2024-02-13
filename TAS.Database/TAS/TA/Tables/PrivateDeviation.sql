CREATE TABLE [TA].[PrivateDeviation] (
    [PrivateDeviationId]        UNIQUEIDENTIFIER CONSTRAINT [DF_PrivateDeviation_PrivateDeviationId] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationNumber] NVARCHAR (14)    NULL,
    [TravelItineraryID]         UNIQUEIDENTIFIER NULL,
    [DateCreated]               DATETIME         NULL,
    [CreatedBy]                 UNIQUEIDENTIFIER NULL,
    [DateModified]              DATETIME         NULL,
    [ModifiedBy]                UNIQUEIDENTIFIER NULL,
    [isDeleted]                 BIT              NULL,
    CONSTRAINT [PK_PrivateDeviation] PRIMARY KEY CLUSTERED ([PrivateDeviationId] ASC) WITH (FILLFACTOR = 80)
);



