CREATE TABLE [TA].[RejectionReason] (
    [ReasonID]              UNIQUEIDENTIFIER CONSTRAINT [DF_RejectionReason_RejectionReasonID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationID] UNIQUEIDENTIFIER NULL,
    [RejectionReasonID]     UNIQUEIDENTIFIER NULL,
    [RejectionReasonType]   CHAR (3)         NULL,
    [isDeleted]             BIT              CONSTRAINT [DF_RejectionReason_isDeleted] DEFAULT ((0)) NULL,
    [DateCreated]           DATETIME         NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_RejectionReason] PRIMARY KEY CLUSTERED ([ReasonID] ASC) WITH (FILLFACTOR = 80)
);



