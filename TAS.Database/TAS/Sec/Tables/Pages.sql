CREATE TABLE [Sec].[Pages] (
    [PageID]            UNIQUEIDENTIFIER CONSTRAINT [DF_Pages_PageID] DEFAULT (newid()) NOT NULL,
    [PageName]          NVARCHAR (500)   NULL,
    [PageURL]           NVARCHAR (4000)  NULL,
    [ParentID]          UNIQUEIDENTIFIER NULL,
    [PageOrder]         INT              NULL,
    [IsDisplayedInMenu] BIT              NULL,
    [DateCreated]       SMALLDATETIME    CONSTRAINT [DF_Pages_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]         UNIQUEIDENTIFIER NULL,
    [DateModified]      SMALLDATETIME    CONSTRAINT [DF_Pages_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]        UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF_Pages_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Pages_1] PRIMARY KEY CLUSTERED ([PageID] ASC) WITH (FILLFACTOR = 80)
);



