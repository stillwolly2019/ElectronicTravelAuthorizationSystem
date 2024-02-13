CREATE TABLE [TEC].[StatusChangeHistory] (
    [StatusHistoryID]       UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationID] UNIQUEIDENTIFIER NULL,
    [StatusCode]            NVARCHAR (50)    NULL,
    [Comments]              NVARCHAR (MAX)   NULL,
    [CreatedDate]           DATETIME         DEFAULT (getdate()) NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_StatusChangeHistory_1] PRIMARY KEY CLUSTERED ([StatusHistoryID] ASC) WITH (FILLFACTOR = 80)
);



