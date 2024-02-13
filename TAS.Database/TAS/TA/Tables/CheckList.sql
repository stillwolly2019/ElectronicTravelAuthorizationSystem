CREATE TABLE [TA].[CheckList] (
    [CheckListID]           UNIQUEIDENTIFIER CONSTRAINT [DF_CheckList_CheckListID] DEFAULT (newid()) NOT NULL,
    [TravelAuthorizationID] UNIQUEIDENTIFIER NULL,
    [LookupID]              UNIQUEIDENTIFIER NULL,
    [Value]                 INT              NULL,
    [Note]                  NVARCHAR (350)   NULL,
    [DateCreated]           DATETIME         CONSTRAINT [DF_CheckList_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]             UNIQUEIDENTIFIER NULL,
    [DateModified]          DATETIME         NULL,
    [ModifiedBy]            UNIQUEIDENTIFIER NULL,
    [isDeleted]             BIT              CONSTRAINT [DF_CheckList_isDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CheckList] PRIMARY KEY CLUSTERED ([CheckListID] ASC) WITH (FILLFACTOR = 80)
);



