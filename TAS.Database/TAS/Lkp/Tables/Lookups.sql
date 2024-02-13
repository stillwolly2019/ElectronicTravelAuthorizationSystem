CREATE TABLE [Lkp].[Lookups] (
    [LookupsID]       UNIQUEIDENTIFIER CONSTRAINT [DF_Lookups_ID] DEFAULT (newid()) NOT NULL,
    [LookupGroupID]   UNIQUEIDENTIFIER NULL,
    [SubGroupID]      UNIQUEIDENTIFIER NULL,
    [Code]            NVARCHAR (50)    NULL,
    [Description]     NVARCHAR (250)   NULL,
    [LongDescription] NVARCHAR (MAX)   NULL,
    [DateCreated]     SMALLDATETIME    CONSTRAINT [DF_Lookups_DateCreated] DEFAULT (getdate()) NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [DateModified]    SMALLDATETIME    CONSTRAINT [DF_Lookups_DateModified] DEFAULT ('1900-01-01') NULL,
    [ModifiedBy]      UNIQUEIDENTIFIER NULL,
    [IsDeleted]       BIT              CONSTRAINT [DF_Lookups_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Lookups] PRIMARY KEY CLUSTERED ([LookupsID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Lookups_LookupsGroups] FOREIGN KEY ([LookupGroupID]) REFERENCES [Lkp].[LookupsGroups] ([LookupGroupID])
);



