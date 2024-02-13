CREATE TABLE [Lkp].[Comment] (
    [CommentID]    UNIQUEIDENTIFIER CONSTRAINT [DF_Table_1_NoteID] DEFAULT (newid()) NOT NULL,
    [TANumber]     NVARCHAR (50)    NULL,
    [CommentType]  NVARCHAR (3)     NULL,
    [Comment]      NVARCHAR (MAX)   NULL,
    [DateCreated]  DATETIME         CONSTRAINT [DF_Comment_DateCreated] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NOT NULL,
    [DateModified] DATETIME         CONSTRAINT [DF_Comment_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              CONSTRAINT [DF_Comment_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([CommentID] ASC) WITH (FILLFACTOR = 80)
);

