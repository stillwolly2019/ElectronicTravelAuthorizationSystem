CREATE TABLE [TA].[StatusChangeHistory] (
    [StatusHistoryID]       UNIQUEIDENTIFIER CONSTRAINT [DF_StatusChangeHistory_StatusHistoryID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationID] UNIQUEIDENTIFIER NULL,
    [StatusCode]            NVARCHAR (50)    NULL,
    [RejectionReasons]      NVARCHAR (MAX)   NULL,
    [CreatedDate]           DATETIME         CONSTRAINT [DF_StatusChangeHistory_CreatedDate] DEFAULT (getdate()) NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_StatusChangeHistory] PRIMARY KEY CLUSTERED ([StatusHistoryID] ASC) WITH (FILLFACTOR = 80)
);



